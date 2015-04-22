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

        Vector3 direction;

        int points;

        public Pipe(Circle start, Circle end)
        {
            this.start = start;
            this.end = end;

            points = start.Points;
            color = Color.Purple;

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

            for (int i = 0; i < 32; i++)
            {
                left[i] = new VertexPositionColor(start.Data[i], color);
                right[i] = new VertexPositionColor(end.Data[i], color);
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
                triangles[2 * i] = right[i];
                triangles[2 * i + 1] = left[i];
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
