using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class Signal : DrawableElement
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

            pipe = new Pipe(new Circle(source, 0.25f, 4), new Circle(source, 0.05f, 4));
            pipe.Color = Color.Khaki;

            move();
        }

        public void setSignal(double factor)
        {
            if (!active)
            {
                move();
                active = true;
            }

            pipe.Start.Position = source + vector * (float)factor;
            pipe.End.Position = pipe.Start.Position + bullet;
            pipe.refresh();

            this.factor = factor;
        }

        void move()
        {
            vector = target - source;
            bullet = vector;
            bullet.Normalize();
            vector -= bullet;
        }

        public override void draw()
        {
            if (active)
                pipe.draw();
        }

        public override void initialize()
        {
            //base.initialize();
        }

        public override void refresh()
        {
            /*move();

            pipe.Start.Position = source + vector * (float)factor;
            pipe.End.Position = pipe.Start.Position + bullet;
            pipe.refresh();*/
        }

        public bool Active
        {
            set
            {
                active = value;
            }
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
