using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Brain3D
{
    class BalancedNeuron
    {
        static Dictionary<AnimatedVector, BalancedVector> map;
        AnimatedNeuron neuron;

        Vector3 position;
        Vector3 shift;
        
        static float k1 = 20;
        static float k2 = k1 * k1;
        static float k3 = k2 * k1;
        static float k0 = 250;

        object locker = new object();

        int count;

        float origin;
        float target;

        public BalancedNeuron(AnimatedNeuron neuron)
        {
            this.neuron = neuron;
            shift = Vector3.Zero;
            position = neuron.Position;

            count = neuron.Input.Count + neuron.Output.Count;
            count *= count;
        }

        public void Repulse(Vector3 pos)
        {
            Vector3 delta = position - pos;
            float distance = delta.LengthSquared() * delta.Length();

            if (distance < 1)
            {
                distance = 1;
            }

            lock (locker)
            {
                shift += k2 * delta / distance;
            }
        }

        public void Repulse()
        {
            Vector3 shift = Vector3.Zero;

            if (Constant.Space == SpaceMode.Box)
            {
                shift.X += Repulse(Constant.Box.X, position.X);
                shift.Y += Repulse(Constant.Box.Y, position.Y);
                shift.Z += Repulse(Constant.Box.Z, position.Z) * 4;
            }
            else
            {
                float distance = Constant.Radius - position.Length();
                shift -= k2 * position / (distance * distance);
            }

            lock(locker)
            {
                this.shift += shift;
            }
        }

        float Repulse(float box, float position)
        {
            float plus = box - position;
            float minus = box + position;

            if (minus < 1)
            {
                minus = 400 - minus * 200;
            }
            else
            {
                minus = k0 / minus;
            }

            if (plus < 1)
            {
                plus = 400 - plus * 200;
            }
            else
            {
                plus = k0 / plus;
            }

            return minus - plus;
         }

        public void Rotate()
        {
            float factor = 4.2f * count;
            Vector3 rotation = Vector3.Zero;
            
            foreach (AnimatedVector s1 in neuron.Input)
            {
                Vector3 shift = Vector3.Zero;

                foreach (AnimatedVector s2 in neuron.Input)
                {
                    if (s1 == s2)
                        continue;

                    shift += Rotate(-s1.Direction, -s2.Direction) * 5;
                }

                foreach (AnimatedVector s2 in neuron.Output)
                {
                    shift += Rotate(-s1.Direction, s2.Direction) * 2;
                }

                shift *= s1.Length / factor;
                map[s1].Pre.Move(-shift);
                rotation += shift;
            }

            foreach (AnimatedVector s1 in neuron.Output)
            {
                Vector3 shift = Vector3.Zero;

                foreach (AnimatedVector s2 in neuron.Output)
                {
                    if (s1 == s2)
                        continue;

                    shift += Rotate(s1.Direction, s2.Direction);
                }

                foreach (AnimatedVector s2 in neuron.Input)
                {
                    shift += Rotate(s1.Direction, -s2.Direction) * 2;
                }

                shift *= s1.Length / factor;
                map[s1].Post.Move(-shift);
                rotation += shift;
            }

            lock (locker)
            {
                shift += rotation;
            }
        }

        Vector3 Rotate(Vector3 source, Vector3 target)
        {
            Vector3 result = Vector3.Zero;
            Vector3 vector = target - source;

            float left = vector.X * source.X + vector.Y * source.Y + vector.Z * source.Z;
            float right = source.X * source.X + source.Y * source.Y + source.Z * source.Z;
            float t = - right / left;

            result.X = vector.X * t + source.X;
            result.Y = vector.Y * t + source.Y;
            result.Z = vector.Z * t + source.Z;

            result *= (float)Math.Pow(2 - vector.Length(), 2) / result.Length();

            if (float.IsNaN(result.Length()))
            {
                return Vector3.Zero;
            }

            return result;
        }

        public float Update(float factor)
        {
            float result = 0;
            string name = neuron.Word;

            if (float.IsNaN(shift.Length()))
            {
                shift = new Vector3(0.1f, 0.1f, 0.1f);
            }

            if (shift.Length() > 1)
            {
                shift.Normalize();
                result = 1;
            }
            else
            {
                result = shift.Length();
            }

            position += shift * factor;
            neuron.Position = position;
            shift = Vector3.Zero;

            return result;
        }

        public void Move(Vector3 vector)
        {
            lock (locker)
            {
                shift += vector;
            }
        }

        public void Rescale(float scale)
        {
            neuron.Radius = origin + (target - origin) * scale;
        }

        public void Rescale()
        {
            neuron.Radius = target;
        }

        public void Calculate(float max)
        {
            origin = neuron.Radius;
            target = 1 + 0.4f * neuron.Neuron.Count / max;
        }

        public static Dictionary<AnimatedVector, BalancedVector> Map
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

        public AnimatedNeuron Neuron
        {
            get
            {
                return neuron;
            }
        }

        public Vector3 Position
        {
            get
            {
                return position;
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
