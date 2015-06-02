using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class AnimatedSynapse : AnimatedElement
    {
        #region deklaracje

        AnimatedNeuron pre;
        AnimatedNeuron post;

        AnimatedVector vector;

        AnimatedState state;
        AnimatedState duplex;

        List<Vector3> pos = new List<Vector3>();
        List<Vector3> vec = new List<Vector3>();
        List<Vector3> sta = new List<Vector3>();
        List<Vector3> con = new List<Vector3>();

        #endregion

        #region konstruktory

        public AnimatedSynapse(AnimatedNeuron pre, AnimatedNeuron post, Synapse synapse)
        {
            this.pre = pre;
            this.post = post;

            duplex = null;
            vector = new AnimatedVector(pre, post);
            state = new AnimatedState(synapse, vector);

            drawables.Add(vector);
            drawables.Add(state);

            pre.Output.Add(this);
            post.Input.Add(this);
        }

        #endregion

        #region rysowanie

        public override void tick(double time)
        {
            int frame = (int)time;
            double rest = time - frame;

            state.tick(frame, rest);

            if (duplex != null)
                duplex.tick(frame, rest);
        }

        public override void move()
        {
            rotate();
            base.move();
        }

        public override void rotate()
        {
            vector.rotate();
            state.rotate();

            if(duplex != null)
                duplex.rotate();
        }

        public override void setFrame(int frame)
        {
            frame *= 10;
            state.tick(frame, 0);

            if (duplex != null)
                duplex.tick(frame, 0);
        }

        public override bool cursor(int x, int y)
        {
            if (!visible)
                return false;

            return base.cursor(x, y);
        }

        #endregion

        #region sterowanie

        public void create()
        {
            vector.Scale = 1;
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
            show();
        }

        public void setDuplex(Synapse synapse)
        {
            duplex = new AnimatedState(synapse, vector, true);
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

        public AnimatedVector Vector
        {
            get
            {
                return vector;
            }
        }

        public AnimatedState State
        {
            get
            {
                return state;
            }
        }

        public AnimatedState Duplex
        {
            get
            {
                return duplex;
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

        public override float Scale
        {
            set
            {
                vector.Scale = value;
                state.Scale = value;

                if (duplex != null)
                    duplex.Scale = value;
            }
        }

        public override float Depth
        {
            get
            {
                return (pre.Depth + post.Depth) / 2;
            }
        }

        #endregion
    }
}
