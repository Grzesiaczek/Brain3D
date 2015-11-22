using System.Collections.Generic;
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

        public override void Initialize()
        {
            foreach (DrawableElement drawable in drawables)
            {
                drawable.Initialize();
            }
        }

        public override void Move()
        {
            foreach (DrawableElement drawable in drawables)
            {
                drawable.Move();
            }
        }

        public override void Rescale()
        {
            foreach (DrawableElement drawable in drawables)
            {
                drawable.Rescale();
            }
        }

        public override void Rotate()
        {
            base.Rotate();

            foreach (DrawableElement drawable in drawables)
            {
                drawable.Rotate();
            }
        }

        public override void Show()
        {
            if (!visible)
            {
                foreach (DrawableElement drawable in drawables)
                {
                    drawable.Show();
                }

                visible = true;
            }
        }

        public override void Hide()
        {
            if (visible)
            {
                foreach (DrawableElement drawable in drawables)
                {
                    drawable.Hide();
                }

                visible = false;
            }
        }

        public override void Remove()
        {
            foreach (DrawableElement drawable in drawables)
            {
                drawable.Remove();
            }
        }

        public override void MoveX(float x)
        {
            foreach (DrawableElement drawable in drawables)
            {
                drawable.MoveX(x);
            }
        }

        public override GraphicsBuffer Buffer
        {
            set
            {
                foreach (DrawableElement drawable in drawables)
                {
                    drawable.Buffer = value;
                }
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
                {
                    drawable.Position = position;
                }
            }
        }

        public override float Scale
        {
            set
            {
                scale = value;

                foreach (DrawableElement drawable in drawables)
                {
                    drawable.Scale = scale;
                }
            }
        }
    }
}
