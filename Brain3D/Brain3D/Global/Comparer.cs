using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    class Comparer : IComparer<AnimatedElement>, IComparer<SimulatedNeuron>, IComparer<BalancedTree>
    {
        public int Compare(AnimatedElement x, AnimatedElement y)
        {
            if (x.Depth < y.Depth)
                return 1;
            if (x.Depth > y.Depth)
                return -1;

            return 0;
        }

        public int Compare(SimulatedNeuron x, SimulatedNeuron y)
        {
            if (x.Ratio < y.Ratio)
                return 1;
            if (x.Ratio > y.Ratio)
                return -1;

            return 0;
        }

        public int Compare(BalancedTree x, BalancedTree y)
        {
            if (x.Cost > y.Cost)
                return 1;
            if (x.Cost < y.Cost)
                return -1;

            return 0;
        }
    }
}
