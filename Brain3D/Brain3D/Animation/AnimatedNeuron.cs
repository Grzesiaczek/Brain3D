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

        List<AnimatedVector> input;
        List<AnimatedVector> output;

        StateDisk disk;
        Label3D label;
        Number3D number;

        double value;
        float radius;

        #endregion

        #region konstruktory

        public AnimatedNeuron() { }

        public AnimatedNeuron(Neuron neuron, Vector3 position)
        {
            this.neuron = neuron;
            this.position = position;

            input = new List<AnimatedVector>();
            output = new List<AnimatedVector>();

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

        void setData(NeuronActivity data)
        {
            value = data.Value;

            switch(data.Phase)
            {
                case ActivityPhase.Normal:
                    disk.changeValue((float)value);
                    number.Value = (int)(Math.Abs(value) * 100);
                    break;
                case ActivityPhase.Start:
                    number.Value = 100;
                    disk.refract();
                    break;
                case ActivityPhase.Active:
                    number.Value = 100;
                    disk.setValue(0);
                    disk.refract((float)data.Refraction / 30);
                    break;
                case ActivityPhase.Finish:
                    number.Value = 100;
                    disk.setValue(0);
                    break;
            }      
        }

        public override void tick(double time)
        {
            int frame = (int)time;

            if(frame + 1 == neuron.Activity.Length)
            {
                setData(neuron.Activity[frame]);
                return;
            }

            NeuronActivity data = neuron.Activity[frame];
            NeuronActivity next = neuron.Activity[frame + 1];

            double factor = time - frame;
            double value = data.Value + factor * (next.Value - data.Value);
            double refraction = data.Refraction + factor * (next.Refraction - data.Refraction);

            setData(new NeuronActivity(data.Phase, value, refraction));
        }

        public override void setFrame(int frame)
        {
            NeuronActivity data = neuron.Activity[frame * 10];
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

            foreach (AnimatedVector synapse in input)
                synapse.move();

            foreach (AnimatedVector synapse in output)
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

        public void create()
        {
            disk.setValue(0);
            number.Value = 0;
            show();
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

        public List<AnimatedVector> Input
        {
            get
            {
                return input;
            }
        }

        public List<AnimatedVector> Output
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

        public override float Scale
        {
            set
            {
                scale = value;
                disk.Scale = value;
                label.Scale = value * radius / 1.2f;
                number.Scale = value * radius / 1.2f;
                rescale();
            }
        }

        #endregion
    }
}
