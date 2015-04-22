using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class BalancedNeuron : BalancedElement
    {
        AnimatedNeuron neuron;

        List<AnimatedVector> input;
        List<AnimatedVector> output;
        List<AnimatedVector> vectors;

        static float k = 4;
        static float k2 = k * k;

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
        }

        public void repulse(Vector3 pos)
        {
            Vector3 delta = position - pos;
            float distance = delta.LengthSquared();

            if (distance < 1)
                distance = 1;

            shift += k2 * delta / distance;
        }

        public void repulse()
        {
            float distance = Constant.Size - position.Length();
            shift -= k2 * position / (distance * distance);
        }

        public void repulse(AnimatedNeuron neuron)
        {
            Vector3 delta = neuron.Screen - this.neuron.Screen;
            delta = Vector3.Transform(delta, camera.Rotation) / 20;

            float distance = delta.LengthSquared();

            if (distance < 16)
                distance = 16;

            shift += k2 * delta / distance;
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

        float diff(float a1, float a2)
        {
            float angle = (float)(Math.PI + a1 - a2);
            float factor = 0.2f;
            angle = a2 - a1;
            float result = 0;

            if (angle < 0)
                angle += (float)(2 * Math.PI);

            angle -= (float)Math.PI;

            if(angle < 0)
            {
                if (angle < -3)
                    angle = -3;

                result = (float)(factor / Math.Pow(Math.PI - angle, 2));
            }
            else
            {
                if (angle > 3)
                    angle = 3;

                result = (float)(-factor / Math.Pow(Math.PI + angle, 2));
            }

            return result;
        }

        public float update(float factor)
        {
            float result = shift.Length();
            position += shift * factor;

            neuron.Position = position;
            shift = Vector3.Zero;

            return result;
        }

        public AnimatedNeuron Neuron
        {
            get
            {
                return neuron;
            }
        }
    }
}
