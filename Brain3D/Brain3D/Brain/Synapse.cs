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

        BrainElement pre;
        Neuron post;

        float change;
        float factor;
        float weight;

        List<CreationData> changes;
        Tuple<bool, double>[] activity;

        #endregion

        #region konstruktory

        public Synapse(Neuron pre, Neuron post)
        {
            this.pre = pre;
            this.post = post;
            initialize();
        }

        public Synapse(Receptor pre, Neuron post)
        {
            this.pre = pre;
            this.post = post;
            initialize();
        }

        void initialize()
        {
            activity = new Tuple<bool, double>[size];
            changes = new List<CreationData>();
            change = 0;

            for (int i = 0; i < size; i++)
                activity[i] = new Tuple<bool, double>(false, 0);
        }

        #endregion

        #region logika

        public void tick()
        {

        }

        public void impulse(int time)
        {
            post.impulse(weight, time + 20);

            for (int i = 0, j = time; i < 20; i++, j++)
                activity[j] = new Tuple<bool, double>(true, (double)i / 20);
        }

        public void undo()
        {/*
            if (activity.Count > 1)
                activity.RemoveAt(activity.Count - 1);

            if(activity[activity.Count - 1])
                ((Neuron)post).receiveSignal(weight);*/
        }

        public void clear(bool init)
        {/*
            activity.Clear();

            if (init)
                activity.Add(false);*/
        }

        #endregion

        #region właściwości

        public BrainElement Pre
        {
            get
            {
                return pre;
            }
            set
            {
                pre = value;
            }
        }

        public Neuron Post
        {
            get
            {
                return post;
            }
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
            set
            {
                activity = value;
            }
        }

        public List<CreationData> Changes
        {
            get
            {
                return changes;
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
