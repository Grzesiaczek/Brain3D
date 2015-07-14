using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Brain3D
{
    class BalancedLeaf
    {
        List<BalancedLeaf> leafs;
        List<BalancedBranch> branches;

        Leaf leaf;
        Vector2 position;

        public BalancedLeaf(Leaf leaf)
        {
            this.leaf = leaf;
            position = new Vector2(leaf.Position.X, leaf.Position.Y);
        }

        public BalancedLeaf(BalancedLeaf leaf)
        {
            this.leaf = leaf.leaf;
            position = leaf.position;

            leafs = new List<BalancedLeaf>(leaf.leafs);
            branches = new List<BalancedBranch>(leaf.branches);
        }

        public void load(List<BalancedLeaf> leafs, List<BalancedBranch> branches)
        {
            this.leafs = new List<BalancedLeaf>();
            this.branches = new List<BalancedBranch>();

            foreach (BalancedLeaf leaf in leafs)
                if (leaf != this && Math.Abs(leaf.position.X - position.X) < 0.1f)
                    this.leafs.Add(leaf);

            foreach (BalancedBranch branch in branches)
                if (position.X > branch.Source.position.X && position.X < branch.Target.position.X)
                    this.branches.Add(branch);
        }

        public void refresh(Dictionary<BalancedLeaf, BalancedLeaf> mapLeafs, Dictionary<BalancedBranch, BalancedBranch> mapBranches)
        {
            for (int i = 0; i < leafs.Count; i++)
                leafs[i] = mapLeafs[leafs[i]];

            for (int i = 0; i < branches.Count; i++)
                branches[i] = mapBranches[branches[i]];
        }

        public void moveY(float y)
        {
            position = new Vector2(position.X, y);
        }

        public void shift()
        {
            leaf.Position = new Vector3(position, 0);
        }

        public float getCost()
        {
            float cost = (float)Math.Abs(position.Y - 0.8f) / 2;

            foreach (BalancedLeaf leaf in leafs)
                cost += (float)Math.Abs(leaf.Position.Y - position.Y);

            foreach (BalancedBranch branch in branches)
                cost += branch.getCost(this);

            return cost;
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }
    }
}
