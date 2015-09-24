using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Brain3D
{
    class Synapse
    {
        #region deklaracje

        Neuron pre;
        Neuron post;

        float change;
        float factor;
        float weight;

        List<CreationData> history;
        
        Dictionary<QuerySequence, SimulatedSynapse> data;

        #endregion

        #region konstruktory

        public Synapse(Neuron pre, Neuron post)
        {
            this.pre = pre;
            this.post = post;

            change = 0;
            data = new Dictionary<QuerySequence, SimulatedSynapse>();
        }

        #endregion

        public void Initialize(QuerySequence query)
        {
            SimulatedSynapse synapse = new SimulatedSynapse(this);

            if (data.ContainsKey(query))
                data[query] = synapse;
            else
                data.Add(query, synapse);
        }

        public SimulatedSynapse GetSimulated(QuerySequence query)
        {
            return data[query];
        }

        public void SetSimulated(QuerySequence query)
        {
            SimulatedSynapse synapse = data[query];
            synapse.Pre = pre.GetSimulated(query);
            synapse.Post = post.GetSimulated(query);
        }

        #region właściwości

        public Tuple<bool, double>[] Activity
        {
            get
            {
                return data[Constant.Query].Activity;
            }
        }

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
