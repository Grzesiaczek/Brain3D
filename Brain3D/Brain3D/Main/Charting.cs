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

        public Charting()
        {
            neurons = new List<ChartedNeuron>();
        }

        protected override void brainLoaded(object sender, EventArgs e)
        {
            foreach (Neuron neuron in brain.Neurons)
            {
                ChartedNeuron chn = new ChartedNeuron(neuron);
                chn.showNeuron += new EventHandler(show);
                chn.hideNeuron += new EventHandler(hide);

                neurons.Add(chn);
            }
        }

        public void addQuery(QuerySequence query)
        {
            this.query = query;
        }

        public override void show()
        {
            foreach (ChartedNeuron neuron in neurons)
                neuron.show();

            display.change(true);
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