using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace Brain3D
{
    class Controller : SpriteComposite
    {
        Text2D brains;
        Text2D queries;

        Text2D fps;
        Text2D status;

        CounterTile counter;
        TrackBar trackBar;

        List<DateTime> times = new List<DateTime>();
        Action<int> updateFrame;

        string brainsPattern = "Brain {0}/{1}";
        string queriesPattern = "Query {0}/{1}";

        string normal;
        bool insertion;

        int count = 0;
        int frame;
        int frames;

        public Controller()
        {
            brains = new Text2D(string.Empty, Fonts.SpriteVerdana, new Vector2(position.X, 20), Color.DarkBlue);
            queries = new Text2D(string.Empty, Fonts.SpriteVerdana, new Vector2(position.X, 50), Color.Purple);

            status = new Text2D("Status OK", Fonts.SpriteVerdana, new Vector2(position.X, 80), Color.DarkOliveGreen);
            fps = new Text2D("FPS: 0", Fonts.SpriteVerdana, new Vector2(position.X, 100), Color.DarkMagenta);
            counter = new CounterTile(new Point(20, display.Height - 80), 0);

            Balancing.Instance.BalanceState += new EventHandler(UpdateState);
            updateFrame = new Action<int>((value) => trackBar.Value = value);

            Presentation.Controller = this;
            normal = "Status OK";
        }

        public override void Draw()
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

            base.Draw();
            count++;
        }

        public void ChangeFrame(int frame)
        {
            this.frame = frame;
            trackBar.Invoke(updateFrame, frame);
            counter.Value = frame;
        }

        public void ChangeState(int frame, int frames)
        {
            this.frames = frames;
            trackBar.Maximum = frames;
            ChangeFrame(frame);
        }

        public void UpdateBrains(int index, int count)
        {
            brains.Text = string.Format(brainsPattern, index, count);
        }

        public void UpdateQueries(int index, int count)
        {
            queries.Text = string.Format(queriesPattern, index, count);
        }

        public void Show(Presentation presentation)
        {
            if(presentation is TimeLine)
            {
                AddSpritesForTimeLine();
            }
            else
            {
                AddAllSprites();
            }

            display.Add(this);
            insertion = false;
        }

        public void Add(TrackBar trackBar)
        {
            this.trackBar = trackBar;
        }

        public void Idle()
        {
            status.Text = normal;
        }

        void AddSpritesForTimeLine()
        {
            sprites.Clear();
            sprites.Add(brains);
            sprites.Add(queries);
            sprites.Add(status);
        }

        void AddAllSprites()
        {
            AddSpritesForTimeLine();
            sprites.Add(fps);
            sprites.Add(counter);
        }

        void UpdateState(object sender, EventArgs e)
        {
            if (!insertion)
            {
                Balancing.Phase phase = (Balancing.Phase)sender;

                switch (phase)
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
        }

        public void Resize()
        {
            brains.Location = new Vector2(display.Width - 100, 20);
            queries.Location = new Vector2(display.Width - 100, 50);

            status.Location = new Vector2(display.Width - 100, 80);
            fps.Location = new Vector2(display.Width - 100, 110);
            
            counter.Top = display.Height - 84;
        }

        public bool Insertion
        {
            set
            {
                insertion = value;

                if (insertion)
                {
                    normal = "Insert";
                }
                else
                {
                    normal = "Status OK";
                }

                status.Text = normal;
            }
        }
    }
}
