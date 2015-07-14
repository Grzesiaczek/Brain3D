using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    class BalancedTree
    {
        List<BalancedLeaf> leafs;
        List<BalancedBranch> branches;

        static Random random = new Random();
        float cost;

        public BalancedTree(HashSet<Leaf> leafs, HashSet<Branch> branches)
        {
            Dictionary<Leaf, BalancedLeaf> map = new Dictionary<Leaf, BalancedLeaf>();

            this.leafs = new List<BalancedLeaf>();
            this.branches = new List<BalancedBranch>();

            foreach (Leaf leaf in leafs)
            {
                BalancedLeaf balanced = new BalancedLeaf(leaf);
                this.leafs.Add(balanced);
                map.Add(leaf, balanced);
            }

            foreach (Branch branch in branches)
                this.branches.Add(new BalancedBranch(branch, map));

            foreach (BalancedLeaf leaf in this.leafs)
                leaf.load(this.leafs, this.branches);

            foreach (BalancedBranch branch in this.branches)
                branch.load(this.branches);
        }

        public BalancedTree(BalancedTree tree)
        {
            leafs = new List<BalancedLeaf>(tree.leafs);
            branches = new List<BalancedBranch>(tree.branches);

            Dictionary<BalancedLeaf, BalancedLeaf> mapLeafs = new Dictionary<BalancedLeaf, BalancedLeaf>();
            Dictionary<BalancedBranch, BalancedBranch> mapBranches = new Dictionary<BalancedBranch, BalancedBranch>();

            for (int i = 0; i < leafs.Count; i++)
            {
                BalancedLeaf leaf = new BalancedLeaf(leafs[i]);
                mapLeafs.Add(leafs[i], leaf);
                leafs[i] = leaf;
            }

            for (int i = 0; i < branches.Count; i++)
            {
                BalancedBranch branch = new BalancedBranch(branches[i]);
                mapBranches.Add(branches[i], branch);
                branches[i] = branch;
            }

            foreach (BalancedLeaf leaf in leafs)
                leaf.refresh(mapLeafs, mapBranches);

            foreach (BalancedBranch branch in branches)
                branch.refresh(mapLeafs, mapBranches);

            mutate();
        }

        public void mutate()
        {
            int count = random.Next(leafs.Count / 2 + 1) + 1;
            cost = 0;

            for (int i = 0; i < count; i++)
            {
                BalancedLeaf leaf = leafs[random.Next(leafs.Count)];
                leaf.moveY((float)(2.4 * random.NextDouble() - 0.8));
            }

            foreach (BalancedLeaf leaf in leafs)
                cost += leaf.getCost();
        }

        public void shift()
        {
            foreach (BalancedLeaf leaf in leafs)
                leaf.shift();
        }

        public float Cost
        {
            get
            {
                return cost;
            }
        }
    }
}
