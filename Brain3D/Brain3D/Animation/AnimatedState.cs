using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class AnimatedState : AnimatedElement
    {
        #region deklaracje

        AnimatedVector vector;

        List<CreationData> history;
        Synapse synapse;

        StateDisk disk;
        BorderedDisk state;
        Signal signal;

        Vector3 shiftDisk = new Vector3(0.5f, 0, 0);
        Vector3 shiftState = new Vector3(0.8f, 0, -0.001f);

        bool active;
        bool activated;
        bool duplex;

        float change;
        float weight;

        #endregion

        public AnimatedState(Synapse synapse, AnimatedVector vector, bool duplex = false)
        {
            this.synapse = synapse;
            this.vector = vector;
            this.duplex = duplex;

            history = new List<CreationData>();

            disk = new StateDisk(position + shiftDisk, 0.5f);
            state = new BorderedDisk(position + shiftState, 0.5f);

            if (duplex)
                signal = new Signal(vector.Target, vector.Source);
            else
                signal = new Signal(vector.Source, vector.Target);

            drawables.Add(state);
            drawables.Add(disk);
            drawables.Add(signal);
        }

        #region sterowanie

        //z ray
        public bool active2(Point location)
        {
            /*if (synapse.Weight == 0)
                return false;

            float x = state.Center.X - location.X;
            float y = state.Center.Y - location.Y;

            if (x * x + y * y < 144)
            {
                activated = true;
                return true;
            }

            activated = false;*/
            return false;
        }

        public void create()
        {
            disk.setValue(synapse.Weight);
            activated = false;
        }

        public void setChange(CreationData data)
        {
            disk.setChange(data);
        }

        public void setSignal(double factor)
        {
            signal.setSignal(factor);

            if(!active)
            {
                active = true;
                state.Color = Color.IndianRed;
            }
        }

        public void setWeight(float weight)
        {
            disk.setValue(weight);
        }

        public override void refresh()
        {
            base.refresh();

            shiftDisk = Vector3.Transform(new Vector3(0.5f * vector.Angle.X, 0.5f * vector.Angle.Y, 0), camera.Rotation);
            shiftState = Vector3.Transform(new Vector3(0.8f * vector.Angle.X, 0.8f * vector.Angle.Y, -0.001f), camera.Rotation);

            disk.Position = position + shiftDisk;
            state.Position = position + shiftState;

            if (duplex)
            {
                signal.Source = vector.Target;
                signal.Target = vector.Source;
            }
            else
            {
                signal.Source = vector.Source;
                signal.Target = vector.Target;
            }

            disk.refresh();
            state.refresh();
            signal.refresh(); 
        }

        #endregion

        #region właściwości

        public Synapse Synapse
        {
            get
            {
                return synapse;
            }
        }

        public Tuple<bool, double>[] Activity
        {
            get
            {
                return synapse.Activity;
            }
            set
            {
                synapse.Activity = value;
            }
        }

        public List<CreationData> History
        {
            get
            {
                return history;
            }
            set
            {
                history = value;
            }
        }

        public bool Active
        {
            set
            {
                if (active && !value)
                    state.Color = Color.LightYellow;

                signal.Active = value;
                active = value;
            }
        }

        public float Change
        {
            get
            {
                return change;
            }
            set
            {
                change = value;
            }
        }

        public float Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
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
            }
        }

        #endregion
    }
}