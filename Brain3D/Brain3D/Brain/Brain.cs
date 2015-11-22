using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Brain3D
{
    class Brain
    {
        #region deklaracje

        List<CreationFrame> frames;
        List<CreationSequence> sequences;
        List<Single> floats;

        HashSet<Neuron> neurons;
        HashSet<Synapse> synapses;

        Dictionary<Neuron, Tuple<AnimatedNeuron, CreatedNeuron>> mapNeurons;
        Dictionary<Synapse, Tuple<AnimatedVector, CreatedVector>> mapVectors;
        Dictionary<Synapse, Tuple<AnimatedSynapse, CreatedSynapse>> mapSynapses;

        QuerySequence query;
        QueryContainer queryContainer;

        double tr;
        double ts;
        double tmax;

        double omega;
        double gamma;
        double theta;

        int length;
        int sentences;

        public static event EventHandler SimulationFinished;

        #endregion

        public Brain()
        {
            neurons = new HashSet<Neuron>();
            synapses = new HashSet<Synapse>();

            mapNeurons = new Dictionary<Neuron, Tuple<AnimatedNeuron, CreatedNeuron>>();
            mapVectors = new Dictionary<Synapse, Tuple<AnimatedVector, CreatedVector>>();
            mapSynapses = new Dictionary<Synapse, Tuple<AnimatedSynapse, CreatedSynapse>>();

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

            floats = new List<Single>();
            floats.Add(0);
            floats.Add(1);

            double interval = (tmax + ts) / omega;

            for (int i = 1; i < 20; i++)
            {
                floats.Add((float)Math.Pow(1 / (1 + i * interval), gamma));
            }
        }

        public void Load(XmlNode node, Display display)
        {
            XmlNode sentences = node.FirstChild;
            XmlNode queries = sentences.NextSibling;
            queryContainer = new QueryContainer(this, display);

            foreach(XmlNode sentence in sentences)
            {
                AddSentence(sentence.InnerText);
            }

            foreach(XmlNode query in queries)
            {
                length = 250;
                queryContainer.Add(new QuerySequence(query.InnerText, length * 10 + 1));
            }
        }

        #region sterowanie

        public void Simulate(QuerySequence sequence, int length)
        {
            this.query = sequence;
            this.length = length * 10 + 1;

            //Constant.Query = sequence;
            sequence.Initialize();
            Simulate(0);
        }

        public void Simulate(int start)
        {
            List<SimulatedNeuron> data = neurons.Select(n => n.GetSimulated(query)).ToList();

            foreach (SimulatedNeuron neuron in data)
            {
                neuron.Neutralize();
            }

            for (int i = start; i < length; i++)
            {
                query.Tick(i);

                foreach (SimulatedNeuron neuron in data)
                {
                    neuron.Tick(i);
                }
            }

            query.LoadTiles();
            SimulationFinished(query, null);
        }

        public void Initialize(QuerySequence query)
        {
            SimulatedElement.Initialize(250);

            foreach (Neuron neuron in neurons)
            {
                neuron.Initialize(query);
            }

            foreach (Synapse synapse in synapses)
            {
                synapse.Initialize(query);
            }

            foreach (Neuron neuron in neurons)
            {
                neuron.SetSimulated(query);
            }

            foreach (Synapse synapse in synapses)
            {
                synapse.SetSimulated(query);
            }
        }

        #endregion

        #region uczenie

        public void AddSentence(string sentence)
        {
            List<CreationFrame> frames = new List<CreationFrame>();
            List<Neuron> sequence = new List<Neuron>();
            string[] words = sentence.Split(' ');

            int index = 0;
            int eta = 0;

            foreach (string word in words)
            {
                CreationFrame frame = Create(word, ++sentences);
                Neuron neuron = frame.Neuron.Neuron.Neuron;
                eta = neuron.Count;

                for (int i = 0; i < index; i++)
                {
                    if (sequence[i] == neuron)
                        continue;

                    Synapse synapse = neuron.Input.Find(k => k.Pre.Equals(sequence[i]));

                    if (synapse == null)
                    {
                        synapse = new Synapse(sequence[i], neuron, queryContainer);
                        synapses.Add(synapse);

                        neuron.Input.Add(synapse);
                        sequence[i].Output.Add(synapse);
                        frame.Add(Create(synapse));
                    }

                    synapse.Change += floats[index - i];
                }

                foreach (Synapse synapse in neuron.Output)
                {
                    float start = synapse.Weight;
                    synapse.Weight = (float)(eta * synapse.Factor * theta / (eta + (eta - 1) * synapse.Factor));

                    if (start == synapse.Weight)
                    {
                        continue;
                    }

                    CreationData data = new CreationData(mapSynapses[synapse].Item2, frame, new Change(start, synapse.Weight), new Change(synapse.Factor));
                    mapSynapses[synapse].Item2.add(data);
                    frame.Add(data);
                }

                foreach (Synapse synapse in neuron.Input)
                {
                    if (synapse.Change == 0)
                    {
                        continue;
                    }

                    float start = synapse.Weight;
                    float factor = synapse.Factor + synapse.Change;
                    
                    eta = synapse.Pre.Count;
                    synapse.Weight = (float)(eta * factor * theta / (eta + (eta - 1) * factor));

                    CreationData data = new CreationData(mapSynapses[synapse].Item2, frame, new Change(start, synapse.Weight), new Change(synapse.Factor, factor));
                    mapSynapses[synapse].Item2.add(data);
                    frame.Add(data);

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

        CreatedNeuron Create(Neuron neuron)
        {
            AnimatedNeuron animated = new AnimatedNeuron(neuron, Constant.RandomPoint());
            CreatedNeuron created = new CreatedNeuron(animated);
            mapNeurons.Add(neuron, new Tuple<AnimatedNeuron,CreatedNeuron>(animated, created));
            return created;
        }

        CreatedVector Create(Synapse synapse)
        {
            AnimatedNeuron pre = mapNeurons[synapse.Pre].Item1;
            AnimatedNeuron post = mapNeurons[synapse.Post].Item1;

            Synapse duplexed = mapVectors.Keys.FirstOrDefault(k => k.Pre == synapse.Post && k.Post == synapse.Pre);

            AnimatedVector animated = null;
            CreatedVector created = null;

            if (duplexed != null)
            {
                animated = mapVectors[duplexed].Item1;
                created = mapVectors[duplexed].Item2;

                created.setDuplex(synapse);
                mapSynapses.Add(synapse, new Tuple<AnimatedSynapse, CreatedSynapse>(animated.Duplex, created.Duplex));
            }
            else
            {
                animated = new AnimatedVector(pre, post, synapse);
                created = new CreatedVector(animated);
                mapSynapses.Add(synapse, new Tuple<AnimatedSynapse, CreatedSynapse>(animated.State, created.State));
            }

            mapVectors.Add(synapse, new Tuple<AnimatedVector, CreatedVector>(animated, created));
            return created;
        }

        CreationFrame Create(String word, int frame)
        {
            Neuron neuron = neurons.FirstOrDefault(k => k.Word == word); 
            CreatedNeuron created = null;

            if (neuron == null)
            {
                neuron = new Neuron(word, queryContainer);
                neurons.Add(neuron);
                created = Create(neuron);
            }
            else
                created = mapNeurons[neuron].Item2;

            neuron.Count++;
            return new CreationFrame(created, frame);
        }

        //do przebudowy
        public CreationFrame Add(CreationSequence sequence, BuiltTile element, int frame)
        {
            CreationFrame result = Create(element.Word, frame);
            Neuron neuron = result.Neuron.Neuron.Neuron;

            frames.Insert(result.Frame, result);
            return result;
        }

        public void Remove(CreationFrame frame)
        {

        }

        #endregion

        #region właściwości

        public QueryContainer QueryContainer
        {
            get
            {
                return queryContainer;
            }
        }

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

        public Dictionary<Synapse, Tuple<AnimatedVector, CreatedVector>> Vectors
        {
            get
            {
                return mapVectors;
            }
        }

        public Dictionary<Synapse, Tuple<AnimatedSynapse, CreatedSynapse>> Synapses
        {
            get
            {
                return mapSynapses;
            }
        }

        #endregion
    }
}