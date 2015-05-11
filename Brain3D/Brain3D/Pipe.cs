using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class Pipe : DrawableElement
    {
        Circle start;
        Circle end;

        Color[] palette;
        Vector3 direction;

        int points;

        public Pipe(Circle start, Circle end)
        {
            this.start = start;
            this.end = end;

            points = start.Points;
            palette = new Color[points];

            palette[0] = Color.Purple;
            palette[8] = Color.Brown;

            int r = palette[8].R - palette[0].R;
            int g = palette[8].G - palette[0].G;
            int b = palette[8].B - palette[0].B;

            for (int i = 1; i < 8; i++)
            {
                int red = palette[0].R + i * r / 8;
                int green = palette[0].G + i * g / 8;
                int blue = palette[0].B + i * b / 8;

                palette[i] = new Color(red, green, blue);
            }

            for (int i = 9; i < 16; i++)
                palette[i] = palette[16 - i];

            for (int i = 16; i < 32; i++)
                palette[i] = palette[i - 16];

            vertices = new VertexPositionColor[2 * points];
            indices = new int[6 * points];
        }

        public override void initialize()
        {
            for (int i = 0, j = 0; i < points; i++)
            {
                vertices[j++] = new VertexPositionColor(start.Data[i], palette[i]);
                vertices[j++] = new VertexPositionColor(end.Data[i], palette[i]);
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
            direction = end.Position - start.Position;
            start.Direction = direction;
            end.Direction = direction;

            start.refresh();
            end.refresh();

            for (int i = 0, j = offset; i < points; i++)
            {
                buffer.Vertices[j++].Position = start.Data[i];
                buffer.Vertices[j++].Position = end.Data[i];
            }
        }

        public Circle Start
        {
            get
            {
                return start;
            }
        }

        public Circle End
        {
            get
            {
                return end;
            }
        }
    }
}
