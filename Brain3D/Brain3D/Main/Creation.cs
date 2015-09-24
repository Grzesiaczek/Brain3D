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
    class Creation : Graph
    {
        #region deklaracje

        List<CreationSequence> sequences;
        List<CreationFrame> frames;

        HashSet<CreatedNeuron> neurons;
        HashSet<CreatedVector> vectors;
        HashSet<CreatedSynapse> synapses;
      
        CreationFrame frame;
        CreationSequence sequence;
        CreatedSynapse history;

        int count;

        double time = 0;
        double interval = 0.4;

        bool insertion;
        bool invitation;

        #endregion

        public Creation()
        {
            neurons = new HashSet<CreatedNeuron>();
            vectors = new HashSet<CreatedVector>();
            synapses = new HashSet<CreatedSynapse>();

            frames = new List<CreationFrame>();

            invitation = false;
            count = 0;
        }

        #region logika

        public void clear()
        {
            neurons.Clear();
            vectors.Clear();
            synapses.Clear();

            mouses.Clear();
            frames.Clear();
            count = 0;
        }

        protected override void BrainLoaded(object sender, EventArgs e)
        {
            clear();

            foreach (Tuple<AnimatedNeuron, CreatedNeuron> tuple in brain.Neurons.Values)
                neurons.Add(tuple.Item2);

            foreach (Tuple<AnimatedVector, CreatedVector> tuple in brain.Vectors.Values)
                vectors.Add(tuple.Item2);

            foreach (Tuple<AnimatedSynapse, CreatedSynapse> tuple in brain.Synapses.Values)
                synapses.Add(tuple.Item2);

            mouses.AddRange(neurons);
            mouses.AddRange(synapses);

            sequences = brain.Sequences;

            foreach (CreationSequence sequence in sequences)
                foreach (CreationFrame frame in sequence.Frames)
                {
                    CreatedNeuron neuron = frame.Neuron;
                    frames.Add(frame);

                    if (neuron.Frame == 0)
                        neuron.Frame = frames.Count;
                }
        }

        public override void Show()
        {
            foreach (CreatedNeuron neuron in neurons)
                neuron.show();

            foreach (CreatedVector vector in vectors)
                vector.show();

            display.Show(this);
            display.HideDisplay();

            if (!balanced)
            {
                balancing.Stop();
                balancing.balance(neurons, vectors);
            }

            foreach (CreatedNeuron neuron in neurons)
                neuron.Hide();

            foreach (CreatedVector vector in vectors)
                vector.Hide();

            for (int i = 0; i < count; i++)
                frames[i].Create();

            controller.ChangeState(count, frames.Count);
            controller.Show();

            insertion = false;
            visible = true;
        }

        public override void BalanceSynapses()
        {
            balancing.balance(vectors);
        }
        
        protected override void Tick()
        {
            time += interval;

            if (frame != null && frame.Tick(interval))
            {
                if (count < frames.Count)
                    setFrame(count + 1);
            }
        }

        public override void MouseClick(int x, int y)
        {
            if (history != null)
            {
                history.historyHide();

                if (active == history)
                {
                    history = null;
                    return;
                }
            }

            if (active != null && active is CreatedSynapse)
            {
                history = (CreatedSynapse)active;
                history.historyShow(x, y, count);
            }
        }

        #endregion

        #region sterowanie

        public override void Start()
        {
            if (Started)
                return;

            if (count < frames.Count - 1)
            {
                if (count == 0)
                    frames[0].Activate();

                if (!paused)
                {
                    frame = frames[count++];
                    controller.ChangeFrame(count);
                }

                if(sequence == null)
                {
                    sequence = frame.Sequence;
                    display.Show(sequence);
                }
            }

            base.Start();
        }

        public override void Back()
        {
            if (count == 0)
                return;

            setFrame(count - 1);
            controller.ChangeFrame(count);
        }

        public override void Forth()
        {
            if(frame != null)
                frame.Create();

            if(count < frames.Count)
                setFrame(++count);
        }

        public override void ChangeFrame(int frame)
        {
            setFrame(frame);
        }

        public override void ChangePace(int pace)
        {
            interval = (8 - Math.Log(116 - pace, 2)) / 8;
        }

        public override void Add(char key)
        {
            if (!insertion)
                return;

            if (invitation)
            {
                clear();
                invitation = false;
                addSequence();
            }

            sequence.Add(key);
        }
        
        public override void Erase()
        {
            sequence.Erase();
        }

        public override void Space()
        {
            if (insertion)
                sequence.Space();
            else if (Started)
                Stop();
            else
                Start();
        }

        public override void Enter()
        {
            if (!insertion)
                return;

            if (sequence.Frames.Count == 0)
                return;

            addFrame(true);
            addSequence();
        }

        public override void Delete()
        {
            controller.ChangeState(--count, frames.Count);
        }

        public override void ChangeInsertion()
        {
            if(insertion)
            {
                setFrame(frames.Count);
                addSequence();
            }
        }

        #endregion

        #region insertion mode

        // do zrobienia
        void addFrame(bool enter = false)
        {

            //addMap(map);
        }

        void addMap(Dictionary<object, object> map)
        {
            foreach (object key in map.Keys)
                if (map[key] is bool)
                {/*
                    CreatedNeuron neuron = mapNeurons[(Neuron)key];
                    neurons.Add(neuron);
                    neuron.show();

                    if(count < neuron.Frame)
                        neuron.Frame = count;*/
                }
                else if (key is Neuron)
                {
                    CreatedNeuron neuron = (CreatedNeuron)(map[key]);
                    //mapNeurons.Add((Neuron)key, neuron);
                    neurons.Add(neuron);

                    neuron.show();
                    neuron.Frame = count;
                }
                else
                {
                    CreatedVector synapse = (CreatedVector)(map[key]);
                    //mapSynapses.Add((Synapse)key, synapse);
                    vectors.Add(synapse);
                    synapse.show();
                }
        }

        void addSequence()
        {
            sequence = new CreationSequence();
            sequences.Add(sequence);

            controller.ChangeState(count, frames.Count);
            display.Show(sequence);
        }

        #endregion

        #region zmiany klatek

        void setFrame(int index)
        {
            controller.ChangeFrame(index);
            bool forward = true;
            paused = false;

            if (count > index)
                forward = false;

            if (forward)
            {
                for (int i = count + 1; i < index; i++)
                    frames[i - 1].Create();
            }
            else
            {
                for (int i = count; i > index; i--)
                    frames[i - 1].Undo();
            }

            if (index == 0)
            {
                if(frame != null)
                    frame.Idle();

                if (sequence != null)
                    sequence.Hide();

                sequence = null;
                frame = null;
                count = 0;
                return;
            }

            if (frame != null)
            {
                frame.Idle();

                if (Started)
                    frame.Execute();
            }

            frame = frames[index - 1];
            frame.Activate();

            count = index;

            if (sequence != frame.Sequence)
            {
                sequence = frame.Sequence;
                display.Show(sequence);
            }

            if(!Started && frame != null)
                frame.Create();
        }

        #endregion
    }
}