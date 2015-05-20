using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class BalancedNeuron
    {
        static Dictionary<AnimatedSynapse, BalancedSynapse> map;
        AnimatedNeuron neuron;

        List<AnimatedVector> input;
        List<AnimatedVector> output;

        static float k1 = 20;
        static float k2 = k1 * k1;
        static float k3 = k2 * k1;
        static float k0 = 1000;

        int count;

        Vector3 shift;
        Vector3 position;

        public BalancedNeuron(AnimatedNeuron neuron)
        {
            this.neuron = neuron;
            shift = Vector3.Zero;
            position = neuron.Position;

            input = new List<AnimatedVector>();
            output = new List<AnimatedVector>();

            foreach (AnimatedSynapse synapse in neuron.Input)
                input.Add(synapse.Vector);

            foreach (AnimatedSynapse synapse in neuron.Output)
                output.Add(synapse.Vector);

            count = input.Count + output.Count;
            count *= count;
        }

        public void repulse(Vector3 pos, bool connected)
        {
            Vector3 delta = position - pos;
            float distance = delta.LengthSquared() * delta.Length();

            if (distance < 1)
                distance = 1;

            if (connected)
                shift += k3 * delta / (distance * delta.Length());
            else
                shift += k2 * delta / distance;
        }

        public void repulse()
        {
            if (Constant.Space == SpaceMode.Box)
            {
                shift.X += repulse(Constant.Box.X, position.X);
                shift.Y += repulse(Constant.Box.Y, position.Y);
                shift.Z += repulse(Constant.Box.Z, position.Z) / 2;
            }
            else
            {
                float distance = Constant.Radius - position.Length();
                shift -= k2 * position / (distance * distance);
            }
        }

        float repulse(float box, float position)
        {
            float plus = box - position;
            float minus = box + position;
            return k0 / (minus * minus) - k0 / (plus * plus);
        }

        public void rotate()
        {
            int count = input.Count + output.Count;
            
            foreach (AnimatedSynapse s1 in neuron.Input)
            {
                Vector3 shift = Vector3.Zero;

                foreach (AnimatedSynapse s2 in neuron.Input)
                {
                    if (s1 == s2)
                        continue;

                    shift += rotate(-s1.Vector.Direction, -s2.Vector.Direction);
                }

                foreach (AnimatedSynapse s2 in neuron.Output)
                    shift += rotate(-s1.Vector.Direction, s2.Vector.Direction);

                shift *= -10 / count;
                map[s1].Pre.move(shift);
                this.shift -= shift;
            }

            foreach (AnimatedSynapse s1 in neuron.Output)
            {
                Vector3 shift = Vector3.Zero;

                foreach (AnimatedSynapse s2 in neuron.Output)
                {
                    if (s1 == s2)
                        continue;

                    shift += rotate(s1.Vector.Direction, s2.Vector.Direction);
                }

                foreach (AnimatedSynapse s2 in neuron.Input)
                    shift += rotate(s1.Vector.Direction, -s2.Vector.Direction);

                shift *= -10 / count;
                map[s1].Post.move(shift);
                this.shift -= shift;
            }
        }

        Vector3 rotate(Vector3 source, Vector3 target)
        {
            Vector3 result = Vector3.Zero;
            Vector3 vector = target - source;

            float left = vector.X * source.X + vector.Y * source.Y + vector.Z * source.Z;
            float right = source.X * source.X + source.Y * source.Y + source.Z * source.Z;
            float t = - right / left;

            result.X = vector.X * t + source.X;
            result.Y = vector.Y * t + source.Y;
            result.Z = vector.Z * t + source.Z;

            Vector3 test = result * (float)Math.Pow(2 - vector.Length(), 2) / result.Length();

            return result * (float)Math.Pow(2 - vector.Length(), 2) / result.Length();
        }


        public float update(float factor)
        {
            float result = 0;
            String name = neuron.Name;

            if (shift.Length() > 1)
            {
                shift.Normalize();
                result = 1;
            }
            else
                result = shift.Length();

            position += shift * factor;
            neuron.Position = position;
            shift = Vector3.Zero;

            return result;
        }

        public void move(Vector3 vector)
        {
            shift += vector;
        }

        public static Dictionary<AnimatedSynapse, BalancedSynapse> Map
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

        public static float K
        {
            set
            {
                k1 = value;
                k2 = k1 * k1;
                k3 = k2 * k1;
            }
        }

        public Vector3 Position
        {
            get
            {
                return position;
            }
        }

        public String Name
        {
            get
            {
                return neuron.Name;
            }
        }

        public int Count
        {
            get
            {
                return count;
            }
        }
    }
}
