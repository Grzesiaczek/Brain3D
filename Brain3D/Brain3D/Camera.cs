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

        public Camera(Vector3 position)
        {
            this.position = position;
            target = new Vector3(position.X, 0, 0);
            initialize();
        }

        public Camera(float radius)
        {
            position = new Vector3(0, 0, radius);
            target = Vector3.Zero;
            initialize();
        }

        void initialize()
        {
            up = Vector3.Up;

            spherical = new Spherical(position);
            rotation = Matrix.CreateFromYawPitchRoll(-spherical.Longitude - Constant.PI2, spherical.Latitude, 0);

            Constant.spaceChanged += new EventHandler(spaceChanged);
        }

        void refresh()
        {
            position = spherical.getVector();
            rotation = Matrix.CreateFromYawPitchRoll(-spherical.Longitude - Constant.PI2, spherical.Latitude, 0);
        }

        public void moveLeft()
        {
            if (Constant.Space == SpaceMode.Box)
            {
                position.X -= 0.25f;

                if (position.X + Constant.Box.X < 0)
                    position.X = -Constant.Box.X;

                target.X = position.X;
            }
            else
            {
                spherical.Longitude += 0.025f;
                refresh();
            }
        }

        public void moveRight()
        {
            if (Constant.Space == SpaceMode.Box)
            {
                position.X += 0.25f;

                if (position.X > Constant.Box.X)
                    position.X = Constant.Box.X;

                target.X = position.X;
            }
            else
            {
                spherical.Longitude -= 0.025f;
                refresh();
            }
        }

        public void moveUp()
        {
            if (Constant.Space == SpaceMode.Box)
            {
                position.Y += 0.25f;

                if (position.Y > Constant.Box.Y)
                    position.Y = Constant.Box.Y;

                target.Y = position.Y;
            }
            else
            {
                spherical.Latitude += 0.025f;
                refresh();
            }
        }

        public void moveDown()
        {
            if (Constant.Space == SpaceMode.Box)
            {
                position.Y -= 0.25f;

                if (position.Y + Constant.Box.Y < 0)
                    position.Y = -Constant.Box.Y;

                target.Y = position.Y;
            }
            else
            {
                spherical.Latitude -= 0.025f;
                refresh();
            }
        }

        public void farther()
        {
            if (Constant.Space == SpaceMode.Box)
            {
                position.Z += 0.25f;

                if (position.Z > 100)
                    position.Z = 100;
            }
            else
            {
                spherical.Radius += 0.25f;
                refresh();
            }
        }

        public void closer()
        {
            if (Constant.Space == SpaceMode.Box)
            {
                position.Z -= 0.25f;

                if (position.Z < 10)
                    position.Z = 10;
            }
            else
            {
                spherical.Radius -= 0.25f;
                refresh();
            }
        }

        void spaceChanged(object sender, EventArgs e)
        {
            if (Constant.Space == SpaceMode.Sphere)
                spherical = new Spherical(position);
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
