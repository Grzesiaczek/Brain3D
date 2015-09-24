using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain3D
{
    class Neuron
    {
        #region deklaracje

        List<Synapse> input;
        List<Synapse> output;

        Dictionary<QuerySequence, SimulatedNeuron> data;
        String word;

        int count;

        #endregion

        #region konstruktory

        public Neuron(String word)
        {
            data = new Dictionary<QuerySequence, SimulatedNeuron>();
            input = new List<Synapse>();
            output = new List<Synapse>();
            
            this.word = word;
        }

        #endregion

        #region sterowanie

        public void Initialize(QuerySequence query)
        {
            SimulatedNeuron neuron = new SimulatedNeuron(this);

            if (data.ContainsKey(query))
                data[query] = neuron;
            else
                data.Add(query, neuron);
        }

        public SimulatedNeuron GetSimulated(QuerySequence query)
        {
            return data[query];
        }

        public SimulatedNeuron GetSimulated()
        {
            return data[Constant.Query];
        }

        public void SetSimulated(QuerySequence query)
        {
            SimulatedNeuron neuron = data[query];
            List<SimulatedSynapse> input = neuron.Input;
            List<SimulatedSynapse> output = neuron.Output;

            input.Clear();
            output.Clear();

            foreach (Synapse synapse in Input)
                input.Add(synapse.GetSimulated(query));

            foreach (Synapse synapse in Output)
                output.Add(synapse.GetSimulated(query));
        }

        #endregion

        #region właściwości

        public NeuronActivity[] Activity
        {
            get
            {
                return data[Constant.Query].Activity;
            }
        }

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