using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class Disk : DrawableElement
    {
        Circle data;

        VertexPositionColor center;
        VertexPositionColor[] circle;
        
        int points;

        public Disk(Vector3 center, Circle circle, Color color)
        {
            position = center;
            data = circle;

            points = data.Points;
            this.circle = new VertexPositionColor[points + 1];

            this.color = color;
            refresh();
        }

        public override void refresh()
        {
            position = data.Position;
            center = new VertexPositionColor(position, color);

            for (int i = 0; i < points; i++)
                circle[i] = new VertexPositionColor(data.Data[i], color);

            circle[points] = circle[0];
        }

        public override void draw()
        {
            VertexPositionColor[] triangle = new VertexPositionColor[384];
            
            for (int i = 0; i < points; i++)
            {
                int k = 3 * i;
                triangle[k] = center;
                triangle[k + 1] = circle[i];
                triangle[k + 2] = circle[i + 1];
            }

            device.DrawUserPrimitives(PrimitiveType.TriangleList, triangle, 0, 128); 
        }
    }
}
