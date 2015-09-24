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
        protected static Display display;
        protected static BasicEffect effect;

        public virtual void Move() { }

        public virtual void Rotate() { }

        public virtual bool Cursor(int x, int y) { return false; }

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

        public static Display Display
        {
            set
            {
                display = value;
            }
        }

        public static BasicEffect Effect
        {
            set
            {
                effect = value;
            }
        }
    }
}
