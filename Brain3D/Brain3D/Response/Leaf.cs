using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class Leaf : DrawableComposite
    {
        Neuron neuron;

        Rect background;
        Rect border;

        LabelAxis word;
        DottedLine line;

        Vector3 size;

        int time;

        public Leaf(Neuron neuron, int time, float y)
        {
            this.neuron = neuron;
            this.time = time;

            Vector2 font = Fonts.SpriteArial.MeasureString(neuron.Word) * 0.005f;

            float width = 0.06f + font.X;
            float height = 0.04f + font.Y;

            Vector3 corner = new Vector3(0.01f * time, y, 0);
            Vector3 shift = new Vector3(0.01f, 0.01f, 0);

            size = new Vector3(width, height, 0);
            position = corner + size / 2;

            background = new Rect(corner + shift, size - 2 * shift, Color.LightYellow);
            border = new Rect(corner - new Vector3(0, 0, 0.001f), size, Color.Purple);
            word = new LabelAxis(position + new Vector3(0, -0.024f, 0.001f), neuron.Word);
            line = new DottedLine(new Vector3(corner.X, -1, -0.001f), new Vector3(corner.X, 2, -0.001f), Color.MediumSlateBlue, 0.005f, 16);

            drawables.Add(background);
            drawables.Add(border);
            drawables.Add(word);
            drawables.Add(line);
        }

        public Neuron Neuron
        {
            get
            {
                return neuron;
            }
        }

        public override float Scale
        {
            set
            {
                scale = value;
                float x = 0.01f * time * scale;
                position = new Vector3(x + size.X / 2, position.Y, position.Z);

                background.moveX(x + 0.01f);
                word.moveX(position.X);

                border.moveX(x);
                line.moveX(x);
            }
        }
    }
}
