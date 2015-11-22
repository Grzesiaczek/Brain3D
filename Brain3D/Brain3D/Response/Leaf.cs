using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Brain3D
{
    class Leaf : DrawableComposite, Mouse
    {
        #region deklaracje

        Neuron neuron;

        Rect background;
        Rect border;

        LabelTL word;
        DottedLine line;

        Vector3 size;

        Vector3 screen;
        Vector2 shift;

        List<Branch> branches;

        int time;

        #endregion

        public Leaf(Neuron neuron, int time)
        {
            this.neuron = neuron;
            this.time = time;
            Prepare();
        }

        public Leaf(Leaf leaf)
        {
            neuron = leaf.neuron;
            time = leaf.time;
            Prepare();
        }

        void Prepare()
        {
            Vector2 font = Fonts.SpriteArial.MeasureString(neuron.Word) * 0.005f;
            branches = new List<Branch>();

            float width = 0.06f + font.X;
            float height = 0.04f + font.Y;

            Vector3 corner = new Vector3(0.01f * time, 0, 0);
            Vector3 shift = new Vector3(0.01f, 0.01f, 0);

            size = new Vector3(width, height, 0);
            position = corner + size / 2;
            screen = device.Viewport.Project(position, effect.Projection, effect.View, effect.World);
            Vector3 test = device.Viewport.Unproject(screen, effect.Projection, effect.View, effect.World);

            background = new Rect(corner + shift, size - 2 * shift, Color.LightYellow);
            border = new Rect(corner - new Vector3(0, 0, 0.001f), size, Color.Purple);
            word = new LabelTL(position + new Vector3(0, -0.024f, 0.001f), neuron.Word);
            line = new DottedLine(new Vector3(corner.X, -1, -0.001f), new Vector3(corner.X, 2, -0.001f), Color.MediumSlateBlue, 0.005f, 16);

            drawables.Add(background);
            drawables.Add(border);
            drawables.Add(word);
            drawables.Add(line);
        }

        public void Add(Branch branch)
        {
            branches.Add(branch);
        }

        public void Hover()
        {
            background.Color = Color.Yellow;
            screen = device.Viewport.Project(position, effect.Projection, effect.View, effect.World);
        }

        public void Idle()
        {
            background.Color = Color.LightYellow;
        }

        public void Move(int x, int y)
        {
            Position = device.Viewport.Unproject(new Vector3(screen.X, shift.Y + y, 0.6f), effect.Projection, effect.View, effect.World);
            screen = device.Viewport.Project(position, effect.Projection, effect.View, effect.World);

            background.Move();
            border.Move();
            word.Move();

            foreach(Branch branch in branches)
            {
                branch.Move();
            }
        }

        public void Push(int x, int y)
        {
            shift = new Vector2(screen.X - x, screen.Y - y);
        }

        public void Click(int x, int y) { }

        public bool Moved(int x, int y)
        {
            return false;
        }

        public override bool Cursor(int x, int y)
        {
            Vector3 upperLeft = device.Viewport.Project(position - size / 2, effect.Projection, effect.View, effect.World);
            Vector3 lowerRight = device.Viewport.Project(position + size / 2, effect.Projection, effect.View, effect.World);

            if(x < upperLeft.X || x > lowerRight.X)
            {
                return false;
            }

            if (y > upperLeft.Y || y < lowerRight.Y)
            {
                return false;
            }

            return true;
        }

        #region właściwości

        public Neuron Neuron
        {
            get
            {
                return neuron;
            }
        }

        public int Time
        {
            get
            {
                return time;
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
                background.moveY(value.Y - size.Y / 2 + 0.01f);
                border.moveY(value.Y - size.Y / 2);
                word.moveY(value.Y - 0.024f);
            }
        }

        public override float Scale
        {
            set
            {
                scale = value;

                if (visible)
                {
                    float x = 0.01f * time * scale;
                    position = new Vector3(x + size.X / 2, position.Y, position.Z);

                    background.MoveX(x + 0.01f);
                    word.MoveX(position.X);

                    border.MoveX(x);
                    line.MoveX(x);
                }
            }
        }

        #endregion
    }
}
