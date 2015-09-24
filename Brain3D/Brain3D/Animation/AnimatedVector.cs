﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class AnimatedVector : AnimatedElement
    {
        #region deklaracje

        AnimatedNeuron pre;
        AnimatedNeuron post;

        AnimatedSynapse state;
        AnimatedSynapse duplex;

        Pipe pipe;
        Vector2 angle;

        Vector3 direction;
        Vector3 vector;

        #endregion

        #region konstruktory

        public AnimatedVector(AnimatedNeuron pre, AnimatedNeuron post, Synapse synapse)
        {
            this.pre = pre;
            this.post = post;

            duplex = null;

            pipe = new Pipe(pre.PointVector(-angle), post.PointVector(angle), 0.1f, 0.1f, 0);
            state = new AnimatedSynapse(synapse, this);

            drawables.Add(pipe);
            drawables.Add(state);

            pre.Output.Add(this);
            post.Input.Add(this);

            refreshAngle();
        }

        #endregion

        #region rysowanie

        public override void Tick(double time)
        {
            int frame = (int)time;
            double rest = time - frame;

            state.tick(frame, rest);

            if (duplex != null)
                duplex.tick(frame, rest);
        }

        public override void Move()
        {
            Rotate();
            base.Move();
        }

        public override void Rotate()
        {
            refreshAngle();
            pipe.Source = pre.PointVector(-angle);
            pipe.Target = post.PointVector(angle);

            pipe.Rotate();
            state.Rotate();

            if(duplex != null)
                duplex.Rotate();
        }

        void refreshAngle()
        {
            Vector3 v1 = pre.Screen;
            Vector3 v2 = post.Screen;

            angle = new Vector2(v2.X - v1.X, v2.Y - v1.Y);
            angle.Normalize();

            vector = pipe.Target - pipe.Source;
            direction = post.Position - pre.Position;
            direction.Normalize();
        }

        public override void SetFrame(int frame)
        {
            frame *= 10;
            state.tick(frame, 0);

            if (duplex != null)
                duplex.tick(frame, 0);
        }

        public override bool Cursor(int x, int y)
        {
            if (!visible)
                return false;

            return base.Cursor(x, y);
        }

        #endregion

        #region sterowanie

        public void Create()
        {
            pipe.Scale = 1;
            state.create();

            if (duplex != null)
                duplex.create();
        }

        public void init()
        {
            state.setValue(0);

            if (duplex != null)
                duplex.setValue(0);

            Scale = 0;
            Show();
        }

        public void setDuplex(Synapse synapse)
        {
            duplex = new AnimatedSynapse(synapse, this, true);
            drawables.Add(duplex);
        }

        #endregion

        #region właściwości

        public AnimatedNeuron Pre
        {
            get
            {
                return pre;
            }
        }

        public AnimatedNeuron Post
        {
            get
            {
                return post;
            }
        }

        public AnimatedSynapse State
        {
            get
            {
                return state;
            }
        }

        public AnimatedSynapse Duplex
        {
            get
            {
                return duplex;
            }
        }

        public Vector2 Angle
        {
            get
            {
                return angle;
            }
        }

        public Vector3 Direction
        {
            get
            {
                return direction;
            }
        }

        public Vector3 Source
        {
            get
            {
                return pipe.Source;
            }
        }

        public Vector3 Target
        {
            get
            {
                return pipe.Target;
            }
        }

        public override float Depth
        {
            get
            {
                return (pre.Depth + post.Depth) / 2;
            }
        }

        public override float Scale
        {
            set
            {
                pipe.Scale = value;
                state.Scale = value;

                if (duplex != null)
                    duplex.Scale = value;
            }
        }

        public float Length
        {
            get
            {
                return vector.Length();
            }
        }

        public float Weight
        {
            get
            {
                float weight = state.Weight;

                if (duplex != null)
                    weight += duplex.Weight;

                return weight;
            }
        }

        #endregion
    }
}
