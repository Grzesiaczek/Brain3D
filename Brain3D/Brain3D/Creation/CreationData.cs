using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain3D
{
    class CreationData
    {
        #region deklaracje

        CreationFrame frame;
        Synapse synapse;
        Color background;

        float start;
        float finish;
        float change;
        float step;

        #endregion

        #region logika

        public CreationData(Synapse synapse, CreationFrame frame, float start, float finish)
        {
            this.synapse = synapse;
            this.frame = frame;
            this.start = start;
            this.finish = finish;
            change = finish - start;
            /*
            Height = 35;
            Width = 160;
            initializeGraphics();

            MouseEnter += new EventHandler(mouseEnter);
            MouseLeave += new EventHandler(mouseLeave);*/
            background = SystemColors.Control;
        }

        private void mouseEnter(object sender, EventArgs e)
        {
            background = SystemColors.ControlLight;
        }

        private void mouseLeave(object sender, EventArgs e)
        {
            background = SystemColors.Control;
        }

        #endregion

        #region właściwości

        public Synapse Synapse
        {
            get
            {
                return synapse;
            }
        }

        public float Change
        {
            get
            {
                return change;
            }
        }

        public int Frame
        {
            get
            {
                return frame.Frame;
            }
        }

        public float Step
        {
            get
            {
                return step;
            }
            set
            {
                step = value;
            }
        }

        public float Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
            }
        }

        public float Weight
        {
            get
            {
                return finish;
            }
            set
            {
                finish = value;
            }
        }

        #endregion
    }
}
