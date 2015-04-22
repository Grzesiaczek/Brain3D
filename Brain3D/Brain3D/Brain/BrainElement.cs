using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    abstract class BrainElement
    {
        public String Name;
        public bool Active;

        protected static double tmax;
        protected static double omega;

        protected static int length;
        protected static int size;

        public static void initialize(int value)
        {
            length = value;
            size = 10 * length;

            tmax = 250;
            omega = 1000;
        }
    }
}
