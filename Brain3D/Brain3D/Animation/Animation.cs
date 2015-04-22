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
    class Animation : Presentation
    {
        #region deklaracje

        Mode mode;
        QuerySequence query;

        GraphBalancing balancing;
        StateBar stateBar;

        List<AnimatedNeuron> neurons;
        List<AnimatedSynapse> synapses;
        List<AnimatedReceptor> receptors;

        bool activation = false;
        bool loaded = false;
        bool pause = false;

        int frame = 0;
        int length = 250;

        double time = 0;
        double interval = 0.4;

        public event EventHandler animationStop;
        public event EventHandler balanceFinished;
        public event EventHandler queryAccepted;
        public event EventHandler frameChanged;
        public event EventHandler framesChanged;

        #endregion

        public Animation(Display display) : base(display)
        {
            balancing = GraphBalancing.Instance;
            balancing.balanceEnded += balanceEnded;
            balancing.balanceState += balanceState;
            balancing.balanceUpdate += balanceUpdate;

            query = new QuerySequence();
            display.add(this);

            neurons = new List<AnimatedNeuron>();
            synapses = new List<AnimatedSynapse>();
            receptors = new List<AnimatedReceptor>();
        }

        #region funkcje inicjujące

        Vector3 randomPoint(Random random, int radius)
        {
            Vector3 vector;

            while(true)
            {
                float x = random.Next(-radius, radius);
                float y = random.Next(-radius, radius);
                float z = random.Next(-radius, radius);

                vector = new Vector3(x, y, z);

                if (vector.Length() < radius)
                    break;
            }

            return vector;
        }

        public void clear()
        {
            neurons.Clear();
            receptors.Clear();
            synapses.Clear();
            display.clear();
        }

        public void loadBrain(Brain brain)
        {
            Random random = new Random();
            this.brain = brain;

            foreach (Neuron neuron in brain.Neurons)
            {
                Vector3 position = randomPoint(random, 16);
                neurons.Add(new AnimatedNeuron(neuron, position));
            }

            foreach (AnimatedNeuron pre in neurons)
                foreach (Synapse synapse in pre.Neuron.Output)
                    foreach (AnimatedNeuron post in neurons)
                        if (post.Neuron == synapse.Post)
                        {
                            AnimatedSynapse syn = synapses.Find(k => pre == k.Pre && post == k.Post);

                            if (syn == null)
                                synapses.Add(new AnimatedSynapse(pre, post, synapse));
                            else
                                syn.setDuplex(synapse);

                            break;
                        }

            //inicjalizacja zapytania
            /*String[] data = { brain.Neurons[0].Word, brain.Neurons[1].Word };
            addQuery(data, 4, 0.6f);
            queryAccepted(query, null);*/
        }

        public Dictionary<object, object> loadFrame(CreationFrame frame, int index)
        {
            Dictionary<object, object> result = new Dictionary<object, object>();
            Random random = new Random();

            Neuron neuron = frame.Neuron.Neuron;

            if (neurons.Find(k => k.Neuron == neuron) == null)
            {
                Vector3 position = randomPoint(random, 16);
                AnimatedNeuron an = new AnimatedNeuron(neuron, position);
                CreatedNeuron cn = new CreatedNeuron(an);

                neurons.Add(an);
                result.Add(neuron, cn);
            }
            else
                result.Add(neuron, true);

            foreach (CreationData data in frame.Data)
                if (data.Synapse.Changes.First<CreationData>() == data)
                {
                    AnimatedNeuron pre = neurons.Find(k => data.Synapse.Pre == k.Neuron);
                    AnimatedNeuron post = neurons.Find(k => data.Synapse.Post == k.Neuron);
                    AnimatedSynapse synapse = synapses.Find(k => pre == k.Pre && post == k.Post);

                    if (synapse == null)
                    {
                        AnimatedSynapse syn = new AnimatedSynapse(pre, post, data.Synapse);
                        CreatedSynapse cs = new CreatedSynapse(syn);

                        synapses.Add(syn);
                        result.Add(syn.Synapse, cs);

                        if (syn.isDuplex())
                            result.Add(syn.Duplex, cs);
                    }
                    else
                        synapse.setDuplex(data.Synapse);

                }

            return result;
        }

        public void create()
        {
            display.clear();

            foreach (AnimatedNeuron neuron in neurons)
                display.add(neuron);

            foreach (AnimatedSynapse synapse in synapses)
            {
                display.add(synapse);
                synapse.create();
            }

            foreach (AnimatedReceptor receptor in receptors)
                display.add(receptor);
        }

        public void create(Creation creation)
        {
            creation.load(neurons, synapses, brain);
            
            Thread thread = new Thread(fastBalancing);
            thread.Start();
        }

        void fastBalancing()
        {
            stateBar.Phase = StateBarPhase.BalanceNormal;
            balancing.balance(neurons, synapses, receptors);
        }

        public void setBar(StateBar stateBar)
        {
            this.stateBar = stateBar;
        }

        #endregion

        #region funkcje sterujące

        public override void start()
        {
            if (animation)
                return;

            if (frame == 0)
            {
                frame = 1;
                frameChanged(frame, null);
            }

            animation = true;
            timer.Start();
            AnimatedNeuron.Animation = true;
        }

        public override void stop()
        {
            if (!animation)
                return;

            timer.Stop();
            animation = false;
            animationStop(this, null);
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

            query.tick(frame);
            frameChanged(frame, null);
        }

        public override void changeFrame(int frame)
        {
            this.frame = frame;
            changeFrame();
        }

        public override void changePace(int pace)
        {
            //count = (int)(count * (float)pace / (interval * 25));
            //interval = pace / 25;
            //arrival = (interval * 3) / 4;
        }

        #endregion

        #region balansowanie grafu

        public void balance()
        {
            stopBalance();
            stateBar.Phase = StateBarPhase.BalanceNormal;
            balancing.animate(neurons, synapses, receptors, 40);
        }

        public void stopBalance()
        {
            balancing.stop();
        }

        private void balanceUpdate(object sender, EventArgs e)
        {
            display.move();
        }

        private void balanceState(object sender, EventArgs e)
        {
            double result = Math.Log10((float)sender);
            int value = (int)Math.Min(stateBar.Height, result * 40);
            stateBar.State = value;
        }

        private void balanceEnded(object sender, EventArgs e)
        {
            stateBar.reset();
            balanceFinished(this, new EventArgs());
        }

        #endregion

        #region różne

        public void labelChanged(bool value)
        {
            foreach (AnimatedNeuron neuron in neurons)
                neuron.Label = value;
        }

        public void stateChanged(bool value)
        {
            foreach (AnimatedNeuron neuron in neurons)
                neuron.State = value;
        }

        public void load(int value)
        {
            loaded = true;
            length = value;
        }

        public void unload()
        {
            loaded = false;
        }

        public void setMode(Mode mode)
        {
            this.mode = mode;

            if (mode == Mode.Manual && frame == 0)
            {
                frame = 1;
                changeFrame();
            }
        }

        #endregion

        #region obsługa zapytań

        public void addQuery(String[] words, int interval, float intensivity)
        {
            frame = 0;

            frameChanged(frame, null);
            query.clear();

            foreach (String word in words)
            {
                AnimatedNeuron an = neurons.Find(k => k.Name == word);

                if (an == null)
                    continue;

                Receptor receptor = brain.Receptors.Find(k => k.Name == word);
                Synapse synapse = brain.Synapses.Find(k => k.Pre == receptor);

                receptor.initialize(interval, 0, intensivity);
                new SequenceReceptor(query, receptor);

                AnimatedReceptor ar = new AnimatedReceptor(receptor, an);
                synapses.Add(new AnimatedSynapse(ar, an, synapse));
                receptors.Add(ar);
            }

            query.arrange();
        }

        public void newQuery()
        {
            Query query = new Query();
            DialogResult result = query.ShowDialog();

            if (result == DialogResult.Cancel)
                return;

            foreach (AnimatedReceptor ar in receptors)
                synapses.Remove(ar.Output);

            receptors.Clear();
            brain.erase(false);
            addQuery(query.Data, query.Interval, query.Intensity);

            balancing.animate(neurons, synapses, receptors, 160);
            queryAccepted(this.query, new EventArgs());
            loaded = true;
        }

        #endregion

        #region interfejs drawable

        public void show()
        {
            display.show(query);
            create();
            balance();

            framesChanged(length, null);
            frameChanged(frame, null);
        }

        protected override void tick(object sender, EventArgs e)
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

            if (frame == length && animation)
            {
                animationStop(this, new EventArgs());
                animation = false;
                timer.Stop();
                return;
            }

            foreach (AnimatedNeuron neuron in neurons)
                neuron.tick(time);

            foreach (AnimatedSynapse synapse in synapses)
                synapse.tick(time);

            /*
            if (pause)
            {
                stateBar.State -= 3;

                if (stateBar.State <= 0)
                {
                    stateBar.reset();
                    pause = false;
                }
            }
            else if (count++ == interval)
            {
                if (activation)
                {
                    stateBar.Phase = StateBarPhase.Activation;
                    activation = false;
                    pause = true;
                    count--;
                    return;
                }
                else
                {
                    frame++;
                    changeFrame();
                    count = 1;

                    foreach (AnimatedNeuron neuron in neurons)
                    {
                        if (neuron.Neuron.Activity[frame].Active)
                        {
                            activation = true;
                            break;
                        }
                    }
                }
            }*/
        }



        #endregion
    }
}