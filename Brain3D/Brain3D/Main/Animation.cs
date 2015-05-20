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

        QuerySequence query;

        List<AnimatedNeuron> neurons;
        List<AnimatedSynapse> synapses;

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

        #endregion

        public Animation()
        {
            balancing.balanceEnded += balanceEnded;
            balancing.balanceState += balanceState;
            balancing.balanceUpdate += balanceUpdate;

            query = new QuerySequence();
            display.add(this);

            neurons = new List<AnimatedNeuron>();
            synapses = new List<AnimatedSynapse>();
        }

        #region funkcje inicjujące

        Vector3 randomPoint(Random random)
        {
            Vector3 area;

            if (Constant.Space == SpaceMode.Box)
                area = Constant.Box;
            else
            {
                float radius = Constant.Radius * 0.7f;
                area = new Vector3(radius, radius, radius);
            }

            float x = random.Next((int)-area.X + 1, (int)area.X - 1);
            float y = random.Next((int)-area.Y + 1, (int)area.Y - 1);
            float z = random.Next((int)-area.Z + 1, (int)area.Z - 1);

            return new Vector3(x, y, z);
        }

        public void clear()
        {
            balancing.stop();
            neurons.Clear();
            synapses.Clear();
            display.clear();
        }

        public void reload()
        {
            Random random = new Random();
            Dictionary<Neuron, AnimatedNeuron> map = new Dictionary<Neuron, AnimatedNeuron>();

            foreach (Neuron neuron in brain.Neurons)
            {
                Vector3 position = randomPoint(random);
                AnimatedNeuron animated = new AnimatedNeuron(neuron, position);
                neurons.Add(animated);
                map.Add(neuron, animated);
            }

            foreach(Synapse synapse in brain.Synapses)
            {
                AnimatedSynapse animated = synapses.Find(k => k.Synapse.Post == synapse.Pre && k.Synapse.Pre == synapse.Post);

                if (animated == null)
                    synapses.Add(new AnimatedSynapse(map[synapse.Pre], map[synapse.Post], synapse));
                else
                    animated.setDuplex(synapse);
            }
        }

        protected override void brainLoaded(object sender, EventArgs e)
        {
            reload();            
        }

        public Dictionary<object, object> loadFrame(CreationFrame frame, int index)
        {
            Dictionary<object, object> result = new Dictionary<object, object>();
            Random random = new Random();

            Neuron neuron = frame.Neuron.Neuron;

            if (neurons.Find(k => k.Neuron == neuron) == null)
            {
                Vector3 position = randomPoint(random);
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

        public void create(Creation creation)
        {
            creation.clear();
            creation.load(neurons, synapses);
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
                controller.changeFrame(1);
            }
            else
                time = frame * 10;

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

            foreach(AnimatedSynapse synapse in synapses)
                synapse.setFrame(frame);

            query.tick(frame);
            controller.changeFrame(frame);
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
            controller.balance();
            balancing.animate(neurons, synapses, 60);
            //balancing.balance(neurons, synapses);
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
            //int value = (int)Math.Min(stateBar.Height, result * 40);
            //stateBar.State = value;
        }

        private void balanceEnded(object sender, EventArgs e)
        {
            controller.idle();
            display.move();
            balanceFinished(this, new EventArgs());
        }

        #endregion

        public void load(int value)
        {
            loaded = true;
            length = value;
        }

        #region obsługa zapytań

        public void addQuery(String[] words, int interval, float intensivity)
        {
            frame = 0;

            controller.changeFrame(frame);
            query.clear();

            foreach (String word in words)
            {
                AnimatedNeuron an = neurons.Find(k => k.Name == word);

                if (an == null)
                    continue;
            }

            query.arrange();
        }

        public void newQuery()
        {
            brain.erase(false);

            balancing.animate(neurons, synapses, 40);
            loaded = true;
        }

        #endregion

        #region interfejs drawable

        public override void show()
        {
            foreach (AnimatedNeuron neuron in neurons)
                neuron.show();

            foreach (AnimatedSynapse synapse in synapses)
            {
                synapse.show();
                synapse.create();
            }

            display.add(controller);
            display.show(query);
            display.change(false);

            balance();
            controller.idle();
            controller.changeState(frame, length);
            visible = true;
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

            display.repaint();
        }

        #endregion
    }
}