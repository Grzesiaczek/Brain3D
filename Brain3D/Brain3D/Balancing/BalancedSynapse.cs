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

        static float k = 50;
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

            force = 100 * (1.56f - (float)Math.Atan(distance - 2));

            shift = new Vector3(force * (pos.X - x) / distance, force * (pos.Y - y) / distance, 0) * 0.8f;
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
