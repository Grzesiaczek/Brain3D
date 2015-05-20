using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class Chart : DrawableElement
    {
        static Vector2[] angles;
        NeuronData[] activity;

        int count;

        float value;
        float previous;
        float next;

        public Chart(Neuron neuron, Color color)
        {
            activity = neuron.Activity;
            count = activity.Length;
            this.color = color;            

            vertices = new VertexPositionColor[2 * count];
            indices = new int[6 * count - 6];
        }

        public static void initializeAngles()
        {
            double angle = Math.PI / 2 - 1.57;
            angles = new Vector2[314];

            for (int i = 0; i < 314; i++, angle += 0.01)
                angles[i] = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public override void initialize()
        {
            for (int i = 2; i < 1000; i++)
            {
                NeuronData data = activity[i];

                if (data.Active)
                    next = 1 - (float)data.Refraction / 15;
                else
                    next = (float)data.Value;

                double a1 = Math.Atan((value - previous) * 100);
                double a2 = Math.Atan((next - value) * 100);
                double an = (a1 + a2) / 2;
                double ad = an - a1;

                double test = Math.Atan(-3);

                if (ad < 0)
                    ad = -ad;

                int index = (int)(157 + an * 100);
                float factor = (float)(0.01 / Math.Cos(ad));

                Vector2 angle = angles[index] * factor;

                vertices[2 * i] = new VertexPositionColor(new Vector3(0.01f * i - angle.X, value - angle.Y, 0), color);
                vertices[2 * i + 1] = new VertexPositionColor(new Vector3(0.01f * i + angle.X, value + angle.Y, 0), color);

                previous = value;
                value = next;
            }

            vertices[0] = new VertexPositionColor(new Vector3(0, -0.01f, 0), color);
            vertices[1] = new VertexPositionColor(new Vector3(0, 0.01f, 0), color);

            for (int i = 0, j = 0; i < 999; i++)
            {
                int vertex = 2 * i;

                indices[j++] = vertex + 0;
                indices[j++] = vertex + 3;
                indices[j++] = vertex + 2;
                indices[j++] = vertex + 0;
                indices[j++] = vertex + 1;
                indices[j++] = vertex + 3;
            }

            offset = buffer.add(vertices, indices);
        }
    }
}
