using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

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

        public override void Move()
        {
            foreach (DrawableElement drawable in drawables)
            {
                drawable.Position = position;
            }

            screen = device.Viewport.Project(position, effect.Projection, effect.View, effect.World);
            base.Move();
        }

        public override void Rotate()
        {
            base.Rotate();
            screen = device.Viewport.Project(position, effect.Projection, effect.View, effect.World);
        }

        void SetData(NeuronActivity data)
        {
            value = data.Value;

            switch(data.Phase)
            {
                case ActivityPhase.Normal:
                    disk.ChangeValue((float)value);
                    number.Value = (int)(Math.Abs(value) * 100);
                    break;

                case ActivityPhase.Start:
                    number.Value = 100;
                    disk.Refract();
                    break;

                case ActivityPhase.Active:
                    number.Value = 100;
                    disk.SetValue(0);
                    disk.Refract((float)data.Refraction / 30);
                    break;

                case ActivityPhase.Finish:
                    number.Value = 100;
                    disk.SetValue(0);
                    break;
            }      
        }

        public override void Tick(double time)
        {
            int frame = (int)time;

            if(frame + 1 == neuron.Activity.Length)
            {
                SetData(neuron.Activity[frame]);
                return;
            }

            NeuronActivity data = neuron.Activity[frame];
            NeuronActivity next = neuron.Activity[frame + 1];

            double factor = time - frame;
            double value = data.Value + factor * (next.Value - data.Value);
            double refraction = data.Refraction + factor * (next.Refraction - data.Refraction);

            SetData(new NeuronActivity(data.Phase, value, refraction));
        }

        public override void SetFrame(int frame)
        {
            NeuronActivity data = neuron.Activity[frame * 10];
            SetData(data);
        }

        public Vector3 PointVector(Vector2 direction)
        {
            double angle = Math.Acos(direction.X);
            float rad = radius * 1.1f;

            if (direction.Y < 0)
            {
                angle = 2 * Math.PI - angle;
            }

            return Vector3.Transform(new Vector3((float)Math.Cos(angle) * rad,(float)Math.Sin(angle) * rad, 0), camera.Rotation) + position;
        }

        public override void Move(int x, int y)
        {
            position = device.Viewport.Unproject(new Vector3(shift.X + x, shift.Y + y, screen.Z), effect.Projection, effect.View, effect.World);
            Move();

            foreach (AnimatedVector synapse in input)
            {
                synapse.Move();
            }

            foreach (AnimatedVector synapse in output)
            {
                synapse.Move();
            }
        }

        public override bool Cursor(int x, int y)
        {
            return disk.Cursor(x, y);
        }

        public override void Activate()
        {
            disk.Activate();
        }

        public override void Hover()
        {
            disk.Hover();
        }

        public override void Idle()
        {
            disk.Idle();
        }

        public override void Show()
        {
            base.Show();
            disk.SetFactor((float)(neuron.Count - 1) / 4);
        }

        public void Create()
        {
            disk.SetValue(0);
            number.Value = 0;
            Show();
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

        public string Word
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

                Rescale();
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
                Rescale();
            }
        }

        #endregion
    }
}
