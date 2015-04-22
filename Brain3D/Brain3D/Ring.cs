using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class Ring : DrawableElement
    {
        Circle innerCircle;
        Circle outerCircle;

        VertexPositionColor[] inner;
        VertexPositionColor[] outer;

        int points;
        int points2;

        public Ring(Circle inner, Circle outer, Color color)
        {
            innerCircle = inner;
            outerCircle = outer;

            this.color = color;
            points = inner.Points;
            points2 = 2 * points;

            this.inner = new VertexPositionColor[points + 1];
            this.outer = new VertexPositionColor[points + 1];

            refresh();
        }

        public override void refresh()
        {
            for (int i = 0; i < points; i++)
            {
                this.inner[i] = new VertexPositionColor(innerCircle.Data[i], color);
                this.outer[i] = new VertexPositionColor(outerCircle.Data[i], color);
            }

            inner[points] = inner[0];
            outer[points] = outer[0];
        }

        public override void draw()
        {
            VertexPositionColor[] triangles = new VertexPositionColor[points2 + 2];

            for (int i = 0; i < points; i++)
            {
                triangles[2 * i] = inner[i];
                triangles[2 * i + 1] = outer[i];
            }

            triangles[points2] = triangles[0];
            triangles[points2 + 1] = triangles[1];

            device.DrawUserPrimitives(PrimitiveType.TriangleStrip, triangles, 0, points2);
        }
    }
}
