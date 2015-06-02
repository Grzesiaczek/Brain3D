using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Brain3D
{
    class Spherical
    {
        float radius;
        float longitude;
        float latitude;

        public Spherical(float radius, float longitude, float latitude)
        {
            this.radius = radius;
            this.longitude = longitude;
            this.latitude = latitude;
        }

        public Spherical(Vector3 vector)
        {
            radius = vector.Length();
            latitude = (float)Math.Asin(vector.Y / radius);

            if (vector.Z != 0)
                longitude = (float)Math.Acos(vector.X / Math.Sqrt(vector.X * vector.X + vector.Z * vector.Z)) * (vector.Z < 0 ? -1 : 1);
            else if (vector.X > 0)
                longitude = 0;
            else
                longitude = (float)Math.PI;
        }

        public Vector3 getVector()
        {
            float x = (float)(radius * Math.Cos(latitude) * Math.Cos(longitude));
            float y = (float)(radius * Math.Sin(latitude));
            float z = (float)(radius * Math.Cos(latitude) * Math.Sin(longitude));

            return new Vector3(x, y, z);
        }

        public Matrix getRotation()
        {
            return Matrix.CreateFromYawPitchRoll(-longitude - Constant.PI2, latitude, 0);
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

        public float Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                longitude = value;
            }
        }

        public float Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                latitude = value;

                if (latitude < -1.5f)
                    latitude = -1.5f;

                if (latitude > 1.5f)
                    latitude = 1.5f;
            }
        }
    }
}
