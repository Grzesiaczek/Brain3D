using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class BalancedVector
    {
        static Dictionary<AnimatedNeuron, BalancedNeuron> map;
        AnimatedVector synapse;

        BalancedNeuron source;
        BalancedNeuron target;

        static float k = 120;
        float factor;

        public BalancedVector(AnimatedVector synapse)
        {
            this.synapse = synapse;
            source = map[synapse.Pre];
            target = map[synapse.Post];

            factor = (1f / source.Count + 1f / target.Count);
            factor = k / (factor * (1 + synapse.Weight));
        }

        public void attract()
        {
            Vector3 delta = target.Position - source.Position;
            Vector3 shift = delta * delta * delta / factor;
            /*
            shift.X *= Math.Abs(delta.X);
            shift.Y *= Math.Abs(delta.Y);
            shift.Z *= Math.Abs(delta.Z);*/
            
            source.move(shift);
            target.move(-shift);
        }

        public void repulse(BalancedNeuron neuron)
        {
            if (neuron == source || neuron == target)
                return;

            Vector3 shift = Vector3.Zero;
            Tuple<Vector2, float, float> tuple = Constant.getDistance(synapse.Pre.Position, synapse.Post.Position, neuron.Position);

            float distance = tuple.Item2;
            float eq = tuple.Item3;

            float factor = 100;
            float force = 0;

            if(eq < 0.25)
            {
                if (eq < -0.25)
                    return;

                factor *= (eq + 0.25f) * 2;
            }

            if(eq > 0.75)
            {
                if (eq > 1.25)
                    return;

                factor *= (1.25f - eq) * 2;
            }

            if (distance == 0)
                return;

            force = factor * (1.56f - (float)Math.Atan(distance - 2));

            shift = new Vector3(force * tuple.Item1.X / distance, force * tuple.Item1.Y / distance, 0);
            neuron.move(shift);
            
            eq = - eq;
            target.move(shift * eq);

            eq = -1 - eq;
            source.move(shift * eq);
        }

        public static Dictionary<AnimatedNeuron, BalancedNeuron> Map
        {
            get
            {
                return map;
            }
            set
            {
                map = value;
            }
        }

        public BalancedNeuron Pre
        {
            get
            {
                return source;
            }
        }

        public BalancedNeuron Post
        {
            get
            {
                return target;
            }
        }
    }
}
