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
        List<Receptor> receptors;

        List<CreationFrame> frames;
        List<Single> floats;

        double tr;
        double ts;
        double tmax;

        double omega;
        double gamma;
        double theta;

        int sentences;
        int length;

        #endregion

        public Brain()
        {
            neurons = new List<Neuron>();
            synapses = new List<Synapse>();
            receptors = new List<Receptor>();

            frames = new List<CreationFrame>();
            frames.Add(null);

            tr = 3;
            ts = 2;
            tmax = 25;

            omega = 100;
            gamma = 4;
            theta = 1;

            sentences = 0;
            length = 0;

            initialize();
        }

        void initialize()
        {
            floats = new List<Single>();
            floats.Add(0);
            floats.Add(1);

            double interval = (tmax + ts) / omega;

            for (int i = 1; i < 20; i++)
                floats.Add((float)Math.Pow(1 / (1 + i * interval), gamma));
        }

        #region sterowanie

        public void simulate(int length)
        {
            length *= 10;
            Neuron neuron = neurons.Find(k => k.Word.Equals("monkey"));
            Neuron lovely = neurons.Find(k => k.Word.Equals("small"));
            //Neuron neuron = neurons.Find(k => k.Word.Equals("jupiter"));

            if (neuron != null)
            {
                neuron.impulse(5, 50);
                neuron.impulse(5, 200);
            }

            for (int i = 0; i < length; i++)
                tick(0);
        }

        void tick(double time)
        {
            foreach (Neuron neuron in neurons)
                neuron.tick();
        }

        public void tick()
        {
            foreach (Receptor receptor in receptors)
                receptor.tick(false);

            foreach (Neuron neuron in neurons)
                neuron.tick();

            foreach (Synapse synapse in synapses)
                synapse.tick();

            length++;
        }

        public void undo()
        {
            foreach (Receptor receptor in receptors)
                receptor.undo();

            foreach (Neuron neuron in neurons)
                neuron.undo();

            foreach (Synapse synapse in synapses)
                synapse.undo();

            length--;
        }

        public void erase(bool manual)
        {
            foreach (Neuron neuron in neurons)
                neuron.clear(manual);

            foreach (Synapse synapse in synapses)
                synapse.clear(manual);

            foreach (Receptor receptor in receptors)
                receptor.erase();

            length = 0;
        }

        public void clear()
        {
            neurons.Clear();
            receptors.Clear();
            synapses.Clear();
        }

        #endregion

        #region uczenie

        public CreationSequence addSentence(String sentence)
        {
            List<CreationFrame> frames = new List<CreationFrame>();
            List<Neuron> sequence = new List<Neuron>();
            String[] words = sentence.Split(' ');

            int index = 0;
            int eta = 0;

            foreach (String word in words)
            {
                CreationFrame frame = create(word, ++sentences);
                Neuron neuron = frame.Neuron.Neuron;
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
                        frame.add(synapse);
                    }

                    synapse.Change += floats[index - i];
                }

                foreach (Synapse synapse in neuron.Output)
                {
                    float start = synapse.Weight;
                    synapse.Weight = (float)(eta * synapse.Factor * theta / (eta + (eta - 1) * synapse.Factor));

                    CreationData data = new CreationData(synapse, frame, start, synapse.Weight);
                    synapse.Changes.Add(data);
                    frame.add(data);
                }

                foreach (Synapse synapse in neuron.Input)
                {
                    if (synapse.Change == 0)
                        continue;

                    float start = synapse.Weight;
                    eta = ((Neuron)synapse.Pre).Count;
                    synapse.Factor += synapse.Change;
                    synapse.Weight = (float)(eta * synapse.Factor * theta / (eta + (eta - 1) * synapse.Factor));

                    CreationData data = new CreationData(synapse, frame, start, synapse.Weight);
                    synapse.Changes.Add(data);
                    frame.add(data);

                    synapse.Change = 0;
                }

                sequence.Add(neuron);
                frames.Add(frame);
                this.frames.Add(frame);

                index++;
            }

            return new CreationSequence(frames);
        }

        CreationFrame create(String word, int frame)
        {
            Neuron neuron = neurons.Find(i => i.Word == word);

            if (neuron == null)
            {
                neuron = new Neuron(word);
                neurons.Add(neuron);

                Receptor receptor = new Receptor();
                Synapse synapse = new Synapse(receptor, neuron);

                synapses.Add(synapse);
                receptors.Add(receptor);
                receptor.Output = synapse;
            }

            neuron.Count++;
            return new CreationFrame(neuron, frame);
        }

        public CreationFrame add(CreationSequence sequence, BuiltElement element, int frame)
        {
            CreationFrame result = create(element.Name, frame);
            Neuron neuron = result.Neuron.Neuron;
            int index = sequence.Frames.Count;

            for (int i = 0; i < index; i++)
            {
                Neuron previous = sequence.Frames[i].Neuron.Neuron;
                Synapse synapse = neuron.Input.Find(k => k.Pre.Equals(previous));

                if (synapse == null)
                {
                    synapse = new Synapse(previous, neuron);
                    synapses.Add(synapse);

                    neuron.Input.Add(synapse);
                    previous.Output.Add(synapse);
                    result.add(synapse);
                }

                synapse.Change += 1 / (float)(index - i);
            }

            foreach (Synapse synapse in neuron.Input)
            {
                if (synapse.Change == 0)
                    continue;

                int count = 0;
                float start = 0;
                float weight = 0;
                
                List<CreationData> changes = synapse.Changes;
                synapse.Factor = 0;

                foreach(CreationData cd in changes)
                {
                    if (cd.Frame > frame)
                        break;

                    count++;
                    start = cd.Weight;
                    synapse.Factor += cd.Change;
                }

                synapse.Factor += synapse.Change;
                weight = (2 * synapse.Factor) / (synapse.Factor + count + 1);

                CreationData data = new CreationData(synapse, result, start, weight);
                
                changes.Insert(count, data);
                result.add(data);
                start = weight;

                for(int i = count; i < changes.Count; i++)
                {
                    synapse.Factor += changes[i].Change;
                    weight = (2 * synapse.Factor) / (synapse.Factor + i + 1);

                    changes[i].Start = start;
                    changes[i].Weight = weight;

                    start = synapse.Weight;
                }

                synapse.Weight = weight;
                synapse.Change = 0;
            }

            for (int i = frame; i < frames.Count; i++)
                frames[i].Frame = i;

            frames.Insert(result.Frame, result);
            return result;
        }

        public void remove(CreationFrame frame)
        {

        }

        #endregion

        #region właściwości

        public List<Neuron> Neurons
        {
            get
            {
                return neurons;
            }
        }

        public List<Synapse> Synapses
        {
            get
            {
                return synapses;
            }
        }

        public List<Receptor> Receptors
        {
            get
            {
                return receptors;
            }
        }

        public int Length
        {
            get
            {
                return length;
            }
        }

        #endregion
    }
}