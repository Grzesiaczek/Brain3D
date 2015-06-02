using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class LabelAxis : Text3D
    {
        int number;

        public LabelAxis(int number)
        {
            position = new Vector3(number * 0.1f, -1.2f, 0);
            this.number = number;
            ratio = 0.008f;

            text = font.Fill(number.ToString());
            shift = new Vector3(-text.Width * ratio / 2, 0, 0);
            color = Color.DarkBlue;        
        }

        public override void initialize()
        {
            int vertex = text.Vertices.Length;
            int index = text.Indices.Length;

            vertices = new VertexPositionColor[vertex];
            framework = new Vector3[vertex];
            indices = new int[index];

            for (int i = 0; i < vertex; i++)
            {
                framework[i] = text.Vertices[i].Position * ratio + shift;
                vertices[i] = new VertexPositionColor(framework[i] + position, color);
            }

            for (int i = 0; i < index; i++)
                indices[i] = text.Indices[i];

            offset = buffer.add(vertices, indices);
            initialized = true;
        }

        public void move(float x)
        {
            position = new Vector3(x, -1.2f, 0);
            move();
        }
    }
}
