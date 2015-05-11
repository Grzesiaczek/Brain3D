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
        Circle innerCircle;
        Circle outerCircle;

        int points;
        int points2;

        public Ring(Circle inner, Circle outer, Color color)
        {
            innerCircle = inner;
            outerCircle = outer;

            this.color = color;
            points = inner.Points;
            points2 = 2 * points;

            vertices = new VertexPositionColor[2 * points];
            indices = new int[6 * points];
        }

        public override void initialize()
        {
            for (int i = 0, j = 0; i < points; i++)
            {
                vertices[j++] = new VertexPositionColor(innerCircle.Data[i], color);
                vertices[j++] = new VertexPositionColor(outerCircle.Data[i], color);
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

        public override void refresh()
        {
            for (int i = 0, j = offset; i < points; i++)
            {
                buffer.Vertices[j++].Position = innerCircle.Data[i];
                buffer.Vertices[j++].Position = outerCircle.Data[i];
            }
        }
    }
}
