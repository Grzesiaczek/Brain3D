using System;
using System.Collections.Generic;

namespace Brain3D
{
    class Animation : Graph
    {
        #region deklaracje

        HashSet<AnimatedNeuron> neurons;
        HashSet<AnimatedVector> vectors;
        HashSet<AnimatedSynapse> synapses;

        bool loaded = false;
        int frame = 0;

        double time = 0;
        double interval = 0.4;

        #endregion

        public Animation()
        {
            balancing.BalanceEnded += BalanceEnded;
            balancing.BalanceUpdate += BalanceUpdate;

            neurons = new HashSet<AnimatedNeuron>();
            vectors = new HashSet<AnimatedVector>();
            synapses = new HashSet<AnimatedSynapse>();
        }

        #region funkcje inicjujące

        public void Clear()
        {
            neurons.Clear();
            vectors.Clear();
            synapses.Clear();
            mouses.Clear();
        }

        public void Reload()
        {
            Clear();
            loaded = true;
            balanced = false;

            foreach (Tuple<AnimatedNeuron, CreatedNeuron> tuple in Brain.Neurons.Values)
            {
                neurons.Add(tuple.Item1);
            }

            foreach (Tuple<AnimatedVector, CreatedVector> tuple in Brain.Vectors.Values)
            {
                vectors.Add(tuple.Item1);
            }

            foreach (Tuple<AnimatedSynapse, CreatedSynapse> tuple in Brain.Synapses.Values)
            {
                synapses.Add(tuple.Item1);
            }

            mouses.AddRange(neurons);
            mouses.AddRange(synapses);
        }

        protected override void BrainLoaded(object sender, EventArgs e)
        {
            Reload();            
        }

        #endregion

        #region funkcje sterujące

        public override void Start()
        {
            if (Started)
            {
                return;
            }

            if (frame == 0)
            {
                frame = 1;
                controller.ChangeFrame(1);
            }

            base.Start();
            AnimatedElement.Animation = true;
        }

        public override void Stop()
        {
            if (!Started)
            {
                return;
            }

            base.Stop();
            AnimatedElement.Animation = false;
        }

        public override void Back()
        {
            if (frame > 1)
            {
                ChangeFrame(frame - 1);
            }
        }

        public override void Forth()
        {
            if (frame < length)
            {
                ChangeFrame(frame + 1);
            }
        }

        public override void ChangeFrame(int frame)
        {
            this.frame = frame;
            time = 10 * frame;
            ChangeFrame();
        }

        public override void ChangePace(int pace)
        {
            interval = (8 - Math.Log(116 - pace, 2)) / 8;
        }

        #endregion

        #region balansowanie grafu

        public override void BalanceSynapses()
        {
            balancing.Balance(vectors);
        }

        public void Balance()
        {
            balancing.Stop();
            balancing.Balance(neurons, vectors, 32);
        }

        #endregion

        public override void MouseClick(int x, int y)
        {
            if (active == null)
            {
                return;
            }

            if (Started && active is AnimatedNeuron && !active.Moved(x, y))
            {
                //shot na simulatedneuron
                //((AnimatedNeuron)active).Neuron.shot(time);
                Brain.Simulate((int)time + 1);
            }
        }

        #region obsługa zapytań

        public override void Space()
        {
            if (QueryContainer.Insertion)
            {
                QueryContainer.Space();
            }
            else if (Started)
            {
                Stop();
            }
            else
            {
                Start();
            }
        }

        #endregion

        #region takie tam

        public override void Show()
        {
            foreach (AnimatedNeuron neuron in neurons)
            {
                neuron.Show();
            }

            foreach (AnimatedVector vector in vectors)
            {
                vector.Show();
                vector.Create();
            }
            
            controller.ChangeState(frame, length);
            controller.Show(this);
            display.Show(this);

            foreach (AnimatedNeuron neuron in neurons)
            {
                neuron.Scale = 1;
            }

            if (!balanced)
            {
                Balance();
            }

            QueryContainer.Insertion = false;
            visible = true;
        }

        public override void Hide()
        {
            foreach (AnimatedNeuron neuron in neurons)
            {
                neuron.Hide();
            }

            foreach (AnimatedVector vector in vectors)
            {
                vector.Hide();
            }

            base.Hide();
        }

        public override void Refresh()
        {
            if (!Started)
            {
                ChangeFrame();
            }
        }

        protected override void Tick()
        {
            if (!loaded || frame == 0)
            {
                return;
            }

            time += interval;
            int step = (int)(time / 10);

            if(step > frame)
            {
                frame = step;
                ChangeFrame();
            }

            if (frame == length && Started)
            {
                base.Stop();
                return;
            }

            foreach (AnimatedNeuron neuron in neurons)
            {
                neuron.Tick(time);
            }

            foreach (AnimatedVector vector in vectors)
            {
                vector.Tick(time);
            }

            QueryContainer.Tick(time);
        }

        void ChangeFrame()
        {
            foreach (AnimatedNeuron neuron in neurons)
            {
                neuron.SetFrame(frame);
            }

            foreach (AnimatedVector vector in vectors)
            {
                vector.SetFrame(frame);
            }

            controller.ChangeFrame(frame);
            QueryContainer.SetFrame(frame);
        }

        #endregion
    }
}