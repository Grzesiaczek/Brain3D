using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    class QueryTile : SequenceTile
    {
        Neuron neuron;

        public QueryTile(Neuron neuron)
        {
            this.neuron = neuron;
            this.word = neuron.Word;
            prepare();
        }

        public override void initialize()
        {
            active = texturesRefract;
            normal = texturesNeuron;
        }

        public void tick(int frame)
        {
            NeuronData data = neuron.Activity[frame];

            if(data.Treshold)
            {
                if (data.Refraction == 0)
                    activate();
                else
                    idle();
            }
        }
    }
}
