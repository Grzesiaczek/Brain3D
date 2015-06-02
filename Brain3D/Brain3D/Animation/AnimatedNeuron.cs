using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Fonts;
using Nuclex.Graphics;

namespace Brain3D
{
    class AnimatedNeuron : AnimatedElement
    {
        #region deklaracje

        Neuron neuron;

        List<AnimatedSynapse> input;
        List<AnimatedSynapse> output;

        Label3D label;
        Number3D number;

        StateDisk disk;

        bool refraction; 
        double value;
        float radius;

        #endregion

        #region konstruktory

        public AnimatedNeuron() { }

        public AnimatedNeuron(Neuron neuron, Vector3 position)
        {
            this.neuron = neuron;
            this.position = position;

            input = new List<AnimatedSynapse>();
            output = new List<AnimatedSynapse>();

            radius = 1.2f;

            disk = new StateDisk(position);
            label = new Label3D(Word, position);
            number = new Number3D(position);

            drawables.Add(disk);
            drawables.Add(label);
            drawables.Add(number);
        }

        #endregion

        #region logika

        public override void move()
        {
            foreach (DrawableElement drawable in drawables)
                drawable.Position = position;

            screen = device.Viewport.Project(position, effect.Projection, effect.View, effect.World);
            base.move();
        }

        public override void rotate()
        {
            base.rotate();
            screen = device.Viewport.Project(position, effect.Projection, effect.View, effect.World);
        }

        void setData(NeuronData data)
        {
            value = data.Value;
            
            if (data.Active)
            {
                if (!refraction)
                {
                    refraction = true;
                    number.Value = 100;
                    disk.refract();
                }
                else
                    disk.refract((float)data.Refraction / 30);
            }
            else
            {
                if (refraction)
                    refraction = false;

                disk.changeValue((float)value);
                number.Value = (int)(Math.Abs(value) * 100);
            } 
        }

        public override void tick(double time)
        {
            NeuronData data = neuron.Activity[(int)time];
            NeuronData next = neuron.Activity[(int)time + 1];

            double factor = time - (int)time;
            double value = data.Value + factor * (next.Value - data.Value);
            double refraction = data.Refraction + factor * (next.Refraction - data.Refraction);

            if (data.Refraction == 30)
                refraction = 30;

            setData(new NeuronData(data.Active, value, refraction));
        }

        public override void setFrame(int frame)
        {
            NeuronData data = neuron.Activity[frame * 10];
            setData(data);
        }

        public Vector3 pointVector(Vector2 direction)
        {
            double angle = Math.Acos(direction.X);
            float rad = radius * 1.1f;

            if (direction.Y < 0)
                angle = 2 * Math.PI - angle;

            return Vector3.Transform(new Vector3((float)Math.Cos(angle) * rad,(float)Math.Sin(angle) * rad, 0), camera.Rotation) + position;
        }

        public override void move(int x, int y)
        {
            position = device.Viewport.Unproject(new Vector3(shift.X + x, shift.Y + y, screen.Z), effect.Projection, effect.View, effect.World);
            move();

            foreach (AnimatedSynapse synapse in input)
                synapse.move();

            foreach (AnimatedSynapse synapse in output)
                synapse.move();
        }

        public override bool cursor(int x, int y)
        {
            return disk.cursor(x, y);
        }

        public override void activate()
        {
            disk.activate();
        }

        public override void idle()
        {
            disk.idle();
        }

        public override void hover()
        {
            disk.hover();
        }

        public override void show()
        {
            base.show();
            disk.setFactor((float)(neuron.Count - 1) / 4);
        }

        #endregion

        #region właściwości

        public Neuron Neuron
        {
            get
            {
                return neuron;
            }
        }

        public String Word
        {
            get
            {
                return neuron.Word;
            }
        }

        public List<AnimatedSynapse> Input
        {
            get
            {
                return input;
            }
        }

        public List<AnimatedSynapse> Output
        {
            get
            {
                return output;
            }
        }

        public float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
                disk.Radius = value;

                label.Scale = scale * value / 1.2f;
                number.Scale = scale * value / 1.2f;

                rescale();
            }
        }

        #endregion
    }
}
