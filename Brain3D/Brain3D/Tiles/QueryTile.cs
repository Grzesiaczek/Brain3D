using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    class QueryTile : SequenceTile
    {
        enum Phase { Activate, Disactivate, Shot, Idle }

        Neuron neuron;
        Phase[] activity;

        public QueryTile(String word, int length)
        {
            prepare(word, length);
        }

        public QueryTile(BuiltTile tile, int length)
        {
            prepare(tile.Word, length);
            Left = tile.Left;
            Top = tile.Top;
        }

        void prepare(String word, int length)
        {
            activity = new Phase[length];
            this.word = word;
            prepare();

            for (int i = 0; i < length; i++)
                activity[i] = Phase.Idle;
        }

        public void add(Neuron neuron)
        {
            this.neuron = neuron;
            prepare();   
        }

        public override void initialize()
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

        public void tick(int frame)
        {
            switch(activity[frame])
            {
                case Phase.Activate:
                    activate();
                    break;
                case Phase.Disactivate:
                    idle();
                    break;
                case Phase.Shot:
                    if(neuron == null)
                        texBackground = texturesShot.Item1;

                    texBorder = texturesShot.Item2;
                    break;
            }                
        }

        public void load()
        {
            if (neuron == null)
                return;

            for (int i = 0; i < activity.Length; i++)
            {
                if (neuron.Activity[i].Phase == ActivityPhase.Start)
                    activity[i] = Phase.Activate;

                if (neuron.Activity[i].Phase == ActivityPhase.Finish)
                    activity[i] = Phase.Disactivate;
            }
        }

        public void shot(int time)
        {
            if (neuron != null)
                neuron.shot(time);

            activity[time] = Phase.Shot;
            activity[time + 10] = Phase.Disactivate;
        }
    }
}
