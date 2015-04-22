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
        protected static Display display;

        protected Vector3 screen;

        protected float factor;
        protected bool creation;
        protected bool visible;

        #endregion

        public AnimatedElement()
        {
        }

        #region logika

        public override void refresh()
        {
            base.refresh();
            screen = device.Viewport.Project(position, effect.Projection, effect.View, effect.World);
        }

        public override void draw()
        {
            //device.DepthStencilState = DepthStencilState.Default;
            effect.CurrentTechnique.Passes[0].Apply();

            foreach (DrawableElement drawable in drawables)
                drawable.draw();
        }

        public virtual void tick(double time) { }

        public virtual void setFrame(int frame) { }

        public virtual void show()
        {
            if (visible)
                return;

            display.add(this);
            visible = true;
            refresh();
        }

        public virtual void hide()
        {
            if (!visible)
                return;

            display.remove(this);
            visible = false;
        }

        #endregion

        #region właściwości

        public static Display Display
        {
            set
            {
                display = value;
            }
        }

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

        public float Factor
        {
            get
            {
                return factor;
            }
            set
            {
                factor = value;
            }
        }

        #endregion
    }
}
