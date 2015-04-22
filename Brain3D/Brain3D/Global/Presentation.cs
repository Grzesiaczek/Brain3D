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

        protected Display display;
        protected ShiftedNeuron shift;

        protected Brain brain;
        protected Timer timer;

        protected static int frames;
        protected bool animation;

        public static event EventHandler factorChanged;
        public static event EventHandler sizeChanged;

        #endregion

        public Presentation(Display display)
        {
            this.display = display;

            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(tick);
            timer.Interval = 25;
        }

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

        #endregion
    }
}