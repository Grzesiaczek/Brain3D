using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    abstract class AnimatedElement : DrawableComposite, Mouse
    {
        #region deklaracje

        protected static bool animation;

        protected Vector3 screen;
        protected Vector2 shift;
        protected Point start;

        protected bool added;

        #endregion

        #region logika

        public virtual void activate() { }

        public virtual void idle() { }

        public virtual void hover() { }

        public virtual void tick(double time) { }

        public virtual void setFrame(int frame) { }

        public virtual void click(int x, int y) { }

        public virtual void move(int x, int y) { }

        public void push(int x, int y)
        {
            shift = new Vector2(screen.X - x, screen.Y - y);
            start = new Point(x, y);
        }

        public bool moved(int x, int y)
        {
            if (Math.Abs(start.X - x) > 1 || Math.Abs(start.Y - y) > 1)
                return true;

            return false;
        }

        public override void show()
        {
            base.show();

            if (!added)
            {
                display.add(this);
                added = true;
            }
        }

        public override void remove()
        {
            added = false;
            visible = added;
            base.remove();
        }

        #endregion

        #region właściwości

        public static bool Animation
        {
            set
            {
                animation = value;
            }
        }

        public virtual float Depth
        {
            get
            {
                return screen.Z;
            }
        }

        public virtual Vector3 Screen
        {
            get
            {
                return screen;
            }
        }

        #endregion
    }
}
