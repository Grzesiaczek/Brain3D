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
        protected GraphicsBuffer buffer;
        protected Color color;
        protected Vector3 position;

        protected Vector3[] framework;
        protected VertexPositionColor[] vertices;
        protected int[] indices;

        protected int offset;
        protected float scale;
        
        protected bool initialized;
        protected bool visible;

        public virtual void initialize() { }

        public virtual void repaint() { }

        public virtual void rescale() { }

        public override void move()
        {
            lock (buffer.Vertices)
            {
                for (int i = 0, j = offset; i < vertices.Length; i++, j++)
                    buffer.Vertices[j].Position = framework[i] + position;
            }
        }

        public void refresh()
        {
            if (initialized)
            {
                move();
                repaint();
            }
        }

        public virtual void remove()
        {
            hide();
            initialized = false;
        }

        public virtual void show()
        {
            if (buffer == null)
                display.add(this);
            else
                buffer.show(indices);

            visible = true;
        }

        public virtual void hide()
        {
            buffer.hide(indices);
            visible = false;
        }

        public virtual GraphicsBuffer Buffer
        {
            set
            {
                buffer = value;

                if (buffer != null)
                    buffer.add(this);
            }
        }

        public virtual Color Color
        {
            set
            {
                color = value;

                if (initialized)
                    repaint();
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

        public virtual float Scale
        {
            set
            {
                scale = value;

                if (initialized)
                    rescale();
            }
        }
    }    
}
