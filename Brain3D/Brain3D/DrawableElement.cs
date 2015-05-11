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

        protected List<GraphicsElement> elements;
        protected Color color;

        protected VertexPositionColor[] vertices;
        protected int[] indices;
        protected int offset;

        public DrawableElement()
        {
            elements = new List<GraphicsElement>();
        }

        public virtual void initialize() { }

        public override void refresh()
        {
            foreach (GraphicsElement element in elements)
                element.refresh();
        }

        public virtual void draw() { }

        public static Display Display
        {
            set
            {
                display = value;
            }
        }

        public GraphicsBuffer Buffer
        {
            set
            {
                buffer = value;
                buffer.add(this);
            }
        }

        public virtual Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }
    }    
}
