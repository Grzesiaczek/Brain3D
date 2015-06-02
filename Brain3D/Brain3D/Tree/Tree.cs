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
            List<Synapse> synapses = new List<Synapse>(brain.Synapses.Keys);

            foreach (Leaf source in leafs)
            {
                Synapse synapse = synapses.Find(k => k.Pre == source.Neuron && k.Post == leaf.Neuron);

                if(synapse != null)
                    branches.Add(new Branch(source, leaf));
            }

            leafs.Add(leaf);
        }

        public void clear()
        {
            foreach (Branch branch in branches)
                branch.hide();

            foreach (Leaf leaf in leafs)
                leaf.hide();

            branches.Clear();
            leafs.Clear();
        }

        public override void show()
        {
            foreach (Branch branch in branches)
                branch.show();

            foreach (Leaf leaf in leafs)
                leaf.show();

            display.show(this);
        }
    }
}
