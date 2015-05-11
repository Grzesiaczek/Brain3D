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
        protected static Display display;

        protected Timer timer;

        protected static int frames;
        protected bool animation;

        public static event EventHandler factorChanged;
        public static event EventHandler sizeChanged;
        public static event EventHandler loadBrain;

        #endregion

        public Presentation()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(tick);
            timer.Interval = 25;
            loadBrain += brainLoaded;
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

        #endregion

        public static Brain Brain
        {
            set
            {
                brain = value;
                loadBrain(null, null);
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