using System.Collections.Generic;

namespace Brain3D
{
    class BalancedBranch
    {
        BalancedLeaf source;
        BalancedLeaf target;

        List<BalancedBranch> branches;

        public BalancedBranch(Branch branch, Dictionary<Leaf, BalancedLeaf> map)
        {
            source = map[branch.Source];
            target = map[branch.Target];
        }

        public BalancedBranch(BalancedBranch branch)
        {
            source = branch.source;
            target = branch.target;
            branches = new List<BalancedBranch>(branch.branches);
        }

        public void Load(List<BalancedBranch> branches)
        {
            this.branches = new List<BalancedBranch>();

            foreach(BalancedBranch branch in branches)
            {
                if (branch != this && branch.Target.Position.X >= source.Position.X && branch.Source.Position.X <= target.Position.X)
                {
                    this.branches.Add(branch);
                }
            }
        }

        public void Refresh(Dictionary<BalancedLeaf, BalancedLeaf> mapLeafs, Dictionary<BalancedBranch, BalancedBranch> mapBranches)
        {
            source = mapLeafs[source];
            target = mapLeafs[target];

            for (int i = 0; i < branches.Count; i++)
            {
                branches[i] = mapBranches[branches[i]];
            }
        }

        public float GetCost(BalancedLeaf leaf)
        {
            float plus = source.Position.X * target.Position.Y + target.Position.X * leaf.Position.Y + leaf.Position.X * source.Position.Y;
            float minus = source.Position.Y * target.Position.X + target.Position.Y * leaf.Position.X + leaf.Position.Y * source.Position.X;
            float distance = plus - minus;

            if (distance < 0.2f)
            {
                distance = 0.2f;
            }

            return 2 / distance;
        }

        public BalancedLeaf Source
        {
            get
            {
                return source;
            }
        }

        public BalancedLeaf Target
        {
            get
            {
                return target;
            }
        }
    }
}
