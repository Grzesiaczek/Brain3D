using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Brain3D
{
    class DrawableComposite : DrawableElement
    {
        protected List<DrawableElement> drawables;

        public DrawableComposite()
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

        public override void rescale()
        {
            foreach (DrawableElement drawable in drawables)
                drawable.rescale();
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

            visible = true;
        }

        public override void hide()
        {
            foreach (DrawableElement drawable in drawables)
                drawable.hide();

            visible = false;
        }

        public override void remove()
        {
            foreach (DrawableElement drawable in drawables)
                drawable.remove();
        }

        public override GraphicsBuffer Buffer
        {
            set
            {
                foreach (DrawableElement drawable in drawables)
                    drawable.Buffer = value;
            }
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

        public override float Scale
        {
            set
            {
                scale = value;

                foreach (DrawableElement drawable in drawables)
                    drawable.Scale = scale;
            }
        }
    }
}
