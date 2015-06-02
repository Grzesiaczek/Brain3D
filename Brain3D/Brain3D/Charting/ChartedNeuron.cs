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
    class ChartedNeuron : DrawableComposite
    {
        Neuron neuron;
        ChartedTile tile;
        Chart chart;

        bool active = false;

        public ChartedNeuron(Neuron neuron, Point corner, Color color)
        {
            chart = new Chart(neuron, color);
            tile = new ChartedTile(neuron.Word, corner, color);

            this.neuron = neuron;
            visible = true;
        }

        public override void show()
        {
            chart.show();
            tile.activate();
            tile.show();
            visible = true;
        }

        public override void hide()
        {
            chart.hide();
            visible = false;
        }

        public void change()
        {
            if(visible)
            {
                chart.hide();
                visible = false;
            }
            else
            {
                chart.show();
                visible = true;
            }
        }

        public void hover()
        {
            if(active)
            {
                if (visible)
                    tile.activate();
                else
                    tile.idle();

                chart.idle();
                active = false;
            }
            else
            {
                chart.activate();
                tile.hover();
                active = true;
            }
        }

        public override bool cursor(int x, int y)
        {
            return tile.cursor(x, y);
        }

        public int Top
        {
            set
            {
                tile.Top = value;
            }
        }

        public override float Scale
        {
            set
            {
                chart.Scale = value;
            }
        }
    }
}
