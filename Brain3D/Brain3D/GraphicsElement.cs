using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    abstract class GraphicsElement
    {
        protected static Camera camera;
        protected static ContentManager content;
        protected static GraphicsDevice device;
        protected static BasicEffect effect;

        protected Vector3[] framework;
        protected Vector3 position;

        public virtual void move() { }

        public virtual void rotate() { }

        public static Camera Camera
        {
            set
            {
                camera = value;
            }
        }

        public static ContentManager Content
        {
            set
            {
                content = value;
            }
        }

        public static GraphicsDevice Device
        {
            set
            {
                device = value;
            }
        }

        public static BasicEffect Effect
        {
            set
            {
                effect = value;
            }
        }

        public virtual Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
    }
}
