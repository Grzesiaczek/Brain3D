using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class Circle : GraphicsElement
    {
        Vector3[] circle;
        Vector3[] framework;
        Vector3 direction;

        int factor;
        int points;

        public Circle(int factor)
        {
            this.factor = factor;

            direction = Vector3.Zero;
            points = Constant.Circle.Length / factor;

            circle = new Vector3[points];
            framework = new Vector3[points];

            for (int i = 0, j = 0; i < points; i++, j += factor)
                circle[i] = new Vector3(Constant.Circle[j], 0);

            rotate();
        }


        public override void rotate()
        {
            if (direction.Length() == 0)
                for (int i = 0; i < points; i++)
                    framework[i] = Vector3.Transform(circle[i], camera.Rotation);
            else
            {
                Matrix rotation = new Spherical(direction).getRotation();

                for (int i = 0; i < points; i++)
                    framework[i] = Vector3.Transform(circle[i], rotation);
            }
        }

        public Vector3 Direction
        {
            set
            {
                direction = value;
            }
        }

        public int Points
        {
            get
            {
                return points;
            }
        }

        public Vector3[] Data
        {
            get
            {
                return framework;
            }
        }
    }
}
