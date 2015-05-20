using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class Disk : DrawableElement
    {
        Circle framework;

        float radius;
        int points;

        public Disk(Vector3 position, Circle framework, Color color, float radius)
        {
            this.position = position;
            this.framework = framework;
            this.color = color;
            this.radius = radius;

            points = framework.Points;

            vertices = new VertexPositionColor[points + 1];
            indices = new int[3 * points];
        }

        public override void initialize()
        {
            vertices[points] = new VertexPositionColor(position, color);

            for (int i = 0; i < points; i++)
                vertices[i] = new VertexPositionColor(framework.Data[i] * radius + position, color);

            indices[0] = points;
            indices[1] = points - 1;
            indices[2] = 0;

            for (int i = 1; i < points; i++)
            {
                int index = 3 * i;
                indices[index + 0] = points;
                indices[index + 1] = i - 1;
                indices[index + 2] = i;
            }

            offset = buffer.add(vertices, indices);
        }

        public override void move()
        {
            buffer.Vertices[offset + points].Position = position;

            for (int i = 0, j = offset; i < points; i++)
                buffer.Vertices[j++].Position = framework.Data[i] * radius + position;
        }

        public override void repaint()
        {
            buffer.Vertices[offset + points].Color = color;

            for (int i = 0, j = offset; i < points; i++)
                buffer.Vertices[j++].Color = color;
        }

        public float Radius
        {
            set
            {
                radius = value;
                move();
            }
        }
    }
}
