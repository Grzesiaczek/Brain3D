
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
            this.synapse.SetDuplex(synapse);
            duplex = new CreatedSynapse(this.synapse.Duplex);
        }

        public override void Show()
        {
            Scale = 0;
            synapse.Show();
        }

        public void init()
        {
            synapse.Init();
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