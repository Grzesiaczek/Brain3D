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

        public virtual void Initialize() { }

        public virtual void Repaint() { }

        public virtual void Rescale() { }

        public override void Move()
        {
            lock (buffer.Vertices)
            {
                for (int i = 0, j = offset; i < vertices.Length; i++, j++)
                {
                    buffer.Vertices[j].Position = framework[i] + position;
                }
            }
        }

        public void refresh()
        {
            if (initialized)
            {
                Move();
                Repaint();
            }
        }

        public virtual void Remove()
        {
            Hide();
            initialized = false;
        }

        public virtual void Show()
        {
            if (buffer == null)
            {
                display.Add(this);
            }
            else
            {
                buffer.Show(indices);
            }

            visible = true;
        }

        public virtual void Hide()
        {
            if (visible)
            {
                if (buffer != null)
                {
                    buffer.Hide(indices);
                }

                visible = false;
            }
        }

        public virtual void MoveX(float x)
        {
            position = new Vector3(x, position.Y, position.Z);
            Move();
        }

        public virtual void moveY(float y)
        {
            position = new Vector3(position.X, y, position.Z);
        }

        public virtual GraphicsBuffer Buffer
        {
            set
            {
                buffer = value;

                if (buffer != null)
                {
                    buffer.Add(this);
                }
            }
        }

        public virtual Color Color
        {
            set
            {
                color = value;

                if (initialized)
                {
                    Repaint();
                }
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
                {
                    Rescale();
                }
            }
        }
    }    
}
