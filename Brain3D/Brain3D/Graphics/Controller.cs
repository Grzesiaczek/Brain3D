using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class Controller : SpriteComposite
    {
        Text2D fps;
        Text2D status;

        CounterTile counter;
        TrackBar trackBar;

        List<DateTime> times = new List<DateTime>();
        Action<int> updateFrame;

        String normal;
        bool insertion;

        int count = 0;
        int frame;
        int frames;

        public Controller()
        {
            fps = new Text2D("FPS: 0", Fonts.SpriteVerdana, new Vector2(position.X, 20), Color.DarkMagenta);
            status = new Text2D("Waiting...", Fonts.SpriteVerdana, new Vector2(position.X, 50), Color.DarkOliveGreen);
            counter = new CounterTile(new Point(20, display.Height - 80), 0);

            Balancing.Instance.balanceState += new EventHandler(updateState);
            updateFrame = new Action<int>((value) => trackBar.Value = value);

            Presentation.Controller = this;
            normal = "Status OK";

            sprites.Add(fps);
            sprites.Add(status);
            sprites.Add(counter);
        }

        public override void draw()
        {
            times.Add(DateTime.Now);

            if (count > 100)
            {
                TimeSpan time = times[count] - times[count - 100];
                double span = time.TotalMilliseconds;
                fps.Text = "FPS: " + (int)(100000 / span);
            }
            else if (count != 0)
            {
                TimeSpan time = times[count] - times[0];
                double span = time.TotalMilliseconds;
                fps.Text = "FPS: " + (int)(1000 * count / span);
            }

            base.draw();
            count++;
        }

        public void changeFrame(int frame)
        {
            this.frame = frame;
            trackBar.Invoke(updateFrame, frame);
            counter.Value = frame;
        }

        public void changeState(int frame, int frames)
        {
            this.frames = frames;
            trackBar.Maximum = frames;
            changeFrame(frame);
        }

        public override void show()
        {
            display.add(this);
            insertion = false;
        }

        public void add(TrackBar trackBar)
        {
            this.trackBar = trackBar;
        }

        public void idle()
        {
            status.Text = normal;
        }

        void updateState(object sender, EventArgs e)
        {
            if (insertion)
                return;

            Balancing.Phase phase = (Balancing.Phase)sender;

            switch(phase)
            {
                case Balancing.Phase.One:
                    status.Text = "Faza 1";
                    break;
                case Balancing.Phase.Two:
                    status.Text = "Faza 2";
                    break;
                case Balancing.Phase.Four:
                    status.Text = "Faza 3";
                    break;
            }
        }

        public void resize()
        {
            fps.Location = new Vector2(display.Width - 100, 20);
            status.Location = new Vector2(display.Width - 100, 50);
            counter.Top = display.Height - 84;
        }

        public bool Insertion
        {
            set
            {
                insertion = value;

                if (insertion)
                    normal = "Insert";
                else
                    normal = "Status OK";

                status.Text = normal;
            }
        }
    }
}
