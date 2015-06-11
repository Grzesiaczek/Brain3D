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
    class Charting : TimeLine
    {
        List<ChartedNeuron> neurons;

        ChartedNeuron highlight;
        ChartLayout layout;

        Color[] palette = { Color.Olive, Color.BlueViolet, Color.Goldenrod, Color.Green, Color.MediumVioletRed, Color.Peru,
                                     Color.HotPink, Color.Firebrick, Color.Crimson, Color.Indigo, Color.Khaki, Color.Lavender };

        public Charting()
        {
            neurons = new List<ChartedNeuron>();
            layout = new ChartLayout();
        }

        public void load()
        {
            List<Neuron> neurons = new List<Neuron>(brain.Neurons.Keys);

            foreach (Neuron neuron in neurons)
                neuron.calculate();

            neurons.Sort(new Comparer());
            this.neurons.Clear();

            for (int i = 0; i < 8; i++)
                this.neurons.Add(new ChartedNeuron(neurons[i], new Point(100 + 120 * i, display.Height - 160), palette[i]));
        }

        public override void show()
        {
            foreach (ChartedNeuron neuron in neurons)
                neuron.show();

            resize();
            layout.show();
            base.show();
        }

        public override void hide()
        {
            foreach (ChartedNeuron neuron in neurons)
                neuron.hide();

            layout.hide();
            base.hide();
        }

        public override void resize()
        {
            for (int i = 0; i < neurons.Count; i++)
                neurons[i].Top = display.Height - 160;
        }

        protected override void rescale()
        {
            foreach (ChartedNeuron neuron in neurons)
                neuron.Scale = scale;

            layout.Scale = scale;
        }


        #region zdarzenia myszy

        public override void mouseClick(int x, int y)
        {
            foreach(ChartedNeuron neuron in neurons)
                if(neuron.cursor(x, y))
                {
                    neuron.change();
                    return;
                }
        }

        public override void mouseMove(int x, int y)
        {
            ChartedNeuron hover = null;

            foreach (ChartedNeuron neuron in neurons)
                if (neuron.cursor(x, y))
                {
                    hover = neuron;
                    break;
                }

            if (highlight != null && highlight != hover)
            {
                highlight.hover();
                highlight = null;
            }

            if (hover != null && hover != highlight)
            {
                hover.hover();
                highlight = hover;
            }
        }

        #endregion
    }
}