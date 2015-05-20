using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    abstract class AnimatedElement : CompositeElement
    {
        #region deklaracje

        protected static bool animation;
        protected Vector3 screen;
        protected bool visible;

        #endregion

        #region logika

        public virtual Vector3 pointVector(Vector2 angle)
        {
            return position;
        }

        public virtual void tick(double time) { }

        public virtual void setFrame(int frame) { }

        public override void show()
        {
            base.show();

            if (visible)
                return;

            display.add(this);
            visible = true;
        }

        public override void hide()
        {
            base.hide();

            if (!visible)
                return;

            display.remove(this);
            visible = false;
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

        public Vector3 Screen
        {
            get
            {
                return screen;
            }
        }

        #endregion
    }
}
