﻿
namespace Brain3D
{
    class CreatedNeuron : CreatedElement
    {
        AnimatedNeuron neuron;
        int frame;

        public CreatedNeuron(AnimatedNeuron neuron)
        {
            this.neuron = neuron;
            element = neuron;
            created = false;
        }

        public void create()
        {
            created = true;
            Scale = 1;
        }

        public override void Show()
        {
            neuron.Create();
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
        }

        public float Scale
        {
            set
            {
                neuron.Scale = value;
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
