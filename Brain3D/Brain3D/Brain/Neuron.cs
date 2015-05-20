using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain3D
{
    class Neuron : BrainElement
    {
        #region deklaracje

        List<Synapse> input;
        List<Synapse> output;

        List<Tuple<double, int>> signals;

        NeuronData[] activity;
        String word;

        int count;
        int time;
        int index;
        int signum;

        bool active;
        int refraction;

        double target;
        double treshold;
        double value;

        static double[] relaxation;
        static double[] activation;
        static bool initialized;

        public static event EventHandler activate;

        #endregion

        #region konstruktory

        public Neuron(String word)
        {
            if (!initialized)
                throw new Exception("Neuron class not initialized!");

            this.word = word;
            treshold = 1.0;
            target = treshold;

            input = new List<Synapse>();
            output = new List<Synapse>();
            activity = new NeuronData[size];

            signals = new List<Tuple<double, int>>();

            for (int i = 0; i < size; i++)
                activity[i] = new NeuronData();
        }

        public static void initialize()
        {
            relaxation = new double[(int)omega];
            relaxation[0] = 1;

            for (int i = 1; i < omega; i++)
                relaxation[i] = Math.Pow(1 - i / omega, 4);

            activation = new double[(int)tmax];
            activation[0] = 0;

            for (int i = 0; i < tmax; i++)
                activation[i] = 1 - Math.Pow(1 - i / tmax, 2);

            initialized = true;
        }

        #endregion

        #region sterowanie

        public void tick()
        {
            if(signum > 1)
            {
                if (++index == tmax)
                {
                    index = (int)((1.0 - Math.Pow(value, 0.25)) * omega);
                    signum = 1;
                }
                else
                {
                    value = activation[index] * target - treshold;

                    if (value >= treshold)
                    {
                        signals.Clear();
                        activate(new Tuple<Neuron, int>(this, time), null);
                        value = treshold;
                        refraction = 1;
                        signum = 0;

                        foreach (Synapse synapse in output)
                            synapse.impulse(time);
                    }
                }

                activity[time].Value = value;
            }
            else if (signum != 0)
            {
                if (++index == omega)
                    signum = 0;
                else
                {
                    value = signum * relaxation[index];
                    activity[time].Value = value;
                }
            }
            else if(refraction > 0)
            {
                activity[time].Active = true;
                activity[time].Value = 1;
                activity[time].Refraction = refraction;
                              
                if (refraction++ == 30)
                {
                    activity[time + 1].Refraction = refraction;

                    refraction = 0;
                    signum = -1;
                    value = -1;
                    index = 0;
                    target = 1;
                }
            }

            List<Tuple<double, int>> removed = new List<Tuple<double, int>>();

            foreach (Tuple<double, int> signal in signals)
            {
                if (signal.Item2 == time)
                {
                    signum = 2;
                    target += signal.Item1;
                    index = (int)((1 - Math.Pow(1 - (value + treshold) / target, 0.5)) * tmax);
                    removed.Add(signal);
                }
            }

            foreach (Tuple<double, int> signal in removed)
                signals.Remove(signal);

            time++;
        }

        public void undo()
        {
        }

        public void clear(bool init)
        {
        }

        public void impulse(double value, int time)
        {
            signals.Add(new Tuple<double, int>(value, time));
        }

        public void shot(int time)
        {
            impulse(5, time);
        }

        #endregion

        #region właściwości

        public List<Synapse> Input
        {
            get
            {
                return input;
            }
        }

        public List<Synapse> Output
        {
            get
            {
                return output;
            }
        }

        public NeuronData[] Activity
        {
            get
            {
                return activity;
            }
        }

        public String Word
        {
            get
            {
                return word;
            }
        }

        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
            }
        }

        #endregion
    }
}