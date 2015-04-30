using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    partial class Charting : Presentation
    {
        List<ChartedNeuron> neurons;
        QuerySequence query;

        public Charting(Display display) : base(display)
        {
            neurons = new List<ChartedNeuron>();
        }

        public void loadBrain(Brain brain)
        {
            foreach (Neuron neuron in brain.Neurons)
            {
                ChartedNeuron chn = new ChartedNeuron(neuron);
                chn.showNeuron += new EventHandler(show);
                chn.hideNeuron += new EventHandler(hide);

                neurons.Add(chn);
                this.brain = brain;
            }
        }

        public void addQuery(QuerySequence query)
        {
            this.query = query;
        }

        void show(object sender, EventArgs e)
        {

        }

        void hide(object sender, EventArgs e)
        {

        }

        public void click(object sender, MouseEventArgs e)
        {
            
        }
    }
}