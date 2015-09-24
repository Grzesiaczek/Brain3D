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
        Circle pattern;

        float radius;
        int points;

        public Disk(Vector3 position, Circle pattern, Color color, float radius)
        {
            this.position = position;
            this.pattern = pattern;
            this.color = color;
            this.radius = radius;

            points = pattern.Points;
            //scale = 1;

            framework = new Vector3[points];
            vertices = new VertexPositionColor[points + 1];
            indices = new int[3 * points];
        }

        public override void Initialize()
        {
            vertices[points] = new VertexPositionColor(position, color);

            for (int i = 0; i < points; i++)
            {
                framework[i] = pattern.Data[i] * radius * scale;
                vertices[i] = new VertexPositionColor(framework[i] + position, color);
            }

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

            offset = buffer.Add(vertices, indices);
            initialized = true;
        }

        public override void Move()
        {
            if (!initialized)
                return;

            buffer.Vertices[offset + points].Position = position;

            for (int i = 0, j = offset; i < points; i++)
                buffer.Vertices[j++].Position = framework[i] + position;
        }

        public override void repaint()
        {
            if (!initialized)
                return;

            buffer.Vertices[offset + points].Color = color;

            for (int i = 0, j = offset; i < points; i++)
                buffer.Vertices[j++].Color = color;
        }

        public override void Rescale()
        {
            for (int i = 0; i < points; i++)
                framework[i] = pattern.Data[i] * radius * scale;

            Move();
        }

        public float Radius
        {
            set
            {
                radius = value;
            }
        }
    }
}
