using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    class Tree : TimeLine
    {
        List<Leaf> leafs;
        List<Branch> branches;

        Random random;
        TreeLayout layout;

        public Tree()
        {
            leafs = new List<Leaf>();
            branches = new List<Branch>();
            layout = new TreeLayout();

            random = new Random();
            Neuron.activate += new EventHandler(activate);
        }

        void activate(object sender, EventArgs e)
        {
            Tuple<Neuron, int> tuple = (Tuple<Neuron, int>)sender;
            Leaf leaf = new Leaf(tuple.Item1, tuple.Item2, (float)(2 * random.NextDouble() - 0.5));
            List<Synapse> synapses = new List<Synapse>(brain.Vectors.Keys);

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
            branches.Clear();
            leafs.Clear();
        }

        public override void show()
        {
            foreach (Branch branch in branches)
                branch.show();

            foreach (Leaf leaf in leafs)
                leaf.show();

            layout.show();
            base.show();
        }

        public override void hide()
        {
            foreach (Branch branch in branches)
                branch.hide();

            foreach (Leaf leaf in leafs)
                leaf.hide();

            layout.hide();
            base.hide();
        }

        protected override void rescale()
        {
            foreach (Leaf leaf in leafs)
                leaf.Scale = scale;

            foreach (Branch branch in branches)
                branch.move();

            layout.Scale = scale;
        }
    }
}
