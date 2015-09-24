using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    abstract class SimulatedElement
    {
        protected static double tmax;
        protected static double omega;

        protected static int length;
        protected static int size;

        public static void initialize(double omega, double tmax)
        {
            SimulatedElement.tmax = tmax * 10;
            SimulatedElement.omega = omega * 10;
        }

        public static void Initialize(int value)
        {
            length = value;
            size = 10 * length + 1;

            tmax = 200;
            omega = 1000;
        }
    }
}
