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

        HashSet<BalancedNeuron> neurons;
        HashSet<BalancedVector> vectors;
        HashSet<BalancedSynapse> synapses;

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
            step = 0.010f;
            Constant.spaceChanged += new EventHandler(spaceChanged);
        }

        public void Balance(HashSet<AnimatedNeuron> neurons, HashSet<AnimatedVector> vectors, int steps)
        {
            while (pause)
                Thread.Sleep(10);

            interval = 2;
            regular = true;
            
            this.steps = steps;
            initialize(neurons, vectors);
        }

        public void balance(HashSet<CreatedNeuron> neurons, HashSet<CreatedVector> vectors)
        {
            while (pause)
                Thread.Sleep(10);

            HashSet<AnimatedNeuron> animatedNeurons = new HashSet<AnimatedNeuron>();
            HashSet<AnimatedVector> animatedVectors = new HashSet<AnimatedVector>();

            foreach (CreatedNeuron neuron in neurons)
                animatedNeurons.Add(neuron.Neuron);

            foreach (CreatedVector vector in vectors)
                animatedVectors.Add(vector.Synapse);

            interval = 0;
            regular = false;

            initialize(animatedNeurons, animatedVectors);
            ThreadPool.QueueUserWorkItem(balance);
        }

        public void Balance(HashSet<AnimatedVector> vectors, bool regular = true)
        {
            if (action)
                return;

            this.vectors = new HashSet<BalancedVector>();
            synapses = new HashSet<BalancedSynapse>();

            foreach (AnimatedVector vector in vectors)
            {
                synapses.Add(new BalancedSynapse(vector.State));

                if (vector.Duplex != null)
                    synapses.Add(new BalancedSynapse(vector.Duplex));
            }

            ThreadPool.QueueUserWorkItem(calculateSynapse);
            this.regular = regular;

            if(regular)
            {
                sw.Start();
                action = true;
                phase = Phase.Four;
                ThreadPool.QueueUserWorkItem(timer);
            }
        }

        public void balance(HashSet<CreatedVector> vectors)
        {
            HashSet<AnimatedVector> animatedVectors = new HashSet<AnimatedVector>();

            foreach (CreatedVector vector in vectors)
                animatedVectors.Add(vector.Synapse);

            Balance(animatedVectors, false);
        }

        void initialize(HashSet<AnimatedNeuron> neurons, HashSet<AnimatedVector> vectors)
        {
            this.neurons = new HashSet<BalancedNeuron>();
            this.vectors = new HashSet<BalancedVector>();
            synapses = new HashSet<BalancedSynapse>();

            BalancedNeuron.Map = new Dictionary<AnimatedVector, BalancedVector>();
            BalancedVector.Map = new Dictionary<AnimatedNeuron, BalancedNeuron>();

            foreach (AnimatedNeuron neuron in neurons)
            {
                BalancedNeuron balanced = new BalancedNeuron(neuron);
                BalancedVector.Map.Add(neuron, balanced);
                this.neurons.Add(balanced);
            }

            foreach (AnimatedVector vector in vectors)
            {
                BalancedVector balanced = new BalancedVector(vector);
                BalancedNeuron.Map.Add(vector, balanced);

                this.vectors.Add(balanced);
                synapses.Add(new BalancedSynapse(vector.State));

                if (vector.Duplex != null)
                    synapses.Add(new BalancedSynapse(vector.Duplex));
            }

            overall = neurons.Count + vectors.Count;
            action = true;
            pause = false;

            count = 0;
            updates = 0;
            treshold = 1;

            if (Constant.Space == SpaceMode.Box)
                phase = Phase.One;
            else
                phase = Phase.Auto;

            BalancedNeuron.K = 32;
            Constant.SetBox(phase);

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

            if (phase == Phase.One && Math.Abs(delta) < 5)
            {
                BalancedNeuron.K = 32;
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
                    Constant.SetBox(phase);
                }
                else
                    Constant.SetBox((float)count / 200);
            }

            if (Math.Abs(delta) < treshold)
            {
                phase = Phase.Four;
                balanceState(phase, null);
                ThreadPool.QueueUserWorkItem(calculateSynapse);
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

            foreach (BalancedVector vector in vectors)
                ThreadPool.QueueUserWorkItem(calculateVector, vector);
        }

        void calculateNeuron(object state)
        {
            BalancedNeuron neuron = (BalancedNeuron)state;

            neuron.repulse();
            neuron.rotate();

            foreach (BalancedNeuron other in neurons)
                if (neuron != other)
                    neuron.repulse(other.Position);

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

        void calculateVector(object state)
        {
            BalancedVector vector = (BalancedVector)state;

            vector.attract();

            if (phase != Phase.One)
                foreach (BalancedNeuron neuron in neurons)
                    vector.repulse(neuron);

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

        void calculateSynapse(object State)
        {
            int max = 0;

            foreach (BalancedNeuron neuron in neurons)
                if (neuron.Neuron.Neuron.Count > max)
                    max = neuron.Neuron.Neuron.Count;

            foreach (BalancedNeuron neuron in neurons)
                neuron.calculate(max);

            foreach (BalancedSynapse synapse in synapses)
                synapse.calculate();
            
            updated = 0;

            if (regular)
                return;

            foreach (BalancedNeuron neuron in neurons)
                neuron.rescale();

            foreach (BalancedSynapse synapse in synapses)
                synapse.update();

            finish(true);
        }

        void shift(object State)
        {
            if (++updated == 128)
                finish(true);

            float scale = (float)updated / 128;

            foreach (BalancedNeuron neuron in neurons)
                neuron.rescale(scale);

            foreach (BalancedSynapse synapse in synapses)
                synapse.update(scale);

            balanceUpdate(this, null);
        }

        void finish(bool finished = false)
        {
            sw.Stop();
            action = false;
            balanceUpdate(this, null);
            balanceEnded(finished, null);
        }

        public void Stop()
        {
            if (!action)
                return;

            pause = true;
            finish();
        }

        public void phaseFour()
        {
            treshold = 1000;
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