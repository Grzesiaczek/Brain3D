using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Brain3D
{
    class Creation : Presentation
    {
        #region deklaracje

        Dictionary<Neuron, CreatedNeuron> mapNeurons;
        Dictionary<Synapse, CreatedSynapse> mapSynapses;
        Dictionary<AnimatedState, CreationHistory> mapHistory;

        List<CreationSequence> sequences;
        List<Synapse> duplex;

        List<CreatedNeuron> neurons;
        List<CreatedSynapse> synapses;
        List<Tuple<int, int>> tracking;

        AnimatedState active;
        CreationFrame frame;
        CreationSequence sequence;

        Tuple<int, int> tuple;

        int count;
        int time;
        int length;

        bool built;
        bool invitation;

        public event EventHandler animationStop;
        public event EventHandler creationFinished;

        #endregion

        public Creation()
        {
            mapNeurons = new Dictionary<Neuron, CreatedNeuron>();
            mapSynapses = new Dictionary<Synapse, CreatedSynapse>();
            mapHistory = new Dictionary<AnimatedState, CreationHistory>();

            neurons = new List<CreatedNeuron>();
            synapses = new List<CreatedSynapse>();

            CreationFrame.setDictionary(mapNeurons, mapSynapses);
            duplex = new List<Synapse>();

            tracking = new List<Tuple<int, int>>();
            tuple = new Tuple<int, int>(-1, -1);
            tracking.Add(tuple);

            invitation = true;
            count = 0;
        }

        #region logika

        public void clear()
        {
            neurons.Clear();
            synapses.Clear();

            mapNeurons.Clear();
            mapSynapses.Clear();
            mapHistory.Clear();

            tracking.Clear();
            tracking.Add(new Tuple<int, int>(-1, -1));

            length = 0;
            count = 0;
        }

        public void load()
        {
            sequences = brain.Sequences;

            for (int i = 0; i < sequences.Count; i++)
            {
                int size = sequences[i].Frames.Count;
                length += size;

                for (int j = 0; j < size; j++)
                    tracking.Add(new Tuple<int, int>(i, j));
            }
        }

        public void load(List<AnimatedNeuron> neurons, List<AnimatedSynapse> synapses)
        {
            foreach (AnimatedNeuron neuron in neurons)
            {
                CreatedNeuron created = new CreatedNeuron(neuron);
                mapNeurons.Add(neuron.Neuron, created);
                this.neurons.Add(created);
            }

            foreach (AnimatedSynapse synapse in synapses)
            {
                CreatedSynapse cSynapse = new CreatedSynapse(synapse);
                mapSynapses.Add(synapse.Synapse, cSynapse);
                this.synapses.Add(cSynapse);

                if (synapse.isDuplex())
                    mapSynapses.Add(synapse.Duplex, cSynapse);
            }

            int index = 1;

            foreach (CreationSequence sequence in sequences)
                foreach (CreationFrame frame in sequence.Frames)
                {
                    frame.finish += new EventHandler(finish);
                    CreatedNeuron neuron = mapNeurons[frame.Neuron.Neuron];

                    if (neuron.Frame == 0)
                        neuron.Frame = index;

                    index++;
                }
        }
        
        protected override void tick(object sender, EventArgs e)
        {
            if (active != null && ++time == 12)
            {
                if (mapHistory.ContainsKey(active))
                    mapHistory[active].show();
                else
                {
                    CreationHistory history = new CreationHistory(active);
                    mapHistory.Add(active, history);
                    history.show();
                }
            }
        }

        void finish(object sender, EventArgs e)
        {
            if (sender is CreatedNeuron)
                neurons.Add((CreatedNeuron)sender);
            else if (sender is List<Synapse>)
                foreach (Synapse s in (List<Synapse>)sender)
                    synapses.Add(mapSynapses[s]);
            else
                nextFrame();
        }

        public override void show()
        {
            load();

            foreach (CreatedNeuron neuron in neurons)
                neuron.show();

            foreach (CreatedSynapse synapse in synapses)
                synapse.show();

            display.add(controller);
            display.change(false);
            display.hide();

            balancing.stop();
            balancing.balance(neurons, synapses);

            controller.idle();
            controller.changeState(count, length);
            visible = true;
        }

        #endregion

        #region sterowanie

        public override void start()
        {
            if (animation)
                return;

            animation = true;
            setFrame(count);
        }

        public override void stop()
        {
            if (!animation)
                return;

            animation = false;
            animationStop(this, new EventArgs());
        }

        public override void back()
        {
            if (count == 0)
            {
                creationFinished(false, null);
                return;
            }

            setFrame(count - 1);
            controller.changeFrame(count);
        }

        public override void forth()
        {
            if(frame != null)
                frame.create();

            nextFrame();
        }

        public override void changeFrame(int frame)
        {
            if (count >= tracking.Count)
                return;

            setFrame(frame);
        }

        public override void changePace(int pace)
        {
            if (frame != null)
                frame.setInterval(pace / 25);
            else
                CreationFrame.Interval = pace / 25;
        }

        public override void add(char key)
        {
            if (invitation)
            {
                clear();
                invitation = false;
                addSequence();

                brain = new Brain();
            }

            sequence.add(key);
        }
        
        public override void erase()
        {
            sequence.erase();
        }

        public override void space()
        {
            addFrame();
            controller.changeState(++count, ++length);
        }

        public override void enter()
        {
            if (sequence.Frames.Count == 0)
                return;

            if(built)
            {
                addFrame(true);
                built = false;
                //return;
            }

            addSequence();
        }

        public override void delete()
        {
            controller.changeState(--count, --length);
        }

        #endregion

        #region zmiany klatek

        void addFrame(bool enter = false)
        {
            if (sequence == null)
                return;

            Dictionary<object, object> map;
            changeFrame(sequence.add(brain, count));

            if (frame != null)
            {
                map = display.loadFrame(frame, count);

                if (enter)
                    frame.Neuron.activate();
            }
            else
                return;

            tuple = new Tuple<int, int>(tuple.Item1, tuple.Item2 + 1);
            tracking.Insert(count, tuple);

            addMap(map);
            frame.change();
        }

        void addMap(Dictionary<object, object> map)
        {
            foreach (object key in map.Keys)
                if (map[key] is bool)
                {
                    CreatedNeuron neuron = mapNeurons[(Neuron)key];
                    neurons.Add(neuron);
                    neuron.show();

                    if(count < neuron.Frame)
                        neuron.Frame = count;
                }
                else if (key is Neuron)
                {
                    CreatedNeuron neuron = (CreatedNeuron)(map[key]);
                    mapNeurons.Add((Neuron)key, neuron);
                    neurons.Add(neuron);

                    neuron.show();
                    neuron.Frame = count;
                }
                else
                {
                    CreatedSynapse synapse = (CreatedSynapse)(map[key]);
                    mapSynapses.Add((Synapse)key, synapse);
                    synapses.Add(synapse);
                    synapse.show();
                }
        }

        void addSequence()
        {
            int index = tuple.Item1 + 1;

            sequence = new CreationSequence();
            sequences.Insert(index, sequence);

            while (++count < length && tracking[count].Item1 != index) ;

            for (int i = count; i < tracking.Count; i++)
                tracking[i] = new Tuple<int, int>(tracking[i].Item1 + 1, tracking[i].Item2);

            if (count == length || tuple.Item1 == -1)
                tuple = new Tuple<int, int>(0, -1);

            controller.changeState(count, ++length);
            display.show(sequence);
            built = true;
        }

        void changeFrame(CreationFrame frame, bool change = false)
        {
            this.frame = frame;

            if (change)
                frame.change();

            if (animation)
                frame.step();
        }

        void setFrame(int index)
        {
            controller.changeFrame(index);
            bool forward = true;

            if (count > index)
                forward = false;

            if (forward)
            {
                for (int i = count + 1; i <= index; i++)
                    sequences[tracking[i].Item1].Frames[tracking[i].Item2].create();
            }
            else
            {
                for (int i = count; i > index; i--)
                    sequences[tracking[i].Item1].Frames[tracking[i].Item2].undo();
            }

            count = index;

            if (index == 0)
            {
                display.clear();
                tuple = tracking[0];
                frame.Neuron.idle();
                return;
            }

            Tuple<int, int> tup = tracking[count];

            if (tup.Item1 != tuple.Item1)
            {
                sequence = sequences[tup.Item1];
                display.show(sequence);
            }

            if(forward && frame != null)
                frame.execute();

            changeFrame(sequence.get(tup.Item2), true);
        }

        void nextFrame()
        {
            if (count == length)
            {
                creationFinished(true, null);
                return;
            }

            setFrame(count + 1);
        }

        #endregion
    }
}