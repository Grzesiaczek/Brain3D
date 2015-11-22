using System;
using System.Collections.Generic;
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

        bool parallel;

        #endregion

        public QuerySequence(int length)
        {
            builder = new BuiltTile();
            sequence.Add(builder);
            this.length = length;
            Prepare();
        }

        public QuerySequence(string sentence, int length)
        {
            string[] words = sentence.Split(' ');
            this.length = length;

            foreach (string word in words)
            {
                sequence.Add(new QueryTile(word, length));
            }

            Prepare();
        }

        void Prepare()
        {
            counter = new CounterTile(new Point(20, 8), 0);
            interval = new CounterTile(new Point(100, 8), 20);

            parallel = false;
            Arrange();
        }

        protected override void Arrange()
        {
            int position = 200;

            foreach (Tile element in sequence)
            {
                element.Top = 8;
                element.Left = position;
                position = element.Right + 10;
            }
        }

        public void Initialize()
        {
            activity.Clear();
            counter.Value = 5;
            start = counter.Value * 10;

            treshold = 2;
            count = 0;
            index = 0;
            prev = -1;
        }

        public void Load(Brain brain)
        {
            List<Neuron> neurons = new List<Neuron>(brain.Neurons.Keys);
            activity = new List<int>();
            activations = new List<Tuple<Neuron, int>>();

            for (int i = 0; i < sequence.Count; i++)
            {
                Neuron neuron = neurons.Find(k => k.Word.Equals(sequence[i].Word));

                if (neuron != null)
                {
                    ((QueryTile)sequence[i]).Add(neuron);
                    treshold++;
                }
            }

            Arrange();
            Initialize();
            brain.Initialize(this);
        }

        public void LoadTiles()
        {
            foreach (QueryTile tile in sequence)
            {
                tile.Load();
            }
        }

        public void SetFrame(int frame)
        {
            int time = 10 * frame;

            foreach (QueryTile tile in sequence)
            {
                tile.Tick(time);
            }

            counter.Value = activity[frame];

            if (counter.Value == 0)
            {
                counter.Activate();
            }
            else
            {
                counter.Idle();
            }
        }

        public void Tick(double time)
        {
            int frame = (int)time;

            foreach (QueryTile tile in sequence)
            {
                tile.Tick(frame);
            }

            counter.Value = activity[frame / 10];

            if (counter.Value == 0)
            {
                counter.Activate();
            }
            else
            {
                counter.Idle();
            }
        }

        public void Tick(int time)
        {
            if (count > treshold)
            {
                if (time % 10 == 0)
                {
                    activity.Add(-1);
                }

                return;
            }

            if (time % 10 != 0)
            {
                return;
            }

            int ticks = interval.Value - ((time - start) / 10) % interval.Value;

            if (ticks > interval.Value)
            {
                ticks -= interval.Value;
            }

            if (ticks == interval.Value)
            {
                if (parallel)
                {
                    foreach(QueryTile tile in sequence)
                    {
                        tile.Shot(time);
                    }
                }
                else
                {
                    ((QueryTile)sequence[index]).Shot(time);
                    prev = index;

                    if (++index == sequence.Count)
                    {
                        index = 0;
                        count = 0;
                    }
                }

                activity.Add(0);
            }
            else
            {
                activity.Add(ticks);
            }
        }

        public void Activate(object sender, EventArgs e)
        {
            Tuple<Neuron, int> tuple = (Tuple<Neuron, int>)sender;
            activations.Add(tuple);
        }

        protected override Tile CreateTile(BuiltTile builder)
        {
            return new QueryTile(builder, length);
        }

        #region sterowanie

        public override bool Execute()
        {
            interval.Idle();

            if (builder == null)
            {
                return true;
            }

            return base.Execute();
        }

        public override void Show()
        {
            base.Show();
            counter.Show();
            interval.Show();
        }

        public override void Hide()
        {
            base.Hide();
            counter.Hide();
            interval.Hide();
        }

        public void Switch()
        {
            if (parallel)
            {
                parallel = false;
                interval.Switch();
            }
            else
            {
                parallel = true;
                interval.Switch();
            }
        }

        public void IntervalUp()
        {
            if (interval.Value < 100)
            {
                interval.Value++;
            }

            interval.Activate();
        }

        public void IntervalDown()
        {
            if (interval.Value > 1)
            {
                interval.Value--;
            }

            interval.Activate();
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
