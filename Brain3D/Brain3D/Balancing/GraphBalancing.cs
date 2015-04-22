using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class GraphBalancing
    {
        #region deklaracje

        List<BalancedNeuron> neurons;
        List<BalancedReceptor> receptors;
        List<BalancedSynapse> synapses;

        System.Windows.Forms.Timer timer;
        Dictionary<AnimatedElement, BalancedElement> map;

        float delta;
        float step;
        float treshold;

        int interval;
        int steps;

        bool action;
        bool initialized;
        bool screenBalance = true;

        static GraphBalancing instance = new GraphBalancing();

        public event EventHandler balanceEnded;
        public event EventHandler balanceState;
        public event EventHandler balanceUpdate;

        #endregion

        private GraphBalancing()
        {
            step = 0.02f;

            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(tick);
            timer.Interval = 25;
        }

        public void animate()
        {
            interval = 0;
            timer.Start();
        }

        public void animate(List<AnimatedNeuron> neurons, List<AnimatedSynapse> synapses, List<AnimatedReceptor> receptors, int steps)
        {
            if (action)
                return;

            initialize(neurons, synapses, receptors);

            this.steps = steps;
            animate();
        }

        public void balance(List<AnimatedNeuron> neurons, List<AnimatedSynapse> synapses, List<AnimatedReceptor> receptors)
        {
            if (action)
                return;

            initialize(neurons, synapses, receptors);
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

        void initialize(List<AnimatedNeuron> neurons, List<AnimatedSynapse> synapses, List<AnimatedReceptor> receptors)
        {
            action = true;
            treshold = 0.5f;

            this.neurons = new List<BalancedNeuron>();
            this.receptors = new List<BalancedReceptor>();
            this.synapses = new List<BalancedSynapse>();
            map = new Dictionary<AnimatedElement, BalancedElement>();

            foreach (AnimatedNeuron an in neurons)
            {
                BalancedNeuron neuron = new BalancedNeuron(an);
                this.neurons.Add(neuron);
                map.Add(an, neuron);
            }

            foreach (AnimatedReceptor ar in receptors)
            {
                BalancedReceptor receptor = new BalancedReceptor(ar);
                this.receptors.Add(receptor);
                map.Add(ar, receptor);
            }

            foreach (AnimatedSynapse synapse in synapses)
                this.synapses.Add(new BalancedSynapse(synapse, map));
        }

        void tick(object sender, EventArgs e)
        {
            if (interval < steps)
                interval += 1;

            for (int i = 0; i < interval; i++)
            {
                calculate();
                update();
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
                        n1.repulse(n2.Position);

                if (screenBalance)
                {
                    foreach (BalancedNeuron n2 in neurons)
                        if (n1 != n2)
                            n1.repulse(n2.Neuron);
                }
            }

            foreach (BalancedSynapse synapse in synapses)
                synapse.attract();

            /*
            foreach (BalancedReceptor r1 in receptors)
            {
                r1.attract(5 * alpha);

                foreach (BalancedReceptor r2 in receptors)
                {
                    if (r1 == r2)
                        continue;

                    r1.repulse(r2, beta);
                }
            }

            foreach (BalancedSynapse bs in synapses)
            {
                bs.rotate();

                foreach (BalancedNeuron bn in neurons)
                    bs.repulse(bn, beta);
            }*/
        }

        void update()
        {
            delta = 0;

            foreach (BalancedNeuron neuron in neurons)
                delta += neuron.update(step);

            foreach (BalancedReceptor receptor in receptors)
                delta += receptor.update();
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

        public static GraphBalancing Instance
        {
            get
            {
                return instance;
            }
        }

        public bool ScreenBalance
        {
            set
            {
                screenBalance = value;
                animate();
            }
        }
    }
}