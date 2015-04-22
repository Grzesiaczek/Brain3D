using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain3D
{
    class CreatedNeuron : CreatedElement
    {
        AnimatedNeuron neuron;

        bool created;
        int frame;

        public CreatedNeuron(AnimatedNeuron neuron)
        {
            this.neuron = neuron;
            element = neuron;
            created = false;
        }

        #region właściwości

        public AnimatedNeuron Neuron
        {
            get
            {
                return neuron;
            }
        }

        public bool Created
        {
            get
            {
                return created;
            }
            set
            {
                created = value;
            }
        }

        public int Frame
        {
            get
            {
                return frame;
            }
            set
            {
                frame = value;
            }
        }

        #endregion
    }
}
