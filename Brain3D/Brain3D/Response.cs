using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    class Response : TimeLine
    {
        List<Leaf> leafs;
        List<Tree> wood;

        Dictionary<Tuple<Neuron, int>, Leaf> map;
        TreeLayout layout;
        Tree tree;

        int index;

        public Response()
        {
            leafs = new List<Leaf>();
            wood = new List<Tree>();

            layout = new TreeLayout();
            map = new Dictionary<Tuple<Neuron, int>, Leaf>();
            Neuron.activate += new EventHandler(activate);
        }

        void activate(object sender, EventArgs e)
        {
            Tuple<Neuron, int> tuple = (Tuple<Neuron, int>)sender;
            Leaf leaf = new Leaf(tuple.Item1, tuple.Item2);
            leafs.Add(leaf);
            map.Add(tuple, leaf);
        }

        public void reload(QuerySequence query)
        {
            wood.Clear();

            foreach(Tuple<Neuron, int> tuple in query.Activations)
            {
                HashSet<Leaf> bush = new HashSet<Leaf>();
                bush.Add(new Leaf(map[tuple]));

                int index = leafs.IndexOf(map[tuple]) + 1;
                int max = tuple.Item2 + query.Interval * 10 + 30;

                for(int i = index; i < leafs.Count; i++)
                {
                    if (leafs[i].Time > max)
                        break;

                    if (leafs[i].Neuron == tuple.Item1)
                        continue;

                    bush.Add(new Leaf(leafs[i]));
                }

                wood.Add(new Tree(bush, new HashSet<Synapse>(brain.Synapses.Keys)));
            }

            tree = wood[0];
        }

        public void clear()
        {
            leafs.Clear();
            wood.Clear();
            map.Clear();
        }

        public override void show()
        {
            foreach (Tree bush in wood)
                bush.show();

            layout.show();
            base.show();

            foreach (Tree bush in wood)
                bush.hide();

            tree.show();
        }

        public override void hide()
        {
            tree.hide();
            layout.hide();
            base.hide();
        }

        protected override void rescale()
        {
            foreach (Tree tree in wood)
                tree.Scale = scale;

            layout.Scale = scale;
        }

        public override void up()
        {
            if(index + 1 < wood.Count)
            {
                tree.hide();
                tree = wood[++index];
                tree.show();
            }
        }

        public override void down()
        {
            if(index > 0)
            {
                tree.hide();
                tree = wood[--index];
                tree.show();
            }
        }
    }
}