using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class SpriteElement : GraphicsElement
    {
        protected static SpriteBatch batch;

        protected Color color;
        protected Point position;

        public virtual void Draw() { }

        public virtual void Show()
        {
            display.Add(this);
        }

        public virtual void Hide()
        {
            display.Remove(this);
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
