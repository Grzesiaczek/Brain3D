using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain3D
{
    class CreationFrame
    {
        #region deklaracje

        SequenceNeuron neuron;

        List<Synapse> created;
        List<CreationData> data;

        static Dictionary<Neuron, CreatedNeuron> neurons;
        static Dictionary<Synapse, CreatedSynapse> synapses;

        int frame;
        int count;
        int phase;

        static int interval;

        public event EventHandler finish;

        #endregion

        public CreationFrame(Neuron neuron, int frame)
        {
            this.neuron = new SequenceNeuron(neuron);
            created = new List<Synapse>();
            data = new List<CreationData>();

            this.frame = frame;
            count = 0;
            phase = 1;
        }

        public static void setDictionary(Dictionary<Neuron, CreatedNeuron> mapNeurons, Dictionary<Synapse, CreatedSynapse> mapSynapses)
        {
            neurons = mapNeurons;
            synapses = mapSynapses;
        }

        public void create()
        {
            CreatedNeuron cn = neurons[neuron.Neuron];

            if (!cn.Created)
                cn.show();

            foreach (Synapse synapse in created)
                synapses[synapse].show();
        }

        public void undo()
        {
            CreatedNeuron cn = neurons[neuron.Neuron];
            neuron.changeType(SequenceElementType.Normal);

            if(cn.Frame == frame)
                cn.hide();

            foreach (Synapse synapse in created)
                synapses[synapse].hide();

            foreach (CreationData cd in data)
                synapses[cd.Synapse].Synapse.setWeight(cd, true);
        }

        public void execute()
        {
            neuron.changeType(SequenceElementType.Normal);

            foreach (CreationData cd in data)
                synapses[cd.Synapse].Synapse.setWeight(cd);
        }

        public void change()
        {
            neuron.changeType(SequenceElementType.Active);

            foreach (CreationData cd in data)
                synapses[cd.Synapse].Synapse.change(cd);
        }

        public void create2()
        {
            foreach (CreationData cd in data)
                synapses[cd.Synapse].Synapse.create(cd);
        }

        public void tick()
        {
            float factor = (float)count / interval;

            switch (phase)
            {
                case 1:
                    if (neurons[neuron.Neuron].Created)
                        phase = 2;
                    else
                        ;//neurons[neuron.Neuron].draw(factor);

                    break;
                case 2:
                    if (created.Count == 0)
                        phase = 3;
                    /*
                    foreach (Synapse s in created)
                        synapses[s].draw(factor);*/

                    break;
                case 3:
                    foreach (CreationData cd in data)
                    {
                        CreatedSynapse cs = synapses[cd.Synapse];
                        cs.tick(cd);
                    }
                    break;
            }

            if(++count == interval)
            {
                switch(phase++)
                {
                    case 1:
                        if (synapses.Count == 0)
                            phase++;

                        CreatedNeuron cn = neurons[neuron.Neuron];
                        cn.show();
                        cn.Frame = frame;
                        finish(cn, null);
                        break;
                    case 2:
                        finish(created, null);
                        break;
                    case 3:
                        create();
                        finish(this, null);
                        break;
                }

                count = 0;
            }
        }

        public void setInterval(int value)
        {
            count *= (int)((float)value / interval);
            interval = value;
        }

        public void step()
        {
            foreach (CreationData cd in data)
                cd.Step = (cd.Weight - cd.Start) / interval;
        }

        public void add(CreationData data)
        {
            this.data.Add(data);
        }

        public void add(Synapse synapse)
        {
            created.Add(synapse);
        }

        #region właściwości

        public SequenceNeuron Neuron
        {
            get
            {
                return neuron;
            }
        }

        public List<CreationData> Data
        {
            get
            {
                return data;
            }
        }

        public int Frame
        {
            get
            {
                return frame;
            }
            set
            {
                frame = value;
            }
        }

        public static int Interval
        {
            set
            {
                interval = value;
            }
        }

        #endregion
    }
}
