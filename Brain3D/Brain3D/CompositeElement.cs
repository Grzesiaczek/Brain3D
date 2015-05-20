using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Brain3D
{
    class CompositeElement : DrawableElement
    {
        protected List<DrawableElement> drawables;

        public CompositeElement()
        {
            drawables = new List<DrawableElement>();
        }

        public override void initialize()
        {
            foreach (DrawableElement drawable in drawables)
                drawable.initialize();
        }

        public override void move()
        {
            foreach (DrawableElement drawable in drawables)
                drawable.move();
        }

        public override void rotate()
        {
            base.rotate();

            foreach (DrawableElement drawable in drawables)
                drawable.rotate();
        }

        public override void show()
        {
            foreach (DrawableElement drawable in drawables)
                drawable.show();
        }

        public override void hide()
        {
            foreach (DrawableElement drawable in drawables)
                drawable.hide();
        }

        public override Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;

                foreach (DrawableElement drawable in drawables)
                    drawable.Position = position;
            }
        }
    }
}
