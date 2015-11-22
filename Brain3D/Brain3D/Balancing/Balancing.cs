using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

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
        object locker = new object();

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

        public event EventHandler BalanceEnded;
        public event EventHandler BalanceState;
        public event EventHandler BalanceUpdate;

        #endregion

        private Balancing()
        {
            step = 0.010f;
            Constant.SpaceChanged += new EventHandler(SpaceChanged);
        }

        public void Balance(HashSet<AnimatedNeuron> neurons, HashSet<AnimatedVector> vectors, int steps)
        {
            while (pause)
            {
                Thread.Sleep(10);
            }

            interval = 2;
            regular = true;
            
            this.steps = steps;
            Initialize(neurons, vectors);
        }

        public void Balance(HashSet<CreatedNeuron> neurons, HashSet<CreatedVector> vectors)
        {
            while (pause)
            {
                Thread.Sleep(10);
            }

            HashSet<AnimatedNeuron> animatedNeurons = new HashSet<AnimatedNeuron>();
            HashSet<AnimatedVector> animatedVectors = new HashSet<AnimatedVector>();

            foreach (CreatedNeuron neuron in neurons)
            {
                animatedNeurons.Add(neuron.Neuron);
            }

            foreach (CreatedVector vector in vectors)
            {
                animatedVectors.Add(vector.Synapse);
            }

            interval = 0;
            regular = false;

            Initialize(animatedNeurons, animatedVectors);
            ThreadPool.QueueUserWorkItem(Balance);
        }

        public void Balance(HashSet<AnimatedVector> vectors, bool regular = true)
        {
            if (action)
            {
                return;
            }

            this.vectors = new HashSet<BalancedVector>();
            synapses = new HashSet<BalancedSynapse>();

            foreach (AnimatedVector vector in vectors)
            {
                synapses.Add(new BalancedSynapse(vector.State));

                if (vector.Duplex != null)
                {
                    synapses.Add(new BalancedSynapse(vector.Duplex));
                }
            }

            ThreadPool.QueueUserWorkItem(CalculateSynapse);
            this.regular = regular;

            if(regular)
            {
                sw.Start();
                action = true;
                phase = Phase.Four;
                ThreadPool.QueueUserWorkItem(Timer);
            }
        }

        public void Balance(HashSet<CreatedVector> vectors)
        {
            HashSet<AnimatedVector> animatedVectors = new HashSet<AnimatedVector>();

            foreach (CreatedVector vector in vectors)
            {
                animatedVectors.Add(vector.Synapse);
            }

            Balance(animatedVectors, false);
        }

        void Initialize(HashSet<AnimatedNeuron> neurons, HashSet<AnimatedVector> vectors)
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
                {
                    synapses.Add(new BalancedSynapse(vector.Duplex));
                }
            }

            overall = neurons.Count + vectors.Count;
            action = true;
            pause = false;

            count = 0;
            updates = 0;
            treshold = 1;

            if (Constant.Space == SpaceMode.Box)
            {
                phase = Phase.One;
            }
            else
            {
                phase = Phase.Auto;
            }

            BalancedNeuron.K = 32;
            Constant.SetBox(phase);

            sw.Start();
            ThreadPool.QueueUserWorkItem(Timer);
        }

        void Timer(object state)
        {
            while(action)
            {
                if(sw.Elapsed.TotalMilliseconds > 20)
                {
                    sw.Restart();

                    if (regular)
                    {
                        if (phase == Phase.Four)
                        {
                            ThreadPool.QueueUserWorkItem(Shift);
                        }
                        else
                        {
                            ThreadPool.QueueUserWorkItem(Balance);
                        }
                    }
                    else
                    {
                        BalanceUpdate(this, null);
                    }
                }

                Thread.Sleep(2);
            }

            pause = false;
            BalanceUpdate(this, null);
        }

        void Update()
        {
            delta = 0;

            foreach (BalancedNeuron neuron in neurons)
            {
                delta += neuron.Update(step);
            }

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
                {
                    Constant.SetBox((float)count / 200);
                }
            }

            if (Math.Abs(delta) < treshold)
            {
                phase = Phase.Four;
                BalanceState(phase, null);
                ThreadPool.QueueUserWorkItem(CalculateSynapse);
                return;
            }

            if(++updates == interval)
            {
                if (interval < steps)
                {
                    interval++;
                }

                updates = 0;
                BalanceUpdate(this, null);
                return;
            }

            BalanceState(phase, null);

            if (action)
            {
                ThreadPool.QueueUserWorkItem(Balance);
            }
            else
            {
                pause = false;
            }
        }

        void Balance(object state)
        {
            updated = 0;

            foreach (BalancedNeuron neuron in neurons)
            {
                ThreadPool.QueueUserWorkItem(calculateNeuron, neuron);
            }

            foreach (BalancedVector vector in vectors)
            {
                ThreadPool.QueueUserWorkItem(CalculateVector, vector);
            }
        }

        void calculateNeuron(object state)
        {
            BalancedNeuron neuron = (BalancedNeuron)state;

            neuron.Repulse();
            neuron.Rotate();

            foreach (BalancedNeuron other in neurons)
            {
                if (neuron != other)
                {
                    neuron.Repulse(other.Position);
                }
            }

            Interlocked.Increment(ref updated);

            lock(locker)
            {
                if(updated == overall)
                {
                    updated = 0;
                    Update();
                }
            }
        }

        void CalculateVector(object state)
        {
            BalancedVector vector = (BalancedVector)state;

            vector.Attract();

            if (phase != Phase.One)
            {
                foreach (BalancedNeuron neuron in neurons)
                {
                    vector.Repulse(neuron);
                }
            }

            Interlocked.Increment(ref updated);

            lock (locker)
            {
                if (updated == overall)
                {
                    updated = 0;
                    Update();
                }
            }
        }

        void CalculateSynapse(object State)
        {
            int max = 0;

            foreach (BalancedNeuron neuron in neurons)
            {
                if (neuron.Neuron.Neuron.Count > max)
                {
                    max = neuron.Neuron.Neuron.Count;
                }
            }

            foreach (BalancedNeuron neuron in neurons)
            {
                neuron.Calculate(max);
            }

            foreach (BalancedSynapse synapse in synapses)
            {
                synapse.Calculate();
            }

            updated = 0;

            if (regular)
            {
                return;
            }

            foreach (BalancedNeuron neuron in neurons)
            {
                neuron.Rescale();
            }

            foreach (BalancedSynapse synapse in synapses)
            {
                synapse.Update();
            }

            Finish(true);
        }

        void Shift(object State)
        {
            if (++updated == 128)
            {
                Finish(true);
            }

            float scale = (float)updated / 128;

            foreach (BalancedNeuron neuron in neurons)
            {
                neuron.Rescale(scale);
            }

            foreach (BalancedSynapse synapse in synapses)
            {
                synapse.Update(scale);
            }

            BalanceUpdate(this, null);
        }

        void Finish(bool finished = false)
        {
            sw.Stop();
            action = false;
            BalanceUpdate(this, null);
            BalanceEnded(finished, null);
        }

        public void Stop()
        {
            if (action)
            {
                pause = true;
                Finish();
            }
        }

        public void PhaseFour()
        {
            treshold = 1000;
        }

        void SpaceChanged(object sender, EventArgs e)
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