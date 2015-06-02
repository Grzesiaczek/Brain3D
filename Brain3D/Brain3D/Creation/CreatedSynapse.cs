using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain3D
{
    class CreatedSynapse : CreatedElement
    {
        AnimatedSynapse synapse;

        CreatedState state;
        CreatedState duplex;

        public CreatedSynapse(AnimatedSynapse synapse)
        {
            this.synapse = synapse;
            element = synapse;

            state = new CreatedState(synapse.State);
        }

        public void setDuplex(Synapse synapse)
        {
            this.synapse.setDuplex(synapse);
            duplex = new CreatedState(this.synapse.Duplex);
        }

        public override void show()
        {
            Scale = 0;
            synapse.show();
        }

        public void init()
        {
            synapse.init();
        }

        public void create()
        {
            synapse.create();
        }

        #region właściwości

        public AnimatedSynapse Synapse
        {
            get
            {
                return synapse;
            }
        }

        public CreatedState State
        {
            get
            {
                return state;
            }
        }

        public CreatedState Duplex
        {
            get
            {
                return state;
            }
        }

        public float Scale
        {
            set
            {
                synapse.Scale = value;
            }
        }

        #endregion;
    }
}