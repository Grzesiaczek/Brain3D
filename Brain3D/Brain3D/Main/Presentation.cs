using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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
        protected Timer timer;

        protected bool animation;
        protected bool visible;

        public static event EventHandler loadBrain;

        #endregion

        public Presentation()
        {
            timer = new Timer();
            timer.Tick += new EventHandler(tick);
            timer.Interval = 25;
            loadBrain += brainLoaded;
            balancing = Balancing.Instance;
        }

        protected virtual void brainLoaded(object sender, EventArgs e) { }

        protected virtual void tick(object sender, EventArgs e) { }

        public bool started()
        {
            return animation;
        }

        #region funkcje sterujące

        public virtual void start() { }

        public virtual void stop() { }

        public virtual void forth() { }

        public virtual void back() { }

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
            display.clear();
        }

        #endregion

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
    }
}