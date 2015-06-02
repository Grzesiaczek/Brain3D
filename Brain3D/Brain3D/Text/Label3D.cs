using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Fonts;
using Nuclex.Graphics;

namespace Brain3D
{
    class Label3D : Text3D
    {
        public Label3D(String data, Vector3 position)
        {
            this.position = position;
            text = font.Fill(data);
            color = Color.DarkBlue;

            if (text.Width > 40)
                ratio = 1.6f / text.Width;
            else
                ratio = 0.04f;
            
            shift = new Vector3(-text.Width * ratio / 2, -0.7f, -0.01f);
        }

        public override void initialize()
        {
            int vertex = text.Vertices.Length;
            int index = text.Indices.Length;

            vertices = new VertexPositionColor[vertex];
            pattern = new Vector3[vertex];
            framework = new Vector3[vertex];
            indices = new int[index];

            for (int i = 0; i < vertex; i++)
            {
                pattern[i] = text.Vertices[i].Position * ratio + shift;
                pattern[i].X *= -1;
                framework[i] = Vector3.Transform(pattern[i], camera.Rotation) * scale;
                vertices[i] = new VertexPositionColor(framework[i] + position, Color.Black);
            }

            for (int i = 0; i < index; i++)
                indices[i] = text.Indices[i];

            offset = buffer.add(vertices, indices);
            initialized = true;
        }
    }
}
