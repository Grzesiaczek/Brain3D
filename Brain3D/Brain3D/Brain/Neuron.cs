﻿using System;
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
        int index;
        int signum;

        bool refraction;

        double ratio;
        double target;
        double treshold;
        //double value;

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

            input = new List<Synapse>();
            output = new List<Synapse>();

            this.word = word;
        }

        public static void initializeArrays()
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

        public void initialize()
        {
            signals = new List<Tuple<double, int>>();
            activity = new NeuronData[size];

            for (int i = 0; i < size; i++)
                activity[i] = new NeuronData();

            treshold = 1.0;
            target = treshold;
        }

        public void tick(int time)
        {
            double value = 0;

            if (time > 0)
                value = activity[time - 1].Value;

            if (index < 0)
                index = 0;

            if(signum > 1)
            {
                if (++index >= tmax)
                {
                    index = (int)((1.0 - Math.Pow(value, 0.25)) * omega);
                    signum = 1;
                }
                else
                {
                    value = activation[index] * target - treshold;

                    if (value >= treshold)
                    {
                        activate(new Tuple<Neuron, int>(this, time), null);

                        activity[time].Active = true;
                        activity[time].Treshold = true;

                        signals.Clear();
                        value = treshold;
                        refraction = true;
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
                    activity[time].Value = signum * relaxation[index];
            }
            else if(refraction)
            {
                activity[time].Active = true;
                activity[time].Value = 1;
                activity[time].Refraction = activity[time - 1].Refraction + 1;

                if (activity[time].Refraction == 30)
                {
                    activity[time].Treshold = true;
                    refraction = false;

                    signum = -1;
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

                    double factor = 1 - (value + treshold) / target;

                    if (factor < 0)
                        index = (int)tmax - 1;
                    else
                        index = (int)((1 - Math.Pow(factor, 0.5)) * tmax);

                    removed.Add(signal);
                }
            }

            foreach (Tuple<double, int> signal in removed)
                signals.Remove(signal);
        }

        public void impulse(double value, int time)
        {
            if(!refraction)
                signals.Add(new Tuple<double, int>(value, time));
        }

        public void shot(double time)
        {
            impulse(5, (int)time + 1);
        }

        public void calculate()
        {
            ratio = 0;

            foreach(NeuronData data in activity)
            {
                if (data.Active)
                    ratio += 1;
                else
                    ratio += data.Value * data.Value;
            }
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

        public double Ratio
        {
            get
            {
                return ratio;
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