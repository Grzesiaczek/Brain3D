using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class Camera
    {
        Vector3 position;
        Vector3 target;
        Vector3 up;

        Spherical spherical;
        Matrix rotation;

        public Camera(float radius)
        {
            position = new Vector3(0, 0, -radius);
            target = Vector3.Zero;
            up = Vector3.Up;
            
            spherical = new Spherical(position);
            rotation = Matrix.CreateFromYawPitchRoll(-spherical.Longitude - Constant.PI2, spherical.Latitude, 0);
        }

        void refresh()
        {
            position = spherical.getVector();
            rotation = Matrix.CreateFromYawPitchRoll(-spherical.Longitude - Constant.PI2, spherical.Latitude, 0);
        }

        public void moveLeft()
        {
            spherical.Longitude += 0.025f;
            refresh();
        }

        public void moveRight()
        {
            spherical.Longitude -= 0.025f;
            refresh();
        }

        public void moveUp()
        {
            spherical.Latitude += 0.025f;
            refresh();
        }

        public void moveDown()
        {
            spherical.Latitude -= 0.025f;
            refresh();
        }

        public void farther()
        {
            spherical.Radius += 0.25f;
            refresh();
        }

        public void closer()
        {
            spherical.Radius -= 0.25f;
            refresh();
        }

        public Vector3 Position
        {
            get
            {
                return position;
            }
        }

        public Vector3 Target
        {
            get
            {
                return target;
            }
        }

        public Vector3 Up
        {
            get
            {
                return up;
            }
        }

        public Matrix Rotation
        {
            get
            {
                return rotation;
            }
        }
    }
}
