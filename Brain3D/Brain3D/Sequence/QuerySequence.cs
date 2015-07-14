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
        #region deklaracje

        List<Tuple<Neuron, int>> activations;
        List<int> activity;

        CounterTile counter;
        CounterTile interval;

        int count;
        int treshold;
        int start;
        int length;

        int index;
        int prev;

        #endregion

        public QuerySequence(int length)
        {
            builder = new BuiltTile();
            sequence.Add(builder);
            this.length = length;
            prepare();
        }

        public QuerySequence(String sentence, int length)
        {
            String[] words = sentence.Split(' ');
            this.length = length;

            foreach (String word in words)
                sequence.Add(new QueryTile(word, length));

            prepare();
        }

        void prepare()
        {
            counter = new CounterTile(new Point(20, 8), 0);
            interval = new CounterTile(new Point(100, 8), 15);

            Neuron.activate += new EventHandler(activate);
            arrange();
        }

        protected override void arrange()
        {
            int position = 200;

            foreach (Tile element in sequence)
            {
                element.Top = 8;
                element.Left = position;
                position = element.Right + 10;
            }
        }

        public void initialize()
        {
            activity.Clear();
            counter.Value = 5;
            start = counter.Value * 10;

            treshold = 2;
            count = 0;
            index = 0;
            prev = -1;
        }

        public void load(Brain brain)
        {
            List<Neuron> neurons = new List<Neuron>(brain.Neurons.Keys);
            activity = new List<int>();
            activations = new List<Tuple<Neuron, int>>();

            for (int i = 0; i < sequence.Count; i++)
            {
                Neuron neuron = neurons.Find(k => k.Word.Equals(sequence[i].Word));

                if (neuron != null)
                {
                    ((QueryTile)sequence[i]).add(neuron);
                    treshold++;
                }
            }

            arrange();
            initialize();
        }

        public void loadTiles()
        {
            foreach (QueryTile tile in sequence)
                tile.load();
        }

        public void setFrame(int frame)
        {
            int time = 10 * frame;

            foreach (QueryTile tile in sequence)
                tile.tick(time);

            counter.Value = activity[frame];

            if (counter.Value == 0)
                counter.activate();
            else
                counter.idle();
        }

        public void tick(double time)
        {
            int frame = (int)time;

            foreach (QueryTile tile in sequence)
                tile.tick(frame);

            counter.Value = activity[frame / 10];

            if (counter.Value == 0)
                counter.activate();
            else
                counter.idle();
        }

        public void tick(int time)
        {
            if (count > treshold)
            {
                if (time % 10 == 0)
                    activity.Add(-1);

                return;
            }

            if (time % 10 != 0)
                return;

            int ticks = interval.Value - ((time - start) / 10) % interval.Value;

            if (ticks > interval.Value)
                ticks -= interval.Value;

            if (ticks == interval.Value)
            {
                ((QueryTile)sequence[index]).shot(time);
                activity.Add(0);

                prev = index;

                if (++index == sequence.Count)
                {
                    index = 0;
                    count = 0;
                }
            }
            else
                activity.Add(ticks);
        }

        void activate(object sender, EventArgs e)
        {
            Tuple<Neuron, int> tuple = (Tuple<Neuron, int>)sender;
            Neuron neuron = tuple.Item1;
            count++;

            if (sequence.Find(k => k.Word == neuron.Word) != null)
                activations.Add(tuple);
        }

        protected override Tile createTile(BuiltTile builder)
        {
            return new QueryTile(builder, length);
        }

        #region sterowanie

        public override bool execute()
        {
            interval.idle();

            if (builder == null)
                return true;

            return base.execute();
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

            interval.activate();
        }

        public void intervalDown()
        {
            if (interval.Value > 5)
                interval.Value--;

            interval.activate();
        }

        #endregion

        #region właściwości

        public List<Tuple<Neuron, int>> Activations
        {
            get
            {
                return activations;
            }
        }

        public int Interval
        {
            get
            {
                return interval.Value * sequence.Count;
            }
        }

        #endregion
    }
}
