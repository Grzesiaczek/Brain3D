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
        AnimatedSynapse synapse;

        BalancedNeuron pre;
        BalancedNeuron post;

        static float k = 10;
        float factor;

        public BalancedSynapse(AnimatedSynapse synapse, Dictionary<AnimatedNeuron, BalancedNeuron> map)
        {
            this.synapse = synapse;
            pre = map[synapse.Pre];
            post = map[synapse.Post];

            factor = k / (1 + synapse.Weight);
        }

        public void attract()
        {
            Vector3 delta = post.Position - pre.Position;
            Vector3 shift = delta / factor;

            shift.X *= Math.Abs(delta.X);
            shift.Y *= Math.Abs(delta.Y);
            shift.Z *= Math.Abs(delta.Z);

            pre.move(shift / pre.Factor);
            post.move(-shift / post.Factor);

            pre.repulse(post.Position, true);
            post.repulse(pre.Position, true);
        }

        public void repulse(BalancedNeuron neuron)
        {
            if (neuron == pre || neuron == post)
                return;

            Vector3 vec = synapse.Post.Position - synapse.Pre.Position;
            Vector3 pos = neuron.Position - synapse.Pre.Position;
            Vector3 shift = Vector3.Zero;
            vec.Z = 0;

            String start = ((AnimatedNeuron)synapse.Pre).Name;
            String end = ((AnimatedNeuron)synapse.Post).Name;
            String word = neuron.Name;

            float a = vec.Y;
            float b = vec.X;
            float c = b * pos.X + a * pos.Y;
            float x, y;
            float eq; // punkt równowagi

            if(a == 0)
            {
                x = c / b;
                y = 0;
                eq = x / b;
            }
            else if (b == 0)
            {
                x = 0;
                y = c / a;
                eq = y / a;
            }
            else
            {
                x = c * b / (vec.LengthSquared());
                eq = x / b;
                y = a * eq;
            }

            if (eq < 0 || eq > 1)
                return;

            float force = 0;
            float distance = Math.Abs(b * pos.Y - a * pos.X) / vec.Length();

            if (distance == 0)
                return;

            force = 20 * (1.56f - (float)Math.Atan(distance - 2));

            shift = new Vector3(force * (pos.X - x) / distance, force * (pos.Y - y) / distance, 0);
            neuron.move(shift);
            
            eq = - eq;
            post.move(shift * eq);

            eq = -1 - eq;
            pre.move(shift * eq);
        }

        public void rotate()
        {
            /*if (pre is BalancedReceptor)
                return;
            
            if (Math.Abs(synapse.Vector.Rotation) < 0.001)
            {
                synapse.Vector.Rotation = 0;
                return;
            }

            float shift = synapse.Vector.Rotation * synapse.Vector.Length / 50;
            float x = (float)(shift * Math.Sin(synapse.Vector.Angle));
            float y = (float)(shift * Math.Cos(synapse.Vector.Angle));

            post.move(x, y);
            pre.move(-x, -y);*/
        }
    }
}
