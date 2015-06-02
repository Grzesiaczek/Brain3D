using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Brain3D
{
    class QuerySequence : Sequence
    {
        List<Neuron> query;
        List<int> activity;

        CounterTile counter;
        CounterTile interval;

        int count;
        int index;
        int treshold;
        int start;

        public QuerySequence()
        {
            builder = new BuiltTile();
            sequence.Add(builder);
            prepare();
        }

        public QuerySequence(String sentence)
        {
            String[] words = sentence.Split(' ');

            foreach (String word in words)
                sequence.Add(new SequenceTile(word));

            prepare();
            arrange();
        }

        void prepare()
        {
            counter = new CounterTile(new Point(1400, 8), 5);
            interval = new CounterTile(new Point(1480, 8), 20);
            Neuron.activate += new EventHandler(activate);
        }

        public void load(Brain brain)
        {
            List<Neuron> neurons = new List<Neuron>(brain.Neurons.Keys);
            query = new List<Neuron>();
            activity = new List<int>();

            for (int i = 0; i < sequence.Count; i++)
            {
                Neuron neuron = neurons.Find(k => k.Word.Equals(sequence[i].Word));

                if (neuron != null)
                {
                    sequence[i] = new QueryTile(neuron);
                    query.Add(neuron);
                }
            }

            arrange();

            start = 50;
            treshold = 2;
            index = 0;
        }

        public void tick(double time)
        {
            int frame = (int)time;

            foreach (SequenceTile tile in sequence)
                if (tile is QueryTile)
                    ((QueryTile)tile).tick(frame);

            counter.Value = activity[frame / 10];
        }

        public void tick(int time)
        {
            if (count >= treshold || query.Count == 0)
            {
                if (time % 10 == 0)
                    activity.Add(0);

                return;
            }

            if (time % 10 != 0)
                return;

            int ticks = interval.Value - ((time - start) / 10) % interval.Value;

            if (ticks > interval.Value)
                ticks -= interval.Value;

            activity.Add(ticks);

            if (ticks == interval.Value)
            {
                query[index].shot(time);

                if (++index == query.Count)
                    index = 0;
            }
        }

        void activate(object sender, EventArgs e)
        {
            Neuron neuron = ((Tuple<Neuron, int>)sender).Item1;

            if(!query.Contains(neuron))
                count++;
        }

        public override void show()
        {
            base.show();
            counter.show();
            interval.show();
        }

        public override void hide()
        {
            base.hide();
            counter.hide();
            interval.hide();
        }

        public void intervalUp()
        {
            if (interval.Value < 40)
                interval.Value++;
        }

        public void intervalDown()
        {
            if (interval.Value > 5)
                interval.Value--;
        }
    }
}
