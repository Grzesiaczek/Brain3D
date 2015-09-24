using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public event EventHandler BalanceFinished;
        public event EventHandler IntervalChanged;
        public event EventHandler queryAccepted;

        #endregion

        public Animation()
        {
            balancing.balanceEnded += BalanceEnded;
            balancing.balanceUpdate += BalanceUpdate;

            neurons = new HashSet<AnimatedNeuron>();
            vectors = new HashSet<AnimatedVector>();
            synapses = new HashSet<AnimatedSynapse>();
        }

        #region funkcje inicjujące

        public void clear()
        {
            neurons.Clear();
            vectors.Clear();
            synapses.Clear();
            mouses.Clear();
        }

        public void reload()
        {
            clear();
            loaded = true;
            balanced = false;

            foreach (Tuple<AnimatedNeuron, CreatedNeuron> tuple in brain.Neurons.Values)
                neurons.Add(tuple.Item1);

            foreach (Tuple<AnimatedVector, CreatedVector> tuple in brain.Vectors.Values)
                vectors.Add(tuple.Item1);

            foreach (Tuple<AnimatedSynapse, CreatedSynapse> tuple in brain.Synapses.Values)
                synapses.Add(tuple.Item1);

            mouses.AddRange(neurons);
            mouses.AddRange(synapses);
        }

        protected override void BrainLoaded(object sender, EventArgs e)
        {
            reload();            
        }

        #endregion

        #region funkcje sterujące

        public override void Start()
        {
            if (Started)
                return;

            if (frame == 0)
            {
                frame = 1;
                controller.ChangeFrame(1);
            }

            base.Start();
            AnimatedNeuron.Animation = true;
        }

        public override void Stop()
        {
            if (!Started)
                return;

            base.Stop();
            AnimatedNeuron.Animation = false;
        }

        public override void Back()
        {
            if (frame > 1)
                ChangeFrame(frame - 1);
        }

        public override void Forth()
        {
            if (frame < length)
                ChangeFrame(frame + 1);
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

        private void BalanceUpdate(object sender, EventArgs e)
        {
            display.Moved();
        }

        private void BalanceEnded(object sender, EventArgs e)
        {
            balanced = true;
            controller.Idle();
            BalanceFinished(this, new EventArgs());
        }

        #endregion

        public override void MouseClick(int x, int y)
        {
            if (active == null)
                return;

            if (Started && active is AnimatedNeuron && !active.Moved(x, y))
            {
                //shot na simulatedneuron
                //((AnimatedNeuron)active).Neuron.shot(time);
                brain.Simulate((int)time + 1);
            }
        }

        #region obsługa zapytań

        public override void Enter()
        {
            if (!container.Insertion)
            {
                container.Execute();
                ChangeFrame(0);
                IntervalChanged(this, null);
                return;
            }

            if (container.Execute())
            {
                //query = added;
                //query.Load(brain);

                ChangeInsertion();
                controller.ChangeFrame(0);
                //queryAccepted(null, null);
                ChangeFrame(0);
            }
        }

        public override void Space()
        {
            if(container.Insertion)
                container.Space();
            else if (Started)
                Stop();
            else
                Start();
        }

        #endregion

        #region takie tam

        public override void Show()
        {
            foreach (AnimatedNeuron neuron in neurons)
                neuron.Show();

            foreach (AnimatedVector vector in vectors)
            {
                vector.Show();
                vector.Create();
            }

            display.Show(this);

            controller.ChangeState(frame, length);
            controller.Show();

            foreach (AnimatedNeuron neuron in neurons)
                neuron.Scale = 1;

            if(!balanced)
                Balance();

            container.Insertion = false;
            visible = true;
        }

        public override void Refresh()
        {
            if (!Started)
                ChangeFrame();
        }

        protected override void Tick()
        {
            if (!loaded || frame == 0)
                return;

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
                neuron.Tick(time);

            foreach (AnimatedVector vector in vectors)
                vector.Tick(time);

            container.Tick(time);
        }

        void ChangeFrame()
        {
            foreach (AnimatedNeuron neuron in neurons)
                neuron.SetFrame(frame);

            foreach (AnimatedVector vector in vectors)
                vector.SetFrame(frame);

            controller.ChangeFrame(frame);
            container.SetFrame(frame);
        }

        #endregion
    }
}