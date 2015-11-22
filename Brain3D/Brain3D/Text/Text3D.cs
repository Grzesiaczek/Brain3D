using System;
using Microsoft.Xna.Framework;
using Nuclex.Fonts;

namespace Brain3D
{
    class Text3D : DrawableElement
    {
        protected Text text;
        protected Vector3 shift;
        protected float ratio;

        protected Vector3[] pattern;

        protected static Tuple<Vector3[], int[]> getPattern(String data)
        {
            Text text = Fonts.VectorArial.Fill(data);
            int vertex = text.Vertices.Length;
            int index = text.Indices.Length;

            float scale = 0.032f;
            Vector3 shift = new Vector3(-text.Width * scale / 2, 0, -0.01f);

            Vector3[] vertices = new Vector3[vertex];
            int[] indices = new int[index];

            for (int i = 0; i < vertex; i++)
            {
                vertices[i] = text.Vertices[i].Position * scale + shift;
                vertices[i].X *= -1;
            }

            for (int i = 0; i < index; i++)
            {
                indices[i] = text.Indices[i];
            }

            return new Tuple<Vector3[], int[]>(vertices, indices);
        }

        public override void Rotate()
        {
            if (initialized)
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    framework[i] = Vector3.Transform(pattern[i], camera.Rotation) * scale;
                }

                Move();
            }
        }

        public override void Rescale()
        {
            Rotate();
        }
    }
}
