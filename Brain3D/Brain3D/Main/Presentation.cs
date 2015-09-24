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
        protected static QueryContainer container;
        protected static Controller controller;
        protected static Display display;
        protected static Int32 length;

        protected Balancing balancing;

        protected bool visible;
        protected bool paused;

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

            loadBrain += BrainLoaded;
            balancing = Balancing.Instance;
        }

        protected virtual void BrainLoaded(object sender, EventArgs e) { }

        void Timer(object state)
        {
            while (started)
            {
                if (stopwatch.Elapsed.TotalMilliseconds > 20)
                {
                    stopwatch.Restart();
                    Tick();
                }

                Thread.Sleep(2);
            }
        }

        public virtual void ChangeInsertion()
        {
            container.ChangeInsertion();
            controller.Insertion = container.Insertion;
        }

        public virtual void Start()
        {
            started = true;
            paused = false;
            ThreadPool.QueueUserWorkItem(Timer);
        }

        public virtual void Stop()
        {
            if (started)
            {
                started = false;
                paused = true;
                animationStop(this, null);
            }
        }

        protected virtual void Tick() { }

        public virtual void Resize() { }

        public virtual void BalanceSynapses() { }

        #region obsługa zdarzeń myszy

        public virtual void MouseClick(int x, int y) { }

        public virtual void MouseMove(int x, int y)
        {
            if (shifted != null)
            {
                shifted.Move(x, y);
                return;
            }

            Mouse hover = null;

            foreach (Mouse mouse in mouses)
                if (mouse.Cursor(x, y))
                {
                    hover = mouse;
                    break;
                }

            if (active != null && active != hover)
            {
                active.Idle();
                active = null;
            }

            if (hover != null && hover != active)
            {
                hover.Hover();
                active = hover;
            }
        }

        public virtual void MouseDown(int x, int y)
        {
            if (active != null)
            {
                shifted = active;
                shifted.Push(x, y);
            }
        }

        public virtual void MouseUp(int x, int y)
        {
            if (shifted != null)
                shifted = null;
        }

        #endregion

        #region funkcje obsługi kontenera zapytań

        public virtual void Add(char key)
        {
            container.Add(key);
        }

        public virtual void Erase()
        {
            container.Erase();
        }

        #endregion

        #region funkcje sterujące

        public virtual void Back() { }

        public virtual void Forth() { }

        public virtual void Higher() { }

        public virtual void Lower() { }

        public virtual void ChangeFrame(int frame) { }

        public virtual void ChangePace(int pace) { }

        public virtual void Enter() { }

        public virtual void Space() { }

        public virtual void Delete() { }

        public virtual void Show() { }

        public virtual void Hide() { }

        public virtual void Refresh() { }

        public virtual void Left() { }

        public virtual void Right() { }

        public virtual void Up() { }

        public virtual void Down() { }

        public virtual void Closer() { }

        public virtual void Farther() { }

        public virtual void Broaden() { }

        public virtual void Tighten() { }

        public virtual void Center() { }

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

        public static QueryContainer Container
        {
            set
            {
                container = value;
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

        public static int Length
        {
            set
            {
                length = value;
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