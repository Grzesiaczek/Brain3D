using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    class Tree : Presentation
    {
        List<Leaf> leafs;
        List<Branch> branches;

        public Tree()
        {
            leafs = new List<Leaf>();
            branches = new List<Branch>();

            Neuron.activate += new EventHandler(activate);
        }

        void activate(object sender, EventArgs e)
        {
            Tuple<Neuron, int> tuple = (Tuple<Neuron, int>)sender;
            Leaf leaf = new Leaf(tuple.Item1, tuple.Item2);

            foreach (Leaf source in leafs)
            {
                Synapse synapse = brain.Synapses.Find(k => k.Pre == source.Neuron && k.Post == leaf.Neuron);

                if(synapse != null)
                    branches.Add(new Branch(source, leaf));
            }

            leafs.Add(leaf);
        }

        protected override void brainLoaded(object sender, EventArgs e)
        {
            base.brainLoaded(sender, e);
        }

        public override void show()
        {
            foreach (Branch branch in branches)
                display.add(branch);

            foreach (Leaf leaf in leafs)
                display.add(leaf);
        }
    }
}
