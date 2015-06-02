﻿using System;
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
        Synapse synapse;

        StateDisk disk;
        BorderedDisk state;
        Signal signal;

        Vector3 shiftDisk = new Vector3(0, 0, -0.1f);
        Vector3 shiftState = new Vector3(0.3f, 0, -0.12f);

        bool active;
        bool duplex;

        float factor;
        float weight;

        #endregion

        public AnimatedState(Synapse synapse, AnimatedVector vector, bool duplex = false)
        {
            this.synapse = synapse;
            this.vector = vector;
            this.duplex = duplex;

            disk = new StateDisk(position + shiftDisk, true);
            state = new BorderedDisk(position + shiftState);

            if (duplex)
            {
                signal = new Signal(vector.Target, vector.Source);
                factor = 0.25f;
            }
            else
            {
                signal = new Signal(vector.Source, vector.Target);
                factor = 0.75f;
            }

            drawables.Add(state);
            drawables.Add(disk);
            drawables.Add(signal);
        }

        #region sterowanie

        public void create()
        {
            Scale = 1;
            setFactor(synapse.Factor);
            disk.setValue(synapse.Weight);
            weight = synapse.Weight;
        }

        public void setChange(float source, float target)
        {
            disk.setChange(source, target);
        }

        public void setFactor(float factor)
        {
            color = Color.LightYellow;
            color.R -= (byte)(10 * factor);
            color.G -= (byte)(33 * factor);
            color.B -= (byte)(45 * factor);

            disk.Color = color;
            state.Color = color;
        }

        public void setValue(float weight)
        {
            disk.changeValue(weight);
        }

        public void tick(int frame, double rest)
        {
            if (synapse.Activity[frame].Item1 && synapse.Activity[frame + 1].Item1)
            {
                signal.setSignal(synapse.Activity[frame].Item2 + rest / 20);

                if (!active)
                {
                    active = true;
                    state.Color = Color.IndianRed;
                    state.repaint();
                }
            }
            else if (active)
            {
                active = false;
                signal.setSignal(-1);
                state.Color = color;
                state.repaint();
            }
        }

        public override void rotate()
        {
            Vector3 pre = device.Viewport.Project(vector.Source, effect.Projection, effect.View, effect.World);
            Vector3 post = device.Viewport.Project(vector.Target, effect.Projection, effect.View, effect.World);
            position = device.Viewport.Unproject(factor * (post - pre) + pre, effect.Projection, effect.View, effect.World);

            shiftDisk = Vector3.Transform(new Vector3(0, 0, -0.104f), camera.Rotation);
            shiftState = Vector3.Transform(new Vector3(0.3f * vector.Angle, -0.102f), camera.Rotation);

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
        }

        public override void idle()
        {
            if (active)
                state.Color = Color.IndianRed;
            else
                state.Color = color;
        }

        public override void hover()
        {
            state.Color = Color.Orange;
        }

        public override bool cursor(int x, int y)
        {
            return disk.cursor(x, y);
        }

        public override void move(int x, int y)
        {
            Vector3 start = device.Viewport.Project(vector.Source, effect.Projection, effect.View, effect.World);
            Vector3 end = device.Viewport.Project(vector.Target, effect.Projection, effect.View, effect.World);
            Vector3 vec = end - start;

            Tuple<Vector2, float, float> tuple = Constant.getDistance(start, end, new Vector3(x, y, (start.Z + end.Z) / 2));

            float min = 20 / vec.Length();
            float max = 1 - min;

            if (tuple.Item3 < min)
                factor = min;
            else if (tuple.Item3 > max)
                factor = max;
            else
                factor = tuple.Item3;

            rotate();
            move();
        }

        #endregion

        #region właściwości

        public override Vector3 Screen
        {
            get
            {
                return device.Viewport.Project(position, effect.Projection, effect.View, effect.World);
            }
        }

        public AnimatedVector Vector
        {
            get
            {
                return vector;
            }
        }

        public float Factor
        {
            get
            {
                return factor;
            }
            set
            {
                factor = value;
            }
        }

        public float Weight
        {
            get
            {
                return weight;
            }
        }

        #endregion
    }
}