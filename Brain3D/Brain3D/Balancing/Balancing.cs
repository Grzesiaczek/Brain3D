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

        enum Phase { Auto, One, Two, Three }
        Phase phase;

        List<BalancedNeuron> neurons;
        List<BalancedSynapse> synapses;

        float delta;
        float step;
        float treshold;

        int count;
        int interval;
        int steps;

        bool action;
        bool pause;
        int counter = 0;

        static Balancing instance = new Balancing();
        Stopwatch sw = new Stopwatch();

        public event EventHandler balanceEnded;
        public event EventHandler balanceState;
        public event EventHandler balanceUpdate;

        #endregion

        private Balancing()
        {
            step = 0.006f;
            Constant.spaceChanged += new EventHandler(spaceChanged);
        }

        public void animate(List<AnimatedNeuron> neurons, List<AnimatedSynapse> synapses, int steps)
        {
            while (pause)
                Thread.Sleep(10);

            initialize(neurons, synapses);
            this.steps = steps;

            interval = 0;
            sw.Start();
            ThreadPool.QueueUserWorkItem(timer);
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

            initialize(animatedNeurons, animatedSynapses);
            ThreadPool.QueueUserWorkItem(balancing);
        }

        void initialize(List<AnimatedNeuron> neurons, List<AnimatedSynapse> synapses)
        {
            action = true;
            treshold = 0.5f;

            BalancedNeuron.K = 20;
            Constant.Box = new Vector3(48, 30, 20);

            this.neurons = new List<BalancedNeuron>();
            this.synapses = new List<BalancedSynapse>();

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
            }

            if (Constant.Space == SpaceMode.Box)
                phase = Phase.One;
            else
                phase = Phase.Auto;
        }

        void timer(object state)
        {
            while(action)
            {
                if(sw.Elapsed.TotalMilliseconds > 20)
                {
                    sw.Restart();
                    tick();
                }

                if(pause)
                {
                    pause = false;
                    break;
                }

                Thread.Sleep(2);
            }
        }

        void tick()
        {
            if (interval < steps)
                interval += 1;

            for (int i = 0; i < interval; i++)
            {
                calculate();
                update();
                counter++;

                if (pause)
                {
                    pause = false;
                    break;
                }
            }
            
            if (phase == Phase.One && Math.Abs(delta) < 3)
            {
                interval = 2;
                steps /= 2;
                phase = Phase.Two;
                BalancedNeuron.K = 3;
            }

            if (phase == Phase.Two)
                squeeze();
            
            if (Math.Abs(delta) < treshold)
                finish();
            else
                balanceState(delta, null);

            balanceUpdate(this, null);
        }

        void balancing(object state)
        {
            balanceUpdate(this, null);

            while (true)
            {
                calculate();
                update();
                counter++;

                if (pause)
                {
                    pause = false;
                    break;
                }

                if (phase == Phase.One && Math.Abs(delta) < 3)
                {
                    BalancedNeuron.K = 3;
                    phase = Phase.Two;
                }

                if (phase == Phase.Two)
                    squeeze();

                if (Math.Abs(delta) < treshold)
                    break;

                balanceState(phase, null);
                balanceUpdate(this, null);
            }

            balanceEnded(true, null);
            balanceUpdate(this, null);
            pause = false;
        }

        void squeeze()
        {
            if (++count == 160)
                phase = Phase.Three;

            Constant.Box -= new Vector3(0, 0, 0.1f);
        }

        void calculate()
        {
            foreach (BalancedNeuron n1 in neurons)
            {
                n1.repulse();
                n1.rotate();

                foreach (BalancedNeuron n2 in neurons)
                    if(n1 != n2)
                        n1.repulse(n2.Position, false);
            }

            foreach (BalancedSynapse synapse in synapses)
            {
                synapse.attract();

                if(phase != Phase.One)
                    foreach (BalancedNeuron neuron in neurons)
                        synapse.repulse(neuron);
            }
        }

        void update()
        {
            delta = 0;

            foreach (BalancedNeuron neuron in neurons)
                delta += neuron.update(step);
        }

        void finish()
        {
            sw.Stop();
            action = false;
            balanceEnded(false, null);
        }

        public bool stop()
        {
            if (pause)
                return false;

            if (action)
            {
                pause = true;
                finish();
            }
            else
                return false;

            return true;
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