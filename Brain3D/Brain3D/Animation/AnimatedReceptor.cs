using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class AnimatedReceptor : AnimatedElement
    {
        #region deklaracje

        Circle circle;
        Receptor receptor;

        AnimatedNeuron neuron;
        AnimatedSynapse synapse;

        #endregion

        public AnimatedReceptor(Receptor receptor, AnimatedNeuron neuron)
        {
            this.receptor = receptor;
            this.neuron = neuron;

            if(neuron.Position.Length() == 0)
            {
                position = new Vector3(16, 0, 0);
                return;
            }

            Spherical spherical = new Spherical(neuron.Position);
            spherical.Radius = 16;
            position = spherical.getVector();
        }

        #region właściwości

        public List<bool> Activity
        {
            get
            {
                return receptor.Activity;
            }
        }

        public override Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public AnimatedNeuron Neuron
        {
            get
            {
                return neuron;
            }
        }

        public AnimatedSynapse Output
        {
            get
            {
                return synapse;
            }
            set
            {
                synapse = value;
            }
        }

        #endregion
    }
}
