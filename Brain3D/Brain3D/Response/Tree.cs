using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Brain3D
{
    class Tree : DrawableComposite
    {
        HashSet<Leaf> leafs;
        HashSet<Branch> branches;

        public Tree(HashSet<Leaf> leafs, HashSet<Synapse> synapses)
        {
            this.leafs = leafs;
            branches = new HashSet<Branch>();

            foreach (Leaf leaf in leafs)
                foreach (Leaf source in leafs)
                {
                    if (leaf == source)
                        continue;

                    Synapse synapse = synapses.FirstOrDefault(k => k.Pre == source.Neuron && k.Post == leaf.Neuron);

                    if (synapse != null)
                        branches.Add(new Branch(source, leaf));
                }

            drawables.AddRange(branches);
            drawables.AddRange(leafs);

            // ThreadPool.QueueUserWorkItem(balance);
            balance(null);
        }

        void balance(object state)
        {
            List<BalancedTree> wood = new List<BalancedTree>();
            List<BalancedTree> forest = new List<BalancedTree>();

            for (int i = 0; i < 3; i++)
                wood.Add(new BalancedTree(leafs, branches));

            for(int i = 0; i < 200; i++)
            {
                for (int j = 0; j < wood.Count; j++)
                    for (int k = 0; k < 4; k++)
                        forest.Add(new BalancedTree(wood[j]));

                foreach (BalancedTree tree in wood)
                    tree.mutate();

                if (forest[0] == forest[1])
                    throw new Exception();

                forest.AddRange(wood);
                forest.Sort(new Comparer());

                for (int j = 0; j < 3; j++)
                    wood[j] = forest[j];

                forest.Clear();
            }

            wood[0].shift();

            foreach (Branch branch in branches)
                branch.move();
        }

        public override float Scale
        {
            set
            {
                foreach (Leaf leaf in leafs)
                    leaf.Scale = value;

                foreach (Branch branch in branches)
                    branch.move();
            }
        }
    }
}
