using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    class Leaf : Tile
    {
        Neuron neuron;
        int time;

        public Leaf(Neuron neuron, int time) : base(neuron.Word)
        {
            this.neuron = neuron;
            this.time = time;
            idle();

            Left = 4 * time;
            //pion do zrobienia
            Top = (int)(100 + ((uint)(DateTime.Now.Ticks * time * time * time)) % 800);
        }

        public Neuron Neuron
        {
            get
            {
                return neuron;
            }
        }
    }
}
