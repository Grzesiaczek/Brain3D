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

        VertexPositionColor[] left;
        VertexPositionColor[] right;

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

            left = new VertexPositionColor[points + 1];
            right = new VertexPositionColor[points + 1];

            refresh();
        }

        public override void refresh()
        {
            direction = end.Position - start.Position;
            start.Direction = direction;
            end.Direction = direction;

            start.refresh();
            end.refresh();

            for (int i = 0; i < points; i++)
            {
                left[i] = new VertexPositionColor(start.Data[i], palette[i]);
                right[i] = new VertexPositionColor(end.Data[i], palette[i]);
            }

            left[points] = left[0];
            right[points] = right[0];
        }

        public override void draw()
        {
            effect.CurrentTechnique.Passes[0].Apply();

            VertexPositionColor[] triangles = new VertexPositionColor[2 * points + 2];

            for (int i = 0; i < points; i++)
            {
                triangles[2 * i] = left[i];
                triangles[2 * i + 1] = right[i];
            }

            triangles[2 * points] = triangles[0];
            triangles[2 * points + 1] = triangles[1];

            device.DrawUserPrimitives(PrimitiveType.TriangleStrip, triangles, 0, 2 * points);
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
