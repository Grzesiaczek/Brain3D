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
        float[] activity;

        public Chart(Neuron neuron, Color color)
        {
            activity = new float[1000];
            this.color = color;

            for (int i = 0; i < 1000; i++)
                activity[i] = (float)neuron.Activity[i].Value;

            vertices = new VertexPositionColor[2000];
            indices = new int[5994];
            //refresh();
        }

        public override void initialize()
        {
            base.initialize();
        }

        public override void refresh()
        {
            for(int i = 0; i < 1000; i++)
            {
                vertices[2 * i] = new VertexPositionColor(new Vector3(0.01f * i, activity[i] - 0.01f, 0), color);
                vertices[2 * i + 1] = new VertexPositionColor(new Vector3(0.01f * i, activity[i] + 0.01f, 0), color);
            }

            for(int i = 0; i < 999; i++)
            {
                int index = 6 * i;
                int vertex = 2 * i;

                indices[index + 0] = vertex + 0;
                indices[index + 1] = vertex + 3;
                indices[index + 2] = vertex + 2;
                indices[index + 3] = vertex + 0;
                indices[index + 4] = vertex + 1;
                indices[index + 5] = vertex + 3;
            }

            buffer.add(vertices, indices);
        }
    }
}
