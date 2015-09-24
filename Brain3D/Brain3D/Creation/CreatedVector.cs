using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain3D
{
    class CreatedVector : CreatedElement
    {
        AnimatedVector synapse;

        CreatedSynapse state;
        CreatedSynapse duplex;

        public CreatedVector(AnimatedVector synapse)
        {
            this.synapse = synapse;
            element = synapse;

            state = new CreatedSynapse(synapse.State);
        }

        public void setDuplex(Synapse synapse)
        {
            this.synapse.setDuplex(synapse);
            duplex = new CreatedSynapse(this.synapse.Duplex);
        }

        public override void show()
        {
            Scale = 0;
            synapse.Show();
        }

        public void init()
        {
            synapse.init();
        }

        public void create()
        {
            synapse.Create();
        }

        #region właściwości

        public AnimatedVector Synapse
        {
            get
            {
                return synapse;
            }
        }

        public CreatedSynapse State
        {
            get
            {
                return state;
            }
        }

        public CreatedSynapse Duplex
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