using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class Line : DrawableElement
    {
        Vector3 source;
        Vector3 target;

        float size;

        public Line(Vector3 source, Vector3 target, Color color, float size)
        {
            this.source = source;
            this.target = target;
            this.color = color;
            this.size = size;

            vertices = new VertexPositionColor[4];
            indices = new int[6];
        }

        public override void Initialize()
        {
            Vector3 vector = target - source;
            double angle = Math.Atan(vector.Y / vector.X);
            vector = new Vector3(-(float)Math.Sin(angle), (float)Math.Cos(angle), 0) * size;

            vertices[0] = new VertexPositionColor(source - vector, color);
            vertices[1] = new VertexPositionColor(source + vector, color);
            vertices[2] = new VertexPositionColor(target - vector, color);
            vertices[3] = new VertexPositionColor(target + vector, color);

            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 3;
            indices[3] = 0;
            indices[4] = 3;
            indices[5] = 2;

            offset = buffer.Add(vertices, indices);
            initialized = true;
        }

        public void shift(Vector3 source, Vector3 target)
        {
            Vector3 vector = target - source;
            double angle = Math.Atan(vector.Y / vector.X);
            vector = new Vector3(-(float)Math.Sin(angle), (float)Math.Cos(angle), 0) * size;

            buffer.Vertices[offset + 0].Position = source - vector;
            buffer.Vertices[offset + 1].Position = source + vector;
            buffer.Vertices[offset + 2].Position = target - vector;
            buffer.Vertices[offset + 3].Position = target + vector;

            this.source = source;
            this.target = target;
        }

        public override void moveX(float x)
        {
            source = new Vector3(x, source.Y, source.Z);
            target = new Vector3(x, target.Y, target.Z);
            Vector3 vector = new Vector3(size, 0, 0);

            buffer.Vertices[offset + 0].Position = source - vector;
            buffer.Vertices[offset + 1].Position = source + vector;
            buffer.Vertices[offset + 2].Position = target - vector;
            buffer.Vertices[offset + 3].Position = target + vector;
        }
    }
}
