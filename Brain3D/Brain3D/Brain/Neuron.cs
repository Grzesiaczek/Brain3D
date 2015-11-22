using System;
using System.Collections.Generic;

namespace Brain3D
{
    class Neuron
    {
        #region deklaracje

        List<Synapse> input;
        List<Synapse> output;

        Dictionary<QuerySequence, SimulatedNeuron> data;
        QueryContainer queryContainer;
        string word;

        int count;

        #endregion

        #region konstruktory

        public Neuron(string word, QueryContainer container)
        {
            data = new Dictionary<QuerySequence, SimulatedNeuron>();
            input = new List<Synapse>();
            output = new List<Synapse>();

            queryContainer = container;
            this.word = word;
        }

        #endregion

        #region sterowanie

        public void Initialize(QuerySequence query)
        {
            SimulatedNeuron neuron = new SimulatedNeuron(this, query);

            if (data.ContainsKey(query))
            {
                data[query] = neuron;
            }
            else
            {
                data.Add(query, neuron);
            }
        }

        public SimulatedNeuron GetSimulated(QuerySequence query)
        {
            return data[query];
        }

        public SimulatedNeuron GetSimulated()
        {
            return data[queryContainer.Query];
        }

        public void SetSimulated(QuerySequence query)
        {
            SimulatedNeuron neuron = data[query];
            List<SimulatedSynapse> input = neuron.Input;
            List<SimulatedSynapse> output = neuron.Output;

            input.Clear();
            output.Clear();

            foreach (Synapse synapse in Input)
            {
                input.Add(synapse.GetSimulated(query));
            }

            foreach (Synapse synapse in Output)
            {
                output.Add(synapse.GetSimulated(query));
            }
        }

        #endregion

        #region właściwości

        public NeuronActivity[] Activity
        {
            get
            {
                return data[queryContainer.Query].Activity;
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