
namespace Brain3D
{
    class CreatedSynapse : CreatedElement
    {
        AnimatedSynapse synapse;
        CreationHistory history;

        public CreatedSynapse(AnimatedSynapse synapse)
        {
            history = new CreationHistory(synapse);
            this.synapse = synapse;
            element = synapse;
        }

        public void add(CreationData data)
        {
            history.add(data);
        }

        public void setChange(float source, float target)
        {
            synapse.SetChange(source, target);
        }

        public void setFactor(float factor)
        {
            synapse.SetFactor(factor);
        }

        public void setValue(float value)
        {
            synapse.SetValue(value);
        }

        public void historyShow(int x, int y, int frame)
        {
            history.show(x, y, frame);
        }

        public void historyHide()
        {
            history.hide();
        }
    }
}