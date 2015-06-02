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
    class Charting : Presentation
    {
        List<ChartedNeuron> neurons;

        ChartedNeuron active;
        ChartLayout layout;

        Color[] palette = { Color.Olive, Color.BlueViolet, Color.Goldenrod, Color.Green, Color.MediumVioletRed, Color.Peru,
                                     Color.HotPink, Color.Firebrick, Color.Crimson, Color.Indigo, Color.Khaki, Color.Lavender };

        float scale;
        int frame;

        public Charting()
        {
            neurons = new List<ChartedNeuron>();
            layout = new ChartLayout();
            frame = 30;
            scale = 1;
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

            layout.show();
            display.show(this);

            resize();
            controller.changeFrame(frame);
        }

        public override void resize()
        {
            for (int i = 0; i < neurons.Count; i++)
                neurons[i].Top = display.Height - 160;
        }

        #region sterowanie widokiem

        public override void left()
        {
            back();
        }

        public override void right()
        {
            forth();
        }

        public override void back()
        {
            if (frame > 0)
                changeFrame(--frame);

            controller.changeFrame(frame);
        }

        public override void forth()
        {
            changeFrame(++frame);

            controller.changeFrame(frame);
        }

        public override void broaden()
        {
            if (scale >= 2)
                return;

            scale += 0.1f;
            rescale();
        }

        public override void tighten()
        {
            if (scale <= 0.4f)
                return;

            scale -= 0.1f;
            rescale();
        }

        void rescale()
        {
            foreach (ChartedNeuron neuron in neurons)
                neuron.Scale = scale;

            layout.Scale = scale;
        }

        public override void changeFrame(int frame)
        {
            display.moveX(0.1f * frame);
            this.frame = frame;
        }

        #endregion

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

            if (active != null && active != hover)
            {
                active.hover();
                active = null;
            }

            if (hover != null && hover != active)
            {
                hover.hover();
                active = hover;
            }
        }

        #endregion
    }
}