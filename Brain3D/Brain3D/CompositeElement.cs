using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    abstract class CompositeElement : DrawableElement
    {
        protected List<DrawableElement> drawables;

        public CompositeElement()
        {
            drawables = new List<DrawableElement>();
        }

        public override void refresh()
        {
            base.refresh();

            foreach (DrawableElement drawable in drawables)
                drawable.refresh();
        }

        public override void draw()
        {
            foreach (DrawableElement drawable in drawables)
                drawable.draw();
        }
    }
}
