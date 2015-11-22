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

            framework = new Vector3[4];
            vertices = new VertexPositionColor[4];
            indices = new int[6];
        }

        public override void Initialize()
        {
            framework[0] = Vector3.Zero;
            framework[1] = new Vector3(0, size.Y, 0);
            framework[2] = new Vector3(size.X, 0, 0);
            framework[3] = size;

            for (int i = 0; i < 4; i++)
            {
                vertices[i] = new VertexPositionColor(position + framework[i], color);
            }

            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 3;
            indices[3] = 0;
            indices[4] = 3;
            indices[5] = 2;

            offset = buffer.Add(vertices, indices);
            initialized = true;
        }

        public override void Move()
        {
            for (int i = 0, j = offset; i < 4; i++)
            {
                buffer.Vertices[j++].Position = position + framework[i];
            }
        }

        public override void Repaint()
        {
            if (initialized)
            {
                for (int i = 0, j = offset; i < 4; i++)
                {
                    buffer.Vertices[j++].Color = color;
                }
            }
        }
    }
}
