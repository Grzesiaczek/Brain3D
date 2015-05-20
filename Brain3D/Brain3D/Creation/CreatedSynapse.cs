﻿using System;
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

        public CreatedSynapse(AnimatedSynapse synapse)
        {
            this.synapse = synapse;
            element = synapse;
        }

        public void tick(CreationData cd)
        {
            if (synapse.Synapse == cd.Synapse)
                synapse.getState(false).Change += cd.Step;
            else
                synapse.getState(true).Change += cd.Step;
        }

        public override void show()
        {
            synapse.show();
            synapse.create();
        }

        //public override 

        #region właściwości

        public AnimatedSynapse Synapse
        {
            get
            {
                return synapse;
            }
        }

        #endregion;
    }
}