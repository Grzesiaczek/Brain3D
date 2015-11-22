using System;
using System.Collections.Generic;

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
            {
                this.branches.Add(new BalancedBranch(branch, map));
            }

            foreach (BalancedLeaf leaf in this.leafs)
            {
                leaf.Load(this.leafs, this.branches);
            }

            foreach (BalancedBranch branch in this.branches)
            {
                branch.Load(this.branches);
            }
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
            {
                leaf.Refresh(mapLeafs, mapBranches);
            }

            foreach (BalancedBranch branch in branches)
            {
                branch.Refresh(mapLeafs, mapBranches);
            }

            Mutate();
        }

        public void Mutate()
        {
            int count = random.Next(leafs.Count / 2 + 1) + 1;
            cost = 0;

            for (int i = 0; i < count; i++)
            {
                BalancedLeaf leaf = leafs[random.Next(leafs.Count)];
                leaf.MoveY((float)(2.4 * random.NextDouble() - 0.8));
            }

            foreach (BalancedLeaf leaf in leafs)
            {
                cost += leaf.GetCost();
            }
        }

        public void Shift()
        {
            foreach (BalancedLeaf leaf in leafs)
            {
                leaf.Shift();
            }
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
