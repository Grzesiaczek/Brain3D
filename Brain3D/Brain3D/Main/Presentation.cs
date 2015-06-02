using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class Presentation
    {
        #region deklaracje

        protected static Brain brain;
        protected static Controller controller;
        protected static Display display;

        protected Balancing balancing;

        protected bool insertion;
        protected bool visible;

        protected List<Mouse> mouses;

        protected Mouse active;
        protected Mouse shifted;

        Stopwatch stopwatch;
        bool started;

        public event EventHandler animationStop;
        public static event EventHandler loadBrain;

        #endregion

        public Presentation()
        {
            mouses = new List<Mouse>();
            stopwatch = new Stopwatch();
            stopwatch.Start();

            loadBrain += brainLoaded;
            balancing = Balancing.Instance;
        }

        protected virtual void brainLoaded(object sender, EventArgs e) { }

        void timer(object state)
        {
            while (started)
            {
                if (stopwatch.Elapsed.TotalMilliseconds > 20)
                {
                    stopwatch.Restart();
                    tick();
                }

                Thread.Sleep(2);
            }
        }

        public virtual void changeInsertion()
        {
            insertion = !insertion;
            controller.Insertion = insertion;
        }

        public virtual void start()
        {
            started = true;
            ThreadPool.QueueUserWorkItem(timer);
        }

        public virtual void stop()
        {
            if (started)
            {
                started = false;
                animationStop(this, null);
            }
        }

        protected virtual void tick() { }

        public virtual void resize() { }

        #region obsługa zdarzeń myszy

        public virtual void mouseClick(int x, int y)
        {

        }

        public virtual void mouseMove(int x, int y)
        {
            if (shifted != null)
            {
                shifted.move(x, y);
                return;
            }

            Mouse hover = null;

            foreach (Mouse mouse in mouses)
                if (mouse.cursor(x, y))
                {
                    hover = mouse;
                    break;
                }

            if (active != null && active != hover)
            {
                active.idle();
                active = null;
            }

            if (hover != null && hover != active)
            {
                hover.hover();
                active = hover;
            }
        }

        public virtual void mouseDown(int x, int y)
        {
            if (active != null)
            {
                shifted = active;
                shifted.push(x, y);
            }
        }

        public virtual void mouseUp(int x, int y)
        {
            if (shifted != null)
                shifted = null;
        }

        #endregion

        #region funkcje sterujące

        public virtual void back() { }

        public virtual void forth() { }

        public virtual void higher() { }

        public virtual void lower() { }

        public virtual void changeFrame(int frame) { }

        public virtual void changePace(int pace) { }

        public virtual void add(char key) { }

        public virtual void erase() { }

        public virtual void enter() { }

        public virtual void space() { }

        public virtual void delete() { }

        public virtual void show() { }

        public virtual void hide()
        {
            visible = false;
            balancing.stop();
            display.clear();
            stop();
        }

        public virtual void left() { }

        public virtual void right() { }

        public virtual void up() { }

        public virtual void down() { }

        public virtual void closer() { }

        public virtual void farther() { }

        public virtual void broaden() { }

        public virtual void tighten() { }

        #endregion

        #region właściwości

        public static Brain Brain
        {
            set
            {
                brain = value;
                loadBrain(null, null);
            }
        }

        public static Controller Controller
        {
            get
            {
                return controller;
            }
            set
            {
                controller = value;
            }
        }

        public static Display Display
        {
            set
            {
                display = value;
            }
        }

        public bool Started
        {
            get
            {
                return started;
            }
        }

        #endregion
    }
}