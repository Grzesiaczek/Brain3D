using System.Collections.Generic;
using System.Linq;

namespace Brain3D
{
    class CreationSequence : Sequence
    {
        List<CreationFrame> frames;

        public CreationSequence()
        {
            frames = new List<CreationFrame>();

            if (sequence.Count == 0)
            {
                builder = new BuiltTile();
            }
            else
            {
                builder = new BuiltTile(sequence.Last().Right + 10);
            }

            Add(builder);
        }

        public CreationSequence(List<CreationFrame> frames)
        {
            foreach (CreationFrame frame in frames)
            {
                sequence.Add(frame.Tile);
                frame.Sequence = this;
            }

            this.frames = frames;
            Arrange();
        }

        public CreationFrame Get(int index)
        {
            return frames[index];
        }

        public CreationFrame Add(Brain brain, int index)
        {
            if (builder == null)
            {
                return null;
            }
            /*
            CreationFrame frame = brain.add(this, builder, index);
            SequenceTile neuron = frame.Neuron;

            neuron.Top = 8;
            neuron.Left = builder.Left;
            neuron.idle();
            position = neuron.Right + 10;

            builder = null;
            sequence.Add(neuron);
            frames.Add(frame);
            return frame;*/

            return null;
        }

        public List<CreationFrame> Frames
        {
            get
            {
                return frames;
            }
        }

        public BuiltTile Builder
        {
            get
            {
                return builder;
            }
            set
            {
                builder = value;
            }
        }
    }
}
