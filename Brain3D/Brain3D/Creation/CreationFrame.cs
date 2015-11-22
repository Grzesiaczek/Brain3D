﻿using System.Collections.Generic;

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

        public void Create()
        {
            if (!neuron.Created)
            {
                neuron.Show();
                neuron.create();
            }

            foreach (CreatedVector synapse in vectors)
            {
                synapse.Show();
                synapse.create();
            }

            foreach (CreationData cd in data)
            {
                cd.Execute();
            }
        }

        public void Activate()
        {
            tile.Activate();
            neuron.activate();
        }

        public void Idle()
        {
            tile.Idle();
            neuron.Idle();
        }

        public void Undo()
        {
            Idle();

            if (neuron.Frame == frame)
            {
                neuron.Hide();
            }

            foreach (CreatedVector vector in vectors)
            {
                vector.Hide();
            }

            foreach (CreationData cd in data)
            {
                cd.Undo();
            }
        }

        public void Execute()
        {
            foreach (CreationData cd in data)
            {
                cd.Execute();
            }
        }

        public bool Tick(double interval)
        {
            time += interval;
            float scale = (float)(time / 20);

            switch (phase)
            {
                case Phase.Zero:
                    neuron.Scale = 0;
                    neuron.Show();
                    time = 50;
                    break;

                case Phase.One:
                    neuron.Scale = scale;
                    break;

                case Phase.Two:
                    foreach (CreatedVector synapse in vectors)
                    {
                        synapse.Scale = scale;
                    }

                    break;

                case Phase.Three:

                    foreach (CreationData cd in data)
                    {
                        cd.Tick(scale);
                    }

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

                            if (!neuron.Created)
                            {
                                finish = true;
                            }

                            break;

                        case Phase.One:
                            neuron.create();
                            phase = Phase.Two;

                            foreach (CreatedVector synapse in vectors)
                            {
                                synapse.init();
                            }

                            if (vectors.Count != 0)
                            {
                                finish = true;
                            }

                            break;

                        case Phase.Two:
                            if (data.Count == 0)
                            {
                                return true;
                            }

                            phase = Phase.Three;
                            finish = true;

                            break;

                        case Phase.Three:
                            foreach (CreatedVector synapse in vectors)
                            {
                                synapse.create();
                            }

                            return true;
                    }
                }
            }

            return false;
        }

        public void Add(CreationData data)
        {
            this.data.Add(data);
        }

        public void Add(CreatedVector synapse)
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
