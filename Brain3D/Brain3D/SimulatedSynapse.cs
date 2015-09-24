using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    class SimulatedSynapse : SimulatedElement
    {
        Synapse synapse;

        SimulatedNeuron pre;
        SimulatedNeuron post;

        Tuple<bool, double>[] activity;

        public SimulatedSynapse(Synapse synapse)
        {
            this.synapse = synapse;
            activity = new Tuple<bool, double>[size];

            for (int i = 0; i < size; i++)
                activity[i] = new Tuple<bool, double>(false, 0);
        }

        public void Impulse(int time)
        {
            post.Impulse(synapse.Weight, time + 20);

            for (int i = 0, j = time; i < 20 && j < activity.Length; i++, j++)
                activity[j] = new Tuple<bool, double>(true, (double)i / 20);
        }

        public Synapse Synapse
        {
            get
            {
                return synapse;
            }
        }

        public SimulatedNeuron Pre
        {
            set
            {
                pre = value;
            }
        }

        public SimulatedNeuron Post
        {
            set
            {
                post = value;
            }
        }

        public Tuple<bool, double>[] Activity
        {
            get
            {
                return activity;
            }
        }
    }
}
