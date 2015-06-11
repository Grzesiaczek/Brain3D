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

        QuerySequence added;
        QuerySequence query;

        HashSet<AnimatedNeuron> neurons;
        HashSet<AnimatedVector> vectors;
        HashSet<AnimatedSynapse> synapses;

        bool loaded = false;
        int frame = 0;

        double time = 0;
        double interval = 0.4;

        public event EventHandler balanceFinished;
        public event EventHandler intervalChanged;
        public event EventHandler queryAccepted;

        #endregion

        public Animation()
        {
            balancing.balanceEnded += balanceEnded;
            balancing.balanceUpdate += balanceUpdate;

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

        protected override void brainLoaded(object sender, EventArgs e)
        {
            reload();            
        }

        #endregion

        #region funkcje sterujące

        public override void start()
        {
            if (Started)
                return;

            if (frame == 0)
            {
                frame = 1;
                controller.changeFrame(1);
            }
            else
                time = frame * 10;

            base.start();
            AnimatedNeuron.Animation = true;
        }

        public override void stop()
        {
            if (!Started)
                return;

            base.stop();
            AnimatedNeuron.Animation = false;
        }

        public override void back()
        {
            if (frame > 1)
                frame--;

            changeFrame();
        }

        public override void forth()
        {
            if (frame < length)
                frame++;

            changeFrame();
        }

        void changeFrame()
        {
            foreach (AnimatedNeuron neuron in neurons)
                neuron.setFrame(frame);

            foreach(AnimatedVector vector in vectors)
                vector.setFrame(frame);

            controller.changeFrame(frame);
        }

        public override void changeFrame(int frame)
        {
            this.frame = frame;
            changeFrame();
        }

        public override void changePace(int pace)
        {
            interval = (8 - Math.Log(116 - pace, 2)) / 8;
        }

        #endregion

        #region balansowanie grafu

        public override void balanceSynapses()
        {
            balancing.balance(vectors);
        }

        public void balance()
        {
            balancing.stop();
            balancing.balance(neurons, vectors, 32);
        }

        private void balanceUpdate(object sender, EventArgs e)
        {
            display.move();
        }

        private void balanceEnded(object sender, EventArgs e)
        {
            balanced = true;
            controller.idle();
            balanceFinished(this, new EventArgs());
        }

        #endregion

        public void simulate(int value)
        {
            loaded = true;
            length = value;

            query = new QuerySequence("what is this monkey like", 10 * length + 1);
            query.load(brain);

            brain.initialize();
            brain.simulate(query, value);
        }

        public override void mouseClick(int x, int y)
        {
            if (active == null)
                return;

            if (Started && active is AnimatedNeuron && !active.moved(x, y))
            {
                ((AnimatedNeuron)active).Neuron.shot(time);
                brain.simulate((int)time + 1);
            }
        }

        #region obsługa zapytań

        public override void enter()
        {
            if (!insertion)
            {
                query.execute();
                intervalChanged(this, null);
                return;
            }

            if (added.execute())
            {
                query = added;
                query.load(brain);

                changeInsertion();
                controller.changeFrame(0);
                queryAccepted(this, null);

                time = 0;
                frame = 0;
            }
        }

        public override void space()
        {
            added.space();
        }

        public override void erase()
        {
            added.erase();
        }

        public override void changeInsertion()
        {
            base.changeInsertion();

            if (insertion)
            {
                added = new QuerySequence(10 * length + 1);
                display.show(added);
            }
            else
                display.show(query);
        }

        public override void add(char key)
        {
            if(insertion)
                added.add(key);
        }

        #endregion

        public override void higher()
        {
            if (added == null)
                query.intervalUp();
            else
                added.intervalUp();
        }

        public override void lower()
        {
            if (added == null)
                query.intervalDown();
            else
                added.intervalDown();
        }

        #region takie tam

        public override void show()
        {
            foreach (AnimatedNeuron neuron in neurons)
                neuron.show();

            foreach (AnimatedVector vector in vectors)
            {
                vector.show();
                vector.create();
            }

            display.show(this);
            display.show(query);

            controller.changeState(frame, length);
            controller.show();

            foreach (AnimatedNeuron neuron in neurons)
                neuron.Scale = 1;

            if(!balanced)
                balance();

            insertion = false;
            visible = true;
        }

        protected override void tick()
        {
            if (!loaded || frame == 0)
                return;

            time += interval;
            int step = (int)(time / 10);

            if(step > frame)
            {
                frame = step;
                changeFrame();
            }

            if (frame == length && Started)
            {
                base.stop();
                return;
            }

            foreach (AnimatedNeuron neuron in neurons)
                neuron.tick(time);

            foreach (AnimatedVector vector in vectors)
                vector.tick(time);

            query.tick(time);
        }

        #endregion
    }
}