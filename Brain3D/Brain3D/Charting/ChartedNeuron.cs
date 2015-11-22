using Microsoft.Xna.Framework;

namespace Brain3D
{
    class ChartedNeuron : DrawableComposite
    {
        SimulatedNeuron neuron;
        ChartedTile tile;
        Chart chart;

        bool active = false;

        public ChartedNeuron(SimulatedNeuron neuron, Point corner, Color color)
        {
            chart = new Chart(neuron.Neuron, color);
            tile = new ChartedTile(neuron.Neuron.Word, corner, color);

            this.neuron = neuron;
            visible = true;
        }

        public override void Show()
        {
            chart.Show();
            tile.Activate();
            tile.Show();
            visible = true;
        }

        public override void Hide()
        {
            chart.Hide();
            visible = false;
        }

        public void Change()
        {
            if(visible)
            {
                chart.Hide();
                visible = false;
            }
            else
            {
                chart.Show();
                visible = true;
            }
        }

        public void Hover()
        {
            if(active)
            {
                if (visible)
                {
                    tile.Activate();
                }
                else
                {
                    tile.Idle();
                }

                chart.Idle();
                active = false;
            }
            else
            {
                chart.Activate();
                tile.hover();
                active = true;
            }
        }

        public override bool Cursor(int x, int y)
        {
            return tile.Cursor(x, y);
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
