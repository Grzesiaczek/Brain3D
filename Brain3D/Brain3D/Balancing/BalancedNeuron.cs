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
        AnimatedNeuron neuron;

        List<AnimatedVector> input;
        List<AnimatedVector> output;
        List<AnimatedVector> vectors;

        static float k1 = 8;
        static float k2 = k1 * k1;
        static float k3 = k2 * k1;
        static float k0 = 800;

        float factor;

        Vector3 border = Vector3.Zero;
        Vector3 others = Vector3.Zero;
        Vector3 synapses = Vector3.Zero;
        Vector3 last;

        Vector3 shift;
        Vector3 position;

        public BalancedNeuron(AnimatedNeuron neuron)
        {
            this.neuron = neuron;
            shift = Vector3.Zero;
            position = neuron.Position;

            input = new List<AnimatedVector>();
            output = new List<AnimatedVector>();
            vectors = new List<AnimatedVector>();

            foreach (AnimatedSynapse synapse in neuron.Input)
                input.Add(synapse.Vector);

            foreach (AnimatedSynapse synapse in neuron.Output)
                output.Add(synapse.Vector);

            factor = input.Count + output.Count;
            factor = 1 + (factor - 1) * (factor - 1);
        }

        public void repulse(Vector3 pos, bool connected)
        {
            Vector3 delta = position - pos;
            float distance = delta.LengthSquared() * delta.Length();

            if (distance < 1)
                distance = 1;

            others += (k2 * delta / distance);

            if(connected)
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
                shift.Z += repulse(Constant.Box.Z, position.Z);
            }
            else
            {
                float distance = Constant.Radius - position.Length();
                shift -= k2 * position / (distance * distance);
            }

            border = shift;
        }

        float repulse(float box, float position)
        {
            float plus = box - position;
            float minus = box + position;
            return k0 / (minus * minus) - k0 / (plus * plus);
        }

        public void rotate()
        {
            /*int count = input.Count + output.Count;

            foreach (AnimatedVector v1 in input)
            {
                float rotation = 0;

                foreach (AnimatedVector v2 in input)
                {
                    if (v1 == v2)
                        continue;

                    rotation += diff(v1.Angle, v2.Angle);
                }

                foreach (AnimatedVector v2 in output)
                {
                    float angle = v2.Angle;

                    if (angle < Math.PI)
                        angle += (float)Math.PI;
                    else
                        angle -= (float)Math.PI;

                    rotation += diff(v1.Angle, angle);
                }

                v1.Rotation += rotation / count;
            }

            foreach (AnimatedVector v1 in output)
            {
                float rotation = 0;

                foreach (AnimatedVector v2 in output)
                {
                    if (v1 == v2)
                        continue;

                    rotation += diff(v1.Angle, v2.Angle);
                }

                foreach (AnimatedVector v2 in input)
                {
                    float angle = v2.Angle;
                    test.Add(rotation);

                    if (angle < Math.PI)
                        angle += (float)Math.PI;
                    else
                        angle -= (float)Math.PI;

                    rotation += diff(v1.Angle, angle);
                }

                v1.Rotation += rotation / count;
            }*/
        }

        public float update(float factor)
        {
            float result = 0;
            String name = neuron.Name;
            last = shift;

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
            synapses += vector;
        }

        public void zero()
        {
            border = Vector3.Zero;
            others = Vector3.Zero;
            synapses = Vector3.Zero;
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

        public float Factor
        {
            get
            {
                return factor;
            }
        }

        public float Radius
        {
            get
            {
                return neuron.Radius;
            }
        }
    }
}
