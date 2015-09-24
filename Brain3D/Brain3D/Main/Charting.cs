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
        Dictionary<QuerySequence, List<ChartedNeuron>> data;
        List<ChartedNeuron> neurons;

        ChartedNeuron highlight;
        ChartLayout layout;

        Color[] palette = { Color.Olive, Color.BlueViolet, Color.Goldenrod, Color.Green, Color.MediumVioletRed, Color.Peru,
                                     Color.HotPink, Color.Firebrick, Color.Crimson, Color.RoyalBlue, Color.Khaki, Color.Lavender };

        public Charting()
        {
            data = new Dictionary<QuerySequence, List<ChartedNeuron>>();
            neurons = new List<ChartedNeuron>();
            layout = new ChartLayout();
        }

        public void Reload()
        {
            foreach (QuerySequence query in container.Data)
            {
                if (data.ContainsKey(query))
                    data[query].Clear();
                else
                    data.Add(query, new List<ChartedNeuron>());

                List<SimulatedNeuron> neurons = new List<SimulatedNeuron>(brain.Neurons.Keys.Select(n => n.GetSimulated(query)));

                foreach (SimulatedNeuron neuron in neurons)
                    neuron.Calculate();

                neurons.Sort(new Comparer());
                int max = neurons.Count;

                if (max > 10)
                    max = 10;

                for (int i = 0; i < max; i++)
                    data[query].Add(new ChartedNeuron(neurons[i], new Point(100 + 120 * i, display.Height - 160), palette[i]));
            }
            
            this.neurons = data[container.Query];
        }

        public override void Show()
        {
            neurons = data[container.Query];

            foreach (ChartedNeuron neuron in neurons)
                neuron.Show();

            Resize();
            layout.Show();
            base.Show();
        }

        public override void Hide()
        {
            foreach (ChartedNeuron neuron in neurons)
                neuron.Hide();

            layout.Hide();
            base.Hide();
        }

        public override void Refresh()
        {
            if (neurons != data[container.Query])
            {
                Hide();
                Show();
            }
        }

        public override void Resize()
        {
            for (int i = 0; i < neurons.Count; i++)
                neurons[i].Top = display.Height - 160;
        }

        protected override void Rescale()
        {
            foreach (ChartedNeuron neuron in neurons)
                neuron.Scale = scale;

            layout.Scale = scale;
        }


        #region zdarzenia myszy

        public override void MouseClick(int x, int y)
        {
            foreach(ChartedNeuron neuron in neurons)
                if(neuron.Cursor(x, y))
                {
                    neuron.Change();
                    return;
                }
        }

        public override void MouseMove(int x, int y)
        {
            ChartedNeuron hover = null;

            foreach (ChartedNeuron neuron in neurons)
                if (neuron.Cursor(x, y))
                {
                    hover = neuron;
                    break;
                }

            if (highlight != null && highlight != hover)
            {
                highlight.Hover();
                highlight = null;
            }

            if (hover != null && hover != highlight)
            {
                hover.Hover();
                highlight = hover;
            }
        }

        #endregion
    }
}