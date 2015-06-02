using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Brain3D
{
    class Brain
    {
        #region deklaracje

        List<Neuron> neurons;  
        List<Synapse> synapses;

        List<CreationFrame> frames;
        List<CreationSequence> sequences;
        List<Single> floats;

        Dictionary<Neuron, Tuple<AnimatedNeuron, CreatedNeuron>> mapNeurons;
        Dictionary<Synapse, Tuple<AnimatedSynapse, CreatedSynapse>> mapSynapses;
        Dictionary<Synapse, Tuple<AnimatedState, CreatedState>> mapStates;

        QuerySequence sequence;

        double tr;
        double ts;
        double tmax;

        double omega;
        double gamma;
        double theta;

        int length;
        int sentences;

        #endregion

        public Brain()
        {
            neurons = new List<Neuron>();
            synapses = new List<Synapse>();

            mapNeurons = new Dictionary<Neuron, Tuple<AnimatedNeuron, CreatedNeuron>>();
            mapSynapses = new Dictionary<Synapse, Tuple<AnimatedSynapse, CreatedSynapse>>();
            mapStates = new Dictionary<Synapse, Tuple<AnimatedState, CreatedState>>();

            frames = new List<CreationFrame>();
            sequences = new List<CreationSequence>();
            frames.Add(null);

            tr = 3;
            ts = 2;
            tmax = 20;

            omega = 100;
            gamma = 4;
            theta = 1;

            sentences = 0;
            BrainElement.initialize(omega, tmax);

            floats = new List<Single>();
            floats.Add(0);
            floats.Add(1);

            double interval = (tmax + ts) / omega;

            for (int i = 1; i < 20; i++)
                floats.Add((float)Math.Pow(1 / (1 + i * interval), gamma));
        }

        #region sterowanie

        public void simulate(QuerySequence sequence, int length)
        {
            this.sequence = sequence;
            this.length = length * 10 + 1;
            simulate(0);
        }

        public void simulate(int start)
        {
            for (int i = start; i < length; i++)
            {
                sequence.tick(i);

                foreach (Neuron neuron in neurons)
                    neuron.tick(i);
            }
        }

        public void clear()
        {
            neurons.Clear();
            synapses.Clear();
            sequences.Clear();
        }

        public void initialize()
        {
            BrainElement.initialize(250);

            foreach (Neuron neuron in neurons)
                neuron.initialize();

            foreach (Synapse synapse in synapses)
                synapse.initialize();
        }

        #endregion

        #region uczenie

        public void addSentence(String sentence)
        {
            List<CreationFrame> frames = new List<CreationFrame>();
            List<Neuron> sequence = new List<Neuron>();
            String[] words = sentence.Split(' ');

            int index = 0;
            int eta = 0;

            foreach (String word in words)
            {
                CreationFrame frame = create(word, ++sentences);
                Neuron neuron = frame.Neuron.Neuron.Neuron;
                eta = neuron.Count;

                for (int i = 0; i < index; i++)
                {
                    Synapse synapse = neuron.Input.Find(k => k.Pre.Equals(sequence[i]));

                    if (synapse == null)
                    {
                        synapse = new Synapse(sequence[i], neuron);
                        synapses.Add(synapse);

                        neuron.Input.Add(synapse);
                        sequence[i].Output.Add(synapse);
                        frame.add(create(synapse));
                    }

                    synapse.Change += floats[index - i];
                }

                foreach (Synapse synapse in neuron.Output)
                {
                    float start = synapse.Weight;
                    synapse.Weight = (float)(eta * synapse.Factor * theta / (eta + (eta - 1) * synapse.Factor));

                    if (start == synapse.Weight)
                        continue;

                    CreationData data = new CreationData(mapStates[synapse].Item2, frame, new Change(start, synapse.Weight), new Change(synapse.Factor));
                    mapStates[synapse].Item2.add(data);
                    frame.add(data);
                }

                foreach (Synapse synapse in neuron.Input)
                {
                    if (synapse.Change == 0)
                        continue;

                    float start = synapse.Weight;
                    float factor = synapse.Factor + synapse.Change;
                    
                    eta = ((Neuron)synapse.Pre).Count;
                    synapse.Weight = (float)(eta * factor * theta / (eta + (eta - 1) *factor));

                    CreationData data = new CreationData(mapStates[synapse].Item2, frame, new Change(start, synapse.Weight), new Change(synapse.Factor, factor));
                    mapStates[synapse].Item2.add(data);
                    frame.add(data);

                    synapse.Change = 0;
                    synapse.Factor = factor;
                }

                sequence.Add(neuron);
                frames.Add(frame);
                this.frames.Add(frame);

                index++;
            }

            sequences.Add(new CreationSequence(frames));
        }

        CreatedNeuron create(Neuron neuron)
        {
            AnimatedNeuron animated = new AnimatedNeuron(neuron, Constant.randomPoint());
            CreatedNeuron created = new CreatedNeuron(animated);
            mapNeurons.Add(neuron, new Tuple<AnimatedNeuron,CreatedNeuron>(animated, created));
            return created;
        }

        CreatedSynapse create(Synapse synapse)
        {
            AnimatedSynapse animated = null;
            CreatedSynapse created = null;

            if (mapSynapses.ContainsKey(synapse))
            {
                animated = mapSynapses[synapse].Item1;
                created = mapSynapses[synapse].Item2;
                created.setDuplex(synapse);
                mapStates.Add(synapse, new Tuple<AnimatedState, CreatedState>(animated.Duplex, created.Duplex));
            }
            else
            {
                AnimatedNeuron pre = mapNeurons[synapse.Pre].Item1;
                AnimatedNeuron post = mapNeurons[synapse.Post].Item1;

                animated = new AnimatedSynapse(pre, post, synapse);
                created = new CreatedSynapse(animated);
                mapStates.Add(synapse, new Tuple<AnimatedState, CreatedState>(animated.State, created.State));
            }

            mapSynapses.Add(synapse, new Tuple<AnimatedSynapse, CreatedSynapse>(animated, created));
            return created;
        }

        CreationFrame create(String word, int frame)
        {
            Neuron neuron = neurons.Find(i => i.Word == word);
            CreatedNeuron created = null;

            if (neuron == null)
            {
                neuron = new Neuron(word);
                neurons.Add(neuron);
                created = create(neuron);
            }
            else
                created = mapNeurons[neuron].Item2;

            neuron.Count++;
            return new CreationFrame(created, frame);
        }

        //do przebudowy
        public CreationFrame add(CreationSequence sequence, BuiltTile element, int frame)
        {
            CreationFrame result = create(element.Word, frame);
            Neuron neuron = result.Neuron.Neuron.Neuron;

            frames.Insert(result.Frame, result);
            return result;
        }

        public void remove(CreationFrame frame)
        {

        }

        #endregion

        #region właściwości

        public List<CreationSequence> Sequences
        {
            get
            {
                return sequences;
            }
        }

        public Dictionary<Neuron, Tuple<AnimatedNeuron, CreatedNeuron>> Neurons
        {
            get
            {
                return mapNeurons;
            }
        }

        public Dictionary<Synapse, Tuple<AnimatedSynapse, CreatedSynapse>> Synapses
        {
            get
            {
                return mapSynapses;
            }
        }

        public Dictionary<Synapse, Tuple<AnimatedState, CreatedState>> States
        {
            get
            {
                return mapStates;
            }
        }

        #endregion
    }
}