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
            target = new Vector3(position.X, position.Y, 0);
            Initialize();
        }

        public Camera(float radius)
        {
            position = new Vector3(0, 0, radius);
            target = Vector3.Zero;
            Initialize();
        }

        void Initialize()
        {
            up = Vector3.Up;

            spherical = new Spherical(position);
            rotation = Matrix.CreateFromYawPitchRoll(-spherical.Longitude - Constant.PI2, spherical.Latitude, 0);

            Constant.spaceChanged += new EventHandler(SpaceChanged);
        }

        void Refresh()
        {
            position = spherical.getVector();
            rotation = Matrix.CreateFromYawPitchRoll(-spherical.Longitude - Constant.PI2, spherical.Latitude, 0);
        }

        public void MoveX(float value)
        {
            position.X = value;
            target.X = value;
        }

        public void Rescale(Vector2 factor)
        {
            position.X *= factor.X;
            target.X *= factor.X;

            position.Y *= factor.Y;
            target.Y *= factor.Y;
        }

        public void MoveLeft()
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
                Refresh();
            }
        }

        public void MoveRight()
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
                Refresh();
            }
        }

        public void MoveUp()
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
                Refresh();
            }
        }

        public void MoveDown()
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
                Refresh();
            }
        }

        public void Farther()
        {
            if(Constant.Space == SpaceMode.Sphere)
            {
                spherical.Radius += 0.25f;
                Refresh();
            }
            else
            {
                float border = 100;

                if (Constant.Space == SpaceMode.Chart)
                    border = 5;

                position.Z += 0.25f;

                if (position.Z > border)
                    position.Z = border;

            }
        }

        public void Closer()
        {
            if (Constant.Space == SpaceMode.Sphere)
            {
                spherical.Radius -= 0.25f;
                Refresh();
            }
            else
            {
                float border = 10;

                if (Constant.Space == SpaceMode.Chart)
                    border = 1;

                position.Z -= 0.25f;

                if (position.Z < border)
                    position.Z = border;

            }
        }

        void SpaceChanged(object sender, EventArgs e)
        {
            if (Constant.Space == SpaceMode.Sphere)
                spherical = new Spherical(position);
        }

        #region właściwości

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

        #endregion
    }
}
