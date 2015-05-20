using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    abstract class BrainElement
    {
        protected static double tmax;
        protected static double omega;

        protected static int length;
        protected static int size;

        public static void initialize(int value)
        {
            length = value;
            size = 10 * length + 1;

            tmax = 200;
            omega = 1000;
        }
    }
}
