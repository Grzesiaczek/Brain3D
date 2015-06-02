using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class Balancing
    {
        #region deklaracje

        public enum Phase { Auto, One, Two, Three, Four }
        Phase phase;

        List<BalancedNeuron> neurons;
        List<BalancedSynapse> synapses;
        List<BalancedState> states;

        static Balancing instance = new Balancing();
        Stopwatch sw = new Stopwatch();
        Object locker = new Object();

        float delta;
        float step;
        float treshold;

        int count;
        int interval;
        int steps;

        int updated;
        int updates;
        int overall;

        bool action;
        bool pause;
        bool regular;

        public event EventHandler balanceEnded;
        public event EventHandler balanceState;
        public event EventHandler balanceUpdate;

        #endregion

        private Balancing()
        {
            step = 0.005f;
            Constant.spaceChanged += new EventHandler(spaceChanged);
        }

        public void animate(List<AnimatedNeuron> neurons, List<AnimatedSynapse> synapses, int steps)
        {
            while (pause)
                Thread.Sleep(10);

            interval = 2;
            regular = true;
            
            this.steps = steps;
            initialize(neurons, synapses);
        }

        public void balance(List<CreatedNeuron> neurons, List<CreatedSynapse> synapses)
        {
            while (pause)
                Thread.Sleep(10);

            List<AnimatedNeuron> animatedNeurons = new List<AnimatedNeuron>();
            List<AnimatedSynapse> animatedSynapses = new List<AnimatedSynapse>();

            foreach (CreatedNeuron neuron in neurons)
                animatedNeurons.Add(neuron.Neuron);

            foreach (CreatedSynapse synapse in synapses)
                animatedSynapses.Add(synapse.Synapse);

            interval = 0;
            regular = false;

            initialize(animatedNeurons, animatedSynapses);
            ThreadPool.QueueUserWorkItem(balance);
        }

        void initialize(List<AnimatedNeuron> neurons, List<AnimatedSynapse> synapses)
        {
            this.neurons = new List<BalancedNeuron>();
            this.synapses = new List<BalancedSynapse>();
            states = new List<BalancedState>();

            BalancedNeuron.Map = new Dictionary<AnimatedSynapse, BalancedSynapse>();
            BalancedSynapse.Map = new Dictionary<AnimatedNeuron, BalancedNeuron>();

            foreach (AnimatedNeuron neuron in neurons)
            {
                BalancedNeuron balanced = new BalancedNeuron(neuron);
                BalancedSynapse.Map.Add(neuron, balanced);
                this.neurons.Add(balanced);
            }

            foreach (AnimatedSynapse synapse in synapses)
            {
                BalancedSynapse balanced = new BalancedSynapse(synapse);
                BalancedNeuron.Map.Add(synapse, balanced);

                this.synapses.Add(balanced);
                states.Add(new BalancedState(synapse.State));

                if (synapse.Duplex != null)
                    states.Add(new BalancedState(synapse.Duplex));
            }

            overall = neurons.Count + synapses.Count;
            action = true;
            pause = false;

            count = 0;
            updates = 0;
            treshold = 0.5f;

            if (Constant.Space == SpaceMode.Box)
                phase = Phase.One;
            else
                phase = Phase.Auto;

            BalancedNeuron.K = 24;
            Constant.setBox(phase);

            sw.Start();
            ThreadPool.QueueUserWorkItem(timer);
        }

        void timer(object state)
        {
            while(action)
            {
                if(sw.Elapsed.TotalMilliseconds > 20)
                {
                    sw.Restart();

                    if (regular)
                    {
                        if (phase == Phase.Four)
                            ThreadPool.QueueUserWorkItem(shift);
                        else
                            ThreadPool.QueueUserWorkItem(balance);
                    }
                    else
                        balanceUpdate(this, null);
                }

                Thread.Sleep(2);
            }

            pause = false;
            balanceUpdate(this, null);
        }

        void update()
        {
            delta = 0;

            foreach (BalancedNeuron neuron in neurons)
                delta += neuron.update(step);

            if (phase == Phase.One && Math.Abs(delta) < 3)
            {
                BalancedNeuron.K = 12;
                phase = Phase.Two;

                if(regular)
                {
                    interval = 2;
                    steps /= 2;
                    updates = 0;
                }
            }

            if (phase == Phase.Two)
            {
                if (++count == 200)
                {
                    phase = Phase.Three;
                    Constant.setBox(phase);
                }
                else
                    Constant.setBox((float)count / 200);
            }

            if (Math.Abs(delta) < treshold)
            {
                phase = Phase.Four;
                balanceState(phase, null);
                ThreadPool.QueueUserWorkItem(calculateState);
                return;
            }

            if(++updates == interval)
            {
                if (interval < steps)
                    interval++;

                updates = 0;
                balanceUpdate(this, null);
                return;
            }

            balanceState(phase, null);

            if (action)
                ThreadPool.QueueUserWorkItem(balance);
            else
                pause = false;
        }

        void balance(object state)
        {
            updated = 0;

            foreach (BalancedNeuron neuron in neurons)
                ThreadPool.QueueUserWorkItem(calculateNeuron, neuron);

            foreach (BalancedSynapse synapse in synapses)
                ThreadPool.QueueUserWorkItem(calculateSynapse, synapse);
        }

        void calculateNeuron(object state)
        {
            BalancedNeuron neuron = (BalancedNeuron)state;

            neuron.repulse();
            neuron.rotate();

            foreach (BalancedNeuron other in neurons)
                if (neuron != other)
                    neuron.repulse(other.Position, false);

            Interlocked.Increment(ref updated);

            lock(locker)
            {
                if(updated == overall)
                {
                    updated = 0;
                    update();
                }
            }
        }

        void calculateSynapse(object state)
        {
            BalancedSynapse synapse = (BalancedSynapse)state;

            synapse.attract();

            if (phase != Phase.One)
                foreach (BalancedNeuron neuron in neurons)
                    synapse.repulse(neuron);

            Interlocked.Increment(ref updated);

            lock (locker)
            {
                if (updated == overall)
                {
                    updated = 0;
                    update();
                }
            }
        }

        void calculateState(object State)
        {
            foreach (BalancedNeuron neuron in neurons)
                neuron.rescale();

            foreach (BalancedState state in states)
                state.calculate();
            /*
            foreach (BalancedState state in states)
                state.update();*/

            updated = 0;
        }

        void shift(object State)
        {
            if (++updated == 128)
                finish();

            float scale = (float)updated / 128;

            foreach (BalancedNeuron neuron in neurons)
                neuron.rescale(scale);

            foreach (BalancedState state in states)
                state.update(scale);

            balanceUpdate(this, null);
        }

        void finish()
        {
            sw.Stop();
            action = false;
            balanceEnded(false, null);
        }

        public void stop()
        {
            if (!action)
                return;

            pause = true;
            finish();
        }

        void spaceChanged(object sender, EventArgs e)
        {

        }

        public static Balancing Instance
        {
            get
            {
                return instance;
            }
        }
    }
}