using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class Rect : DrawableElement
    {
        Vector3 size;

        public Rect(Vector3 position, Vector3 size, Color color)
        {
            this.position = position;
            this.size = size;
            this.color = color;

            //framework = new Vector3[points];
            vertices = new VertexPositionColor[4];
            indices = new int[6];
        }

        public override void initialize()
        {
            vertices[0] = new VertexPositionColor(position, color);
            vertices[1] = new VertexPositionColor(position + new Vector3(0, size.Y, 0), color);
            vertices[2] = new VertexPositionColor(position + new Vector3(size.X, 0, 0), color);
            vertices[3] = new VertexPositionColor(position + size, color);

            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 3;
            indices[3] = 0;
            indices[4] = 3;
            indices[5] = 2;

            offset = buffer.add(vertices, indices);
            initialized = true;
        }

        public override void move()
        {
            buffer.Vertices[offset + 0].Position = position;
            buffer.Vertices[offset + 1].Position = position + new Vector3(0, size.Y, 0);
            buffer.Vertices[offset + 2].Position = position + new Vector3(size.X, 0, 0);
            buffer.Vertices[offset + 3].Position = position + size;
        }
    }
}
