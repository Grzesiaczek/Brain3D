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

        List<AnimatedNeuron> neurons;
        List<AnimatedSynapse> synapses;
        List<AnimatedState> states;

        bool loaded = false;

        int frame = 0;
        int length = 250;

        double time = 0;
        double interval = 0.4;

        public event EventHandler balanceFinished;
        public event EventHandler queryAccepted;

        #endregion

        public Animation()
        {
            balancing.balanceEnded += balanceEnded;
            balancing.balanceUpdate += balanceUpdate;

            neurons = new List<AnimatedNeuron>();
            synapses = new List<AnimatedSynapse>();
            states = new List<AnimatedState>();
        }

        #region funkcje inicjujące

        public void clear()
        {
            balancing.stop();
            neurons.Clear();
            synapses.Clear();
            states.Clear();
            mouses.Clear();
        }

        public void reload()
        {
            foreach (Tuple<AnimatedNeuron, CreatedNeuron> tuple in brain.Neurons.Values)
                neurons.Add(tuple.Item1);

            foreach (Tuple<AnimatedSynapse, CreatedSynapse> tuple in brain.Synapses.Values)
                synapses.Add(tuple.Item1);

            foreach (Tuple<AnimatedState, CreatedState> tuple in brain.States.Values)
                states.Add(tuple.Item1);

            mouses.AddRange(neurons);
            mouses.AddRange(states);

            query = new QuerySequence("what is this monkey like");
            query.load(brain);
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

            foreach(AnimatedSynapse synapse in synapses)
                synapse.setFrame(frame);

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

        public void balance()
        {
            stopBalance();
            balancing.animate(neurons, synapses, 60);
        }

        public void stopBalance()
        {
            balancing.stop();
        }

        private void balanceUpdate(object sender, EventArgs e)
        {
            display.move();
        }

        private void balanceEnded(object sender, EventArgs e)
        {
            controller.idle();
            display.move();
            balanceFinished(this, new EventArgs());
        }

        #endregion

        public void simulate(int value)
        {
            brain.initialize();
            brain.simulate(query, value);

            loaded = true;
            length = value;
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
                return;

            if (added.execute())
            {
                query = added;
                query.load(brain);
                changeInsertion();
                queryAccepted(this, null);
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
                added = new QuerySequence();
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
            query.intervalUp();
        }

        public override void lower()
        {
            query.intervalDown();
        }

        #region takie tam

        public override void show()
        {
            foreach (AnimatedNeuron neuron in neurons)
                neuron.show();

            foreach (AnimatedSynapse synapse in synapses)
            {
                synapse.show();
                synapse.create();
            }

            display.show(this);
            display.show(query);

            controller.changeState(frame, length);
            controller.show();

            foreach (AnimatedNeuron neuron in neurons)
                neuron.Scale = 1;

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

            foreach (AnimatedSynapse synapse in synapses)
                synapse.tick(time);

            query.tick(time);
        }

        #endregion
    }
}