using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class SpriteElement : GraphicsElement
    {
        protected static SpriteBatch batch;

        protected Color color;
        protected Point position;

        public virtual void draw() { }

        public virtual void show()
        {
            display.add(this);
        }

        public virtual void hide()
        {
            display.remove(this);
        }

        public static SpriteBatch Batch
        {
            set
            {
                batch = value;
            }
        }
    }
}
