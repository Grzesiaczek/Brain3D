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
        protected List<GraphicsElement> elements;
        protected Color color;

        public DrawableElement()
        {
            elements = new List<GraphicsElement>();
        }

        public override void refresh()
        {
            foreach (GraphicsElement element in elements)
                element.refresh();
        }

        public abstract void draw();

        public virtual Color Color
        {
            set
            {
                color = value;
            }
        }
    }    
}
