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
        Vector3[] data;
        Vector3 direction;

        static float[] cos;
        static float[] sin;

        static int total = 128;

        float radius;

        int points;
        int factor;

        public Circle(Vector3 position, float radius, int factor)
        {
            this.position = position;
            this.radius = radius;
            this.factor = factor;

            direction = Vector3.Zero;
            points = total / factor;
            data = new Vector3[points];
            refresh();
        }

        public static void initialize()
        {
            float interval = (float)Math.PI * 2/ total;

            cos = new float[total];
            sin = new float[total];

            for (int i = 0; i < total; i++)
            {
                cos[i] = (float)Math.Cos(interval * i);
                sin[i] = (float)Math.Sin(interval * i);
            }
        }

        public override void refresh()
        {
            if (direction.Length() == 0)
            {
                for (int i = 0, j = 0; i < points; i++, j += factor)
                    data[i] = Vector3.Transform(new Vector3(cos[j] * radius, sin[j] * radius, 0), camera.Rotation) + position;

                return;
            }

            for (int i = 0, j = 0; i < points; i++, j += factor)
                data[i] = Vector3.Transform(new Vector3(cos[j] * radius, sin[j] * radius, 0), new Spherical(direction).getRotation()) + position;
        }

        public int Points
        {
            get
            {
                return points;
            }
        }

        public Vector3 Direction
        {
            set
            {
                direction = value;
            }
        }

        public float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }

        public Vector3[] Data
        {
            get
            {
                return data;
            }
        }
    }
}
