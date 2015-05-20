﻿using System;
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
        Vector3[] data;
        Vector3 direction;

        static float[] cos;
        static float[] sin;

        static int total = 128;

        int factor;
        int points;

        public Circle(Vector3 position, int factor)
        {
            this.position = position;
            this.factor = factor;

            direction = Vector3.Zero;
            points = total / factor;

            circle = new Vector3[points];
            data = new Vector3[points];
            framework = new Vector3[points];

            for (int i = 0, j = 0; i < points; i++, j += factor)
                circle[i] = new Vector3(cos[j], sin[j], 0);

            rotate();
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

        public override void move()
        {
            for (int i = 0; i < points; i++)
                data[i] = framework[i] + position;
        }

        public override void rotate()
        {
            if (direction.Length() == 0)
                for (int i = 0; i < points; i++)
                    framework[i] = Vector3.Transform(circle[i], camera.Rotation);
            else
            {
                Matrix rotation = new Spherical(direction).getRotation();
                //rotation = new Spherical(new Vector3(0, 0, 1)).getRotation();

                for (int i = 0; i < points; i++)
                    framework[i] = Vector3.Transform(circle[i], rotation);
            }

            move();
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
                return data;
            }
        }
    }
}
