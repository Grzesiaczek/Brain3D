using System;
using System.Collections.Generic;

namespace Brain3D
{
    class SimulatedNeuron : SimulatedElement
    {
        #region deklaracje

        Neuron neuron;

        List<SimulatedSynapse> input;
        List<SimulatedSynapse> output;

        List<Tuple<double, int>> signals;
        NeuronActivity[] activity;
        QuerySequence query;

        int index;
        int signum;

        bool activated;
        bool refraction;

        double ratio;
        double target;
        double treshold;

        static double[] relaxation;
        static double[] activation;
        static bool initialized;

        public event EventHandler ActivateQuery;
        public static event EventHandler ActivateResponse;

        #endregion

        public SimulatedNeuron(Neuron neuron, QuerySequence query)
        {
            if (!initialized)
            {
                throw new Exception("Neuron class not initialized!");
            }

            this.neuron = neuron;
            this.query = query;

            activity = new NeuronActivity[size];
            ActivateQuery += new EventHandler(query.Activate);

            for (int i = 0; i < size; i++)
            {
                activity[i] = new NeuronActivity();
            }

            input = new List<SimulatedSynapse>();
            output = new List<SimulatedSynapse>();

            signals = new List<Tuple<double, int>>();

            treshold = 1.0;
            target = treshold;
            refraction = false;

            signum = 0;
            ratio = 0;
            index = 0;
        }

        public static void InitializeArrays()
        {
            relaxation = new double[(int)omega];
            relaxation[0] = 1;

            for (int i = 1; i < omega; i++)
            {
                relaxation[i] = Math.Pow(1 - i / omega, 4);
            }

            activation = new double[(int)tmax];
            activation[0] = 0;

            for (int i = 0; i < tmax; i++)
            {
                activation[i] = 1 - Math.Pow(1 - i / tmax, 2);
            }

            initialized = true;
        }

        public void Tick(int time)
        {
            if (!activated)
            {
                Receive(time, activity[time].Value);
                return;
            }

            double value = 0;

            if (time > 0)
            {
                value = activity[time - 1].Value;
            }

            if (index < 0)
            {
                index = 0;
            }

            if (signum > 1)
            {
                if (++index >= tmax)
                {
                    if (value > 0)
                    {
                        index = (int)((1.0 - Math.Pow(value, 0.25)) * omega);
                        signum = 1;
                    }
                    else
                    {
                        index = (int)((1.0 - Math.Pow(-value, 0.25)) * omega);
                        signum = -1;
                    }

                    activity[time].Phase = ActivityPhase.Break;
                }
                else
                {
                    value = activation[index] * target - treshold;

                    if (value >= treshold)
                    {
                        ActivateQuery(new Tuple<Neuron, int>(neuron, time), null);
                        ActivateResponse(new Tuple<QuerySequence, Neuron, int>(query, neuron, time), null);
                        activity[time].Phase = ActivityPhase.Start;

                        signals.Clear();
                        value = treshold;
                        refraction = true;
                        signum = 0;

                        foreach (SimulatedSynapse synapse in output)
                        {
                            synapse.Impulse(time);
                        }
                    }
                }
            }
            else if (signum != 0)
            {
                if (++index == omega)
                {
                    signum = 0;
                    value = 0;
                }
                else
                {
                    value = signum * relaxation[index];
                }
            }
            else if (refraction)
            {
                activity[time].Phase = ActivityPhase.Active;
                activity[time].Refraction = activity[time - 1].Refraction + 1;
                value = 1;

                if (activity[time].Refraction == 30)
                {
                    activity[time].Phase = ActivityPhase.Finish;
                    refraction = false;

                    signum = -1;
                    index = 0;
                    target = 1;
                }
            }

            activity[time].Value = value;

            if (!refraction)
            {
                Receive(time, value);
            }
        }

        void Receive(int time, double value)
        {
            List<Tuple<double, int>> removed = new List<Tuple<double, int>>();

            foreach (Tuple<double, int> signal in signals)
            {
                if (signal.Item2 == time)
                {
                    if (!activated)
                    {
                        target = treshold + value + signal.Item1;
                        activated = true;
                    }
                    else if (signum == 2)
                    {
                        target += signal.Item1;
                    }
                    else if (signum == 0)
                    {
                        target = treshold + signal.Item1;
                    }
                    else
                    {
                        int finish = index + (int)tmax;

                        if (finish >= omega)
                        {
                            target = treshold + signal.Item1;
                        }
                        else if (signum < 0)
                        {
                            target = signum * relaxation[finish] + treshold + signal.Item1;
                        }
                        else
                        {
                            target = value + treshold + signal.Item1;
                        }
                    }

                    signum = 2;

                    double factor = 1 - (value + treshold) / target;
                    activity[time].Phase = ActivityPhase.Break;

                    if (factor < 0)
                    {
                        index = (int)tmax - 1;
                    }
                    else
                    {
                        index = (int)((1 - Math.Pow(factor, 0.5)) * tmax);
                    }

                    removed.Add(signal);
                }
            }

            foreach (Tuple<double, int> signal in removed)
            {
                signals.Remove(signal);
            }
        }

        public void Impulse(double value, int time)
        {
            if (!refraction)
            {
                signals.Add(new Tuple<double, int>(value, time));
            }
        }

        public void Shot(double time)
        {
            Impulse(5, (int)time + 1);
        }

        public void Neutralize()
        {
            activated = false;
            refraction = false;

            foreach (NeuronActivity data in activity)
            {
                data.Zero();
            }
        }

        public void Calculate()
        {
            ratio = 0;

            foreach (NeuronActivity data in activity)
            {
                if (data.Phase == ActivityPhase.Active)
                {
                    ratio += 1;
                }
                else
                {
                    ratio += data.Value * data.Value;
                }
            }
        }

        #region właściwości

        public NeuronActivity[] Activity
        {
            get
            {
                return activity;
            }
        }

        public Neuron Neuron
        {
            get
            {
                return neuron;
            }
        }

        public List<SimulatedSynapse> Input
        {
            get
            {
                return input;
            }
        }

        public List<SimulatedSynapse> Output
        {
            get
            {
                return output;
            }
        }

        public double Ratio
        {
            get
            {
                return ratio;
            }
        }

        #endregion
    }
}
