using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class BalancedSynapse
    {
        static Dictionary<AnimatedNeuron, BalancedNeuron> map;
        AnimatedSynapse synapse;

        BalancedNeuron pre;
        BalancedNeuron post;

        static float k = 120;
        float factor;

        public BalancedSynapse(AnimatedSynapse synapse)
        {
            this.synapse = synapse;
            pre = map[synapse.Pre];
            post = map[synapse.Post];

            factor = (1f / pre.Count + 1f / post.Count);
            factor = k / (factor * (1 + synapse.Weight));
        }

        public void attract()
        {
            Vector3 delta = post.Position - pre.Position;
            Vector3 shift = delta * delta * delta / factor;
            /*
            shift.X *= Math.Abs(delta.X);
            shift.Y *= Math.Abs(delta.Y);
            shift.Z *= Math.Abs(delta.Z);*/
            
            pre.move(shift);
            post.move(-shift);
        }

        public void repulse(BalancedNeuron neuron)
        {
            if (neuron == pre || neuron == post)
                return;

            Vector3 shift = Vector3.Zero;
            Tuple<Vector2, float, float> tuple = Constant.getDistance(synapse.Pre.Position, synapse.Post.Position, neuron.Position);

            float distance = tuple.Item2;
            float eq = tuple.Item3;

            float factor = 50;
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
            post.move(shift * eq);

            eq = -1 - eq;
            pre.move(shift * eq);
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
                return pre;
            }
        }

        public BalancedNeuron Post
        {
            get
            {
                return post;
            }
        }
    }
}
