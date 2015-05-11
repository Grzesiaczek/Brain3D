﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    abstract class AnimatedElement : DrawableElement
    {
        #region deklaracje

        protected static bool animation;

        protected Vector3 location;
        protected Vector3 screen;

        protected float factor;
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

        public void update()
        {
            position = location;
        }

        public virtual Vector3 pointVector(Vector2 angle)
        {
            return position;
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

        public Vector3 Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
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
