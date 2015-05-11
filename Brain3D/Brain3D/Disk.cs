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
        Circle data;        
        int points;

        public Disk(Vector3 center, Circle circle, Color color)
        {
            position = center;
            data = circle;

            points = data.Points;
            this.color = color;

            vertices = new VertexPositionColor[points + 1];
            indices = new int[3 * points];
        }

        public override void initialize()
        {
            position = data.Position;
            vertices[points] = new VertexPositionColor(position, color);

            for (int i = 0; i < points; i++)
                vertices[i] = new VertexPositionColor(data.Data[i], color);

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

        public override void refresh()
        {
            position = data.Position;
            buffer.Vertices[offset + points].Position = position;

            for (int i = 0, j = offset; i < points; i++)
                buffer.Vertices[j++].Position = data.Data[i];
        }
    }
}
