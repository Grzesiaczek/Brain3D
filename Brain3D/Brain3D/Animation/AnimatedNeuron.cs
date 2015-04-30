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

        Text3D number;
        Label3D label;

        StateDisk disk;

        bool refraction;
        float radius;
        double value;

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

            disk = new StateDisk(position, radius);
            label = new Label3D(Name, position);
            number = new Text3D("0", position);

            drawables.Add(disk);
            drawables.Add(label);
            drawables.Add(number);
        }

        #endregion

        #region logika

        void setData(NeuronData data)
        {
            value = data.Value;
            
            if (data.Active)
            {
                if (!refraction)
                {
                    refraction = true;
                    number.Text = "R";
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
                number.Text = ((int)(Math.Abs(value) * 100)).ToString();
            } 
        }

        public override void tick(double time)
        {
            NeuronData data = neuron.Activity[(int)time];
            NeuronData next = neuron.Activity[(int)time + 1];

            double factor = time - (int)time;
            double value = data.Value + factor * (next.Value - data.Value);
            double refraction = data.Refraction + factor * (next.Refraction - data.Refraction);

            setData(new NeuronData(data.Active, value, refraction));
        }

        public override void setFrame(int frame)
        {
            NeuronData data = neuron.Activity[frame * 10];
            setData(data);
        }

        public override Vector3 pointVector(Vector2 angle2)
        {
            double angle = Math.Acos(angle2.X);
            float rad = radius * 1.05f;

            if (angle2.Y < 0)
                angle = 2 * Math.PI - angle;

            return Vector3.Transform(new Vector3((float)Math.Cos(angle) * rad,(float)Math.Sin(angle) * rad, 0.1f), camera.Rotation) + position;
        }

        public void checkCollision(List<AnimatedNeuron> neurons)
        {

        }

        public void activate(bool shifted)
        {
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

        public String Name
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
            set
            {
                input = value;
            }
        }

        public List<AnimatedSynapse> Output
        {
            get
            {
                return output;
            }
            set
            {
                output = value;
            }
        }

        public override Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                disk.Position = position;
                number.Position = position;
                label.Position = position;
            }
        }

        public float Radius
        {
            get
            {
                return radius;
            }
        }

        public bool State
        {
            set
            {
                number.Suppress = !value;
            }
        }

        #endregion
    }
}
