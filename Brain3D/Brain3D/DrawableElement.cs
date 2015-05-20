using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    abstract class DrawableElement : GraphicsElement
    {
        protected static Display display;

        protected GraphicsBuffer buffer;
        protected Color color;

        protected VertexPositionColor[] vertices;
        protected int[] indices;
        protected int offset;

        public virtual void initialize() { }

        public virtual void repaint() { }

        public override void move()
        {
            for (int i = 0, j = offset; i < vertices.Length; i++, j++)
                buffer.Vertices[j].Position = framework[i] + position;
        }

        public void refresh()
        {
            move();
            repaint();
        }

        public virtual void show()
        {
            if (buffer == null)
                display.add(this);
            else
                buffer.show(indices);
        }

        public virtual void hide()
        {
            buffer.hide(indices);
        }

        public static Display Display
        {
            set
            {
                display = value;
            }
        }

        public virtual GraphicsBuffer Buffer
        {
            set
            {
                buffer = value;
                buffer.add(this);
            }
        }

        public virtual Color Color
        {
            set
            {
                color = value;
                repaint();
            }
        }
    }    
}
