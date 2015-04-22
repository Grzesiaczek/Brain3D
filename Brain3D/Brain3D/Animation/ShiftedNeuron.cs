using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain3D
{
    class ShiftedNeuron
    {
        List<AnimatedNeuron> neurons;
        AnimatedNeuron neuron;

        PointF click;
        PointF shift;
        PointF original;

        bool moved;

        public ShiftedNeuron(AnimatedNeuron neuron, PointF click, List<AnimatedNeuron> neurons)
        {
            this.click = click;
            this.neuron = neuron;
            this.neurons = neurons;

            shift = new PointF();
            original = new PointF(neuron.Position.X, neuron.Position.Y);

            neuron.activate(true);
            moved = false;
        }

    }
}
