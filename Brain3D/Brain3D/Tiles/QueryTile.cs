
namespace Brain3D
{
    class QueryTile : SequenceTile
    {
        enum Phase { Activate, Disactivate, Shot, Idle }

        Neuron neuron;
        Phase[] activity;

        public QueryTile(string word, int length)
        {
            Prepare(word, length);
        }

        public QueryTile(BuiltTile tile, int length)
        {
            Prepare(tile.Word, length);
            Left = tile.Left;
            Top = tile.Top;
        }

        void Prepare(string word, int length)
        {
            activity = new Phase[length];
            this.word = word;
            Prepare();

            for (int i = 0; i < length; i++)
            {
                activity[i] = Phase.Idle;
            }
        }

        public void Add(Neuron neuron)
        {
            this.neuron = neuron;
            Prepare();   
        }

        public override void Initialize()
        {
            if (neuron == null)
            {
                active = texturesActive;
                normal = texturesNormal;
            }
            else
            {
                active = texturesRefract;
                normal = texturesNeuron;
            }
        }

        public void Tick(int frame)
        {
            switch(activity[frame])
            {
                case Phase.Activate:
                    Activate();
                    break;

                case Phase.Disactivate:
                    Idle();
                    break;

                case Phase.Idle:
                    Idle();
                    break;

                case Phase.Shot:
                    if(neuron == null)
                        texBackground = texturesShoot.Item1;

                    texBorder = texturesShoot.Item2;
                    break;
            }                
        }

        public void Load()
        {
            if (neuron != null)
            {
                for (int i = 0; i < activity.Length; i++)
                {
                    if (neuron.Activity[i].Phase == ActivityPhase.Start)
                    {
                        activity[i] = Phase.Activate;
                    }

                    if (neuron.Activity[i].Phase == ActivityPhase.Finish)
                    {
                        activity[i] = Phase.Disactivate;
                    }
                }
            }
        }

        public void Shot(int time)
        {
            if (neuron != null)
            {
                neuron.GetSimulated().Shot(time);
            }

            activity[time] = Phase.Shot;
            activity[time + 10] = Phase.Disactivate;
        }
    }
}
