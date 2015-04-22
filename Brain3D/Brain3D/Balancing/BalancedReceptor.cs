using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class BalancedReceptor : BalancedElement
    {
        AnimatedReceptor receptor;

        static float k = 8;
        static float k2 = k * k;

        public BalancedReceptor(AnimatedReceptor receptor)
        {
            this.receptor = receptor;
            shift = Vector3.Zero;
            position = receptor.Position;
        }

        public float update()
        {
            if (receptor.Neuron.Position.Length() < 5)
                return 0;

            Spherical spherical = new Spherical(receptor.Neuron.Position);
            spherical.Radius = Constant.Size;
            position = spherical.getVector();
            return 0;
        }
    }
}
