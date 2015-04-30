using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class ChartedNeuron : CompositeElement
    {
        Neuron neuron;
        Chart chart;

        Color background;
        Color light;

        static Color[] palette = { Color.Red, Color.Purple, Color.Orchid, Color.BlueViolet, Color.Goldenrod, Color.DarkGreen, Color.Maroon, Color.Navy,
                                     Color.HotPink, Color.Firebrick, Color.Crimson, Color.Indigo, Color.Khaki, Color.Lavender};
        static int index = 0;
        bool visible;

        public ChartedNeuron(Neuron neuron)
        {
            this.neuron = neuron;
            visible = false;
        }

        void click(object sender, EventArgs e)
        {
            
        }

        void mouseOn(object sender, EventArgs e)
        {
            
        }

        void mouseOff(object sender, EventArgs e)
        {
            
        }

        public event EventHandler showNeuron;
        public event EventHandler hideNeuron;
    }
}
