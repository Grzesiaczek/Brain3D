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

        enum Phase { Zero, One, Two, Three, Finish }

        Phase phase;

        CreatedNeuron neuron;
        CreationSequence sequence;
        SequenceTile tile;

        List<CreatedVector> vectors;
        List<CreationData> data;

        int frame;
        double time;

        #endregion

        public CreationFrame(CreatedNeuron neuron, int frame)
        {
            this.neuron = neuron;
            this.frame = frame;

            vectors = new List<CreatedVector>();
            data = new List<CreationData>();

            tile = new SequenceTile(neuron.Neuron.Word);
            phase = Phase.Zero;
        }

        public void create()
        {
            if (!neuron.Created)
            {
                neuron.show();
                neuron.create();
            }

            foreach (CreatedVector synapse in vectors)
            {
                synapse.show();
                synapse.create();
            }

            foreach (CreationData cd in data)
                cd.execute();
        }

        public void activate()
        {
            tile.activate();
            neuron.activate();
        }

        public void idle()
        {
            tile.idle();
            neuron.idle();
        }

        public void undo()
        {
            idle();

            if(neuron.Frame == frame)
                neuron.hide();

            foreach (CreatedVector vector in vectors)
                vector.hide();

            foreach (CreationData cd in data)
                cd.undo();
        }

        public void execute()
        {
            foreach (CreationData cd in data)
                cd.execute();
        }

        public bool tick(double interval)
        {
            time += interval;
            float scale = (float)(time / 20);

            switch (phase)
            {
                case Phase.Zero:
                    neuron.Scale = 0;
                    neuron.show();
                    time = 50;
                    break;
                case Phase.One:
                    neuron.Scale = scale;
                    break;
                case Phase.Two:
                    foreach(CreatedVector synapse in vectors)
                        synapse.Scale = scale;

                    break;
                case Phase.Three:
                    foreach (CreationData cd in data)
                        cd.tick(scale);

                    break;
            }

            if(time > 20)
            {
                bool finish = false;
                time = 0;

                while (!finish)
                {
                    switch (phase)
                    {
                        case Phase.Zero:
                            phase = Phase.One;

                            if(!neuron.Created)
                                finish = true;

                            break;
                        case Phase.One:
                            neuron.create();
                            phase = Phase.Two;

                            foreach (CreatedVector synapse in vectors)
                                synapse.init();

                            if(vectors.Count != 0)
                                finish = true;

                            break;
                        case Phase.Two:
                            if(data.Count == 0)
                                return true;

                            phase = Phase.Three;
                            finish = true;

                            break;
                        case Phase.Three:
                            foreach (CreatedVector synapse in vectors)
                                synapse.create();

                            return true;
                    }
                }
            }

            return false;
        }

        public void add(CreationData data)
        {
            this.data.Add(data);
        }

        public void add(CreatedVector synapse)
        {
            vectors.Add(synapse);
        }

        #region właściwości

        public CreationSequence Sequence
        {
            get
            {
                return sequence;
            }
            set
            {
                sequence = value;
            }
        }

        public CreatedNeuron Neuron
        {
            get
            {
                return neuron;
            }
        }

        public SequenceTile Tile
        {
            get
            {
                return tile;
            }
        }

        public int Frame
        {
            get
            {
                return frame;
            }
        }

        #endregion
    }
}
