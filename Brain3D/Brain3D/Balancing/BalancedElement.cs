using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    abstract class BalancedElement
    {
        protected static Camera camera;

        protected Vector3 position;
        protected Vector3 shift;

        public virtual void move(Vector3 vector)
        {
            shift += vector;
        }

        public static Camera Camera
        {
            set
            {
                camera = value;
            }
        }

        public Vector3 Position
        {
            get
            {
                return position;
            }
        }
    }
}
