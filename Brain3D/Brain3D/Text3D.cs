using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Fonts;
using Nuclex.Graphics;

namespace Brain3D
{
    class Text3D : DrawableElement
    {
        protected static VectorFont font;

        protected Text text;
        protected Vector3 shift;
        protected float scale;

        Vector3[] pattern;
        bool suppress;

        protected Text3D() { }

        public Text3D(String data, Vector3 position)
        {
            this.position = position;
            text = font.Fill(data);
            color = Color.DarkSlateBlue;
            scale = 0.07f;
            shift = new Vector3(-text.Width * scale / 2, 0, -0.01f);
        }

        public static void load()
        {
            font = content.Load<VectorFont>("Standard");
        }

        public override void initialize()
        {
            int vertex = text.Vertices.Length;
            int index = text.Indices.Length;

            vertices = new VertexPositionColor[vertex];
            pattern = new Vector3[vertex];
            indices = new int[index];

            for (int i = 0; i < vertex; i++)
            {
                pattern[i] = text.Vertices[i].Position * scale + shift;
                pattern[i].X *= -1;
                vertices[i] = new VertexPositionColor(Vector3.Transform(pattern[i], camera.Rotation) + position, Color.Black);
            }

            for (int i = 0; i < index; i++)
                indices[i] = text.Indices[i];

            offset = buffer.add(vertices, indices);
        }

        public override void refresh()
        {
            int index = offset;

            for (int i = 0; i < vertices.Length; i++)
                buffer.Vertices[index++].Position = Vector3.Transform(pattern[i], camera.Rotation) + position;
        }

        public String Text
        {
            set
            {
                text = font.Fill(value);
                shift = new Vector3(-text.Width * scale / 2, 0, -0.01f);
                initialize();
            }
        }

        public bool Suppress
        {
            set
            {
                suppress = value;
            }
        }
    }
}
