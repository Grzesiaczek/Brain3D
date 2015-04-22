using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    class Comparer : IComparer<AnimatedElement>, IComparer<String>
    {
        public int Compare(AnimatedElement x, AnimatedElement y)
        {
            if (x.Depth < y.Depth)
                return 1;
            if (x.Depth > y.Depth)
                return -1;

            return 0;
        }

        public int Compare(string x, string y)
        {
            //throw new NotImplementedException();
            return 0;
        }
    }
}
