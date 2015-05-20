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
    class Controller : SpriteElement
    {
        Text2D state;
        Text2D fps;
        Text2D status;

        Vector2 location;
        TrackBar trackBar;
        Display display;

        List<DateTime> times = new List<DateTime>();

        int count = 0;
        int frame;
        int frames;

        public Controller(Display display)
        {
            SpriteFont font = content.Load<SpriteFont>("Sequence");

            state = new Text2D("", font, new Vector2(location.X, 20), Color.DarkBlue, 40);
            fps = new Text2D("FPS: 0", font, new Vector2(location.X, 50), Color.DarkMagenta);
            status = new Text2D("Waiting...", font, new Vector2(location.X, 80), Color.DarkOliveGreen);

            Presentation.Controller = this;
            this.display = display;
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

            state.draw();
            fps.draw();
            status.draw();

            count++;
        }

        public void changeFrame(int frame)
        {
            this.frame = frame;
            trackBar.Value = frame;
            state.Text = frame.ToString() + "/" + frames;
            display.repaint();
        }

        public void changeState(int frame, int frames)
        {
            this.frames = frames;
            trackBar.Maximum = frames;
            changeFrame(frame);
        }

        public void add(TrackBar trackBar)
        {
            this.trackBar = trackBar;
        }

        public void balance()
        {
            status.Text = "Balancing";
        }

        public void idle()
        {
            status.Text = "Status OK";
        }

        public Vector2 Location
        {
            set
            {
                location = value;
                state.Location = new Vector2(location.X, 20);
                fps.Location = new Vector2(location.X, 50);
                status.Location = new Vector2(location.X, 80);
            }
        }
    }
}
