using Microsoft.Xna.Framework;

namespace Brain3D
{
    class Signal : DrawableComposite
    {
        Vector3 source;
        Vector3 target;

        Vector3 vector;
        Vector3 bullet;

        Pipe pipe;

        double factor;
        bool active;

        public Signal(Vector3 source, Vector3 target)
        {
            this.source = source;
            this.target = target;

            pipe = new Pipe(source, source, 0.5f, 0.1f, 1);
            pipe.Color = Color.Khaki;

            Shift();
        }

        public void SetSignal(double factor)
        {
            if(factor == -1)
            {
                Hide();
                return;
            }

            if (!active)
            {
                Activate();
            }

            this.factor = factor;
            Rotate();
            pipe.Move();
        }

        void Shift()
        {
            vector = target - source;
            bullet = vector;
            bullet.Normalize();
            bullet *= 1.6f;
            vector -= bullet / 2;
        }

        void Activate()
        {
            pipe.Scale = 1;
            pipe.Show();
            active = true;
        }

        public override void Move()
        {
            pipe.Move();
        }

        public override void Rotate()
        {
            if (active)
            {
                Shift();
                pipe.Source = source + vector * (float)factor + new Vector3(0, 0, 0.05f);
                pipe.Target = pipe.Source + bullet + new Vector3(0, 0, 0.05f);
                pipe.Rotate();
            }
        }

        public override void Show()
        {
            pipe.Add();
        }

        public override void Hide()
        {
            pipe.Hide();
            pipe.Move();
            active = false;
        }

        public Vector3 Source
        {
            set
            {
                source = value;
            }
        }

        public Vector3 Target
        {
            set
            {
                target = value;
            }
        }
    }
}
