using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        List<BalancedNeuron> neurons;
        List<BalancedSynapse> synapses;

        System.Windows.Forms.Timer timer;
        Dictionary<AnimatedNeuron, BalancedNeuron> map;

        float delta;
        float step;
        float treshold;

        int interval;
        int steps;

        bool action;
        bool balance2d;

        static Balancing instance = new Balancing();

        public event EventHandler balanceEnded;
        public event EventHandler balanceState;
        public event EventHandler balanceUpdate;

        #endregion

        private Balancing()
        {
            step = 0.02f;

            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(tick);
            timer.Interval = 25;

            Constant.spaceChanged += new EventHandler(spaceChanged);
        }

        public void animate()
        {
            interval = 0;
            timer.Start();
        }

        public void animate(List<AnimatedNeuron> neurons, List<AnimatedSynapse> synapses, int steps)
        {
            if (action)
                return;

            initialize(neurons, synapses);

            this.steps = steps;
            animate();
        }

        public void balance(List<AnimatedNeuron> neurons, List<AnimatedSynapse> synapses)
        {
            if (action)
                return;

            initialize(neurons, synapses);
            int count = 0;

            while(true)
            {
                calculate();
                update();

                if (Math.Abs(delta) < treshold)
                    break;

                balanceState(delta, null);
                count++;
            }

            balanceEnded(true, null);
            action = false;
        }

        void initialize(List<AnimatedNeuron> neurons, List<AnimatedSynapse> synapses)
        {
            action = true;
            treshold = 0.5f;

            this.neurons = new List<BalancedNeuron>();
            this.synapses = new List<BalancedSynapse>();
            map = new Dictionary<AnimatedNeuron, BalancedNeuron>();

            foreach (AnimatedNeuron an in neurons)
            {
                BalancedNeuron neuron = new BalancedNeuron(an);
                this.neurons.Add(neuron);
                map.Add(an, neuron);
            }

            foreach (AnimatedSynapse synapse in synapses)
                this.synapses.Add(new BalancedSynapse(synapse, map));
        }

        void tick(object sender, EventArgs e)
        {
            /*Thread thread = new Thread(tick);
            thread.Start();*/

            tick();
        }

        void tick()
        {
            if (interval < steps)
                interval += 1;

            for (int i = 0; i < interval; i++)
            {
                foreach (BalancedNeuron neuron in neurons)
                    neuron.zero();

                calculate();
                update();
            }
            
            if (!balance2d && Constant.Space == SpaceMode.Box && Math.Abs(delta) < 3)
            {
                interval = 2;
                steps /= 2;
                balance2d = true;
            }
            
            if (Math.Abs(delta) < treshold)
                finish();
            else
                balanceState(delta, null);

            balanceUpdate(this, null);
        }

        void calculate()
        {
            foreach (BalancedNeuron n1 in neurons)
            {
                n1.repulse();
                //n1.rotate();

                foreach (BalancedNeuron n2 in neurons)
                    if(n1 != n2)
                        n1.repulse(n2.Position, false);
            }

            foreach (BalancedSynapse synapse in synapses)
            {
                synapse.attract();

                if(balance2d)
                    foreach (BalancedNeuron neuron in neurons)
                        synapse.repulse(neuron);
            }

            /*

            foreach (BalancedSynapse bs in synapses)
            {
                bs.rotate();
            }*/
        }

        void update()
        {
            delta = 0;

            foreach (BalancedNeuron neuron in neurons)
                delta += neuron.update(step);
        }

        void finish()
        {
            timer.Stop();
            action = false;
            balanceEnded(false, null);
        }

        public bool stop()
        {
            if (!action)
                return false;

            finish();
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