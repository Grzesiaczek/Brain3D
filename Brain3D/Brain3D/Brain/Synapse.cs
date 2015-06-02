using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Brain3D
{
    class Synapse : BrainElement
    {
        #region deklaracje

        Neuron pre;
        Neuron post;

        float change;
        float factor;
        float weight;

        List<CreationData> history;
        Tuple<bool, double>[] activity;

        #endregion

        #region konstruktory

        public Synapse(Neuron pre, Neuron post)
        {
            this.pre = pre;
            this.post = post;

            change = 0;
            initialize();
        }

        #endregion

        public void impulse(int time)
        {
            post.impulse(weight, time + 20);

            for (int i = 0, j = time; i < 20 && j < activity.Length; i++, j++)
                activity[j] = new Tuple<bool, double>(true, (double)i / 20);
        }

        public void initialize()
        {
            activity = new Tuple<bool, double>[size];

            for (int i = 0; i < size; i++)
                activity[i] = new Tuple<bool, double>(false, 0);
        }

        #region właściwości

        public Neuron Pre
        {
            get
            {
                return pre;
            }
        }

        public Neuron Post
        {
            get
            {
                return post;
            }
        }
        
        public Tuple<bool, double>[] Activity
        {
            get
            {
                return activity;
            }
        }

        public float Change
        {
            get
            {
                return change;
            }
            set
            {
                change = value;
            }
        }

        public float Factor
        {
            get
            {
                return factor;
            }
            set
            {
                factor = value;
            }
        }

        public float Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
            }
        }

        #endregion
    }
}
