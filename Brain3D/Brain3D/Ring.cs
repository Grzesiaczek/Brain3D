using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class Ring : DrawableElement
    {
        Circle framework;

        int points;
        int points2;

        float r1;
        float r2;

        public Ring(Vector3 position, Circle framework, Color color)
        {
            this.position = position;
            this.framework = framework;

            this.color = color;
            points = framework.Points;
            points2 = 2 * points;

            vertices = new VertexPositionColor[2 * points];
            indices = new int[6 * points];
        }

        public override void initialize()
        {
            for (int i = 0, j = 0; i < points; i++)
            {
                vertices[j++] = new VertexPositionColor(framework.Data[i] * r1 + position, color);
                vertices[j++] = new VertexPositionColor(framework.Data[i] * r2 + position, color);
            }

            int index = 6;
            int vertex = 2 * points - 2;

            indices[0] = 0;
            indices[1] = vertex;
            indices[2] = vertex + 1;
            indices[3] = 0;
            indices[4] = vertex + 1;
            indices[5] = 1;

            for (int i = 1; i < points; i++)
            {
                vertex = 2 * i;

                indices[index++] = vertex;
                indices[index++] = vertex - 2;
                indices[index++] = vertex - 1;
                indices[index++] = vertex;
                indices[index++] = vertex - 1;
                indices[index++] = vertex + 1;
            }

            offset = buffer.add(vertices, indices);
        }

        public override void move()
        {
            for (int i = 0, j = offset; i < points; i++)
            {
                buffer.Vertices[j++].Position = framework.Data[i] * r1 + position;
                buffer.Vertices[j++].Position = framework.Data[i] * r2 + position;
            }
        }

        public override void repaint()
        {
            for (int i = 0, j = offset; i < points; i++)
            {
                buffer.Vertices[j++].Color = color;
                buffer.Vertices[j++].Color = color;
            }
        }

        public float R1
        {
            set
            {
                r1 = value;
            }
        }

        public float R2
        {
            set
            {
                r2 = value;
            }
        }
    }
}
