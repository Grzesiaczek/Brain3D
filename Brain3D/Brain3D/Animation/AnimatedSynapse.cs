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
    class AnimatedSynapse : AnimatedElement
    {
        #region deklaracje

        AnimatedElement pre;
        AnimatedElement post;

        AnimatedVector vector;

        AnimatedState synapse;
        AnimatedState duplex;

        Vector3 begin;
        Vector3 end;

        #endregion

        #region konstruktory

        public AnimatedSynapse(AnimatedNeuron pre, AnimatedNeuron post, Synapse syn)
        {
            this.pre = pre;
            this.post = post;

            duplex = null;
            vector = new AnimatedVector(pre, post);
            synapse = new AnimatedState(syn, vector);

            pre.Output.Add(this);
            post.Input.Add(this);
        }

        public AnimatedSynapse(AnimatedReceptor pre, AnimatedNeuron post, Synapse syn)
        {
            this.pre = pre;
            this.post = post;
            
            duplex = null;

            vector = new AnimatedVector(pre, post);
            synapse = new AnimatedState(syn, vector);

            pre.Output = this;
            post.Input.Add(this);
        }

        #endregion

        #region rysowanie

        public override void tick(double time)
        {
            int frame = (int)time;
            double rest = time - frame;

            if (synapse.Activity[frame].Item1 && synapse.Activity[frame + 1].Item1)
                synapse.setSignal(synapse.Activity[frame].Item2 + rest / 20);
            else
                synapse.Active = false;

            if (duplex != null)
            {
                if (duplex.Activity[frame].Item1 && duplex.Activity[frame + 1].Item1)
                    duplex.setSignal(duplex.Activity[frame].Item2 + rest / 20);
                else
                    duplex.Active = false;
            }
        }

        public override void draw()
        {
            vector.draw();
            synapse.draw();

            if (duplex != null)
                duplex.draw();
        }

        public override void refresh()
        {
            vector.refresh();

            Vector3 diff = post.Position - pre.Position;
            synapse.Position = 0.75f * diff + pre.Position;
            synapse.refresh();

            if(duplex != null)
            {
                duplex.Position = 0.25f * diff + pre.Position;
                duplex.refresh();
            }
        }

        public override void setFrame(int frame)
        {

        }

        #endregion

        #region sterowanie

        public void change(CreationData data)
        {
            if (data.Synapse == synapse.Synapse)
                synapse.setChange(data);
            else
                duplex.setChange(data);
        }

        public void create(CreationData data)
        {
            if (data.Synapse == synapse.Synapse)
            {
                synapse.History.Add(data);
                synapse.setWeight(data.Weight);
                synapse.Change = 0;
            }
            else
            {
                duplex.History.Add(data);
                duplex.setWeight(data.Weight);
                synapse.Change = 0;
            }
        }

        public void setWeight(CreationData data, bool undo = false)
        {
            float value = data.Weight;

            if (undo)
                value = data.Start;

            if (data.Synapse == synapse.Synapse)
                synapse.setWeight(value);
            else
                duplex.setWeight(value);
        }

        public void create()
        {
            synapse.create();

            if (duplex != null)
                duplex.create();
        }

        public void setDuplex(Synapse synapse)
        {
            duplex = new AnimatedState(synapse, vector, true);
        }

        public bool isDuplex()
        {
            if (duplex == null)
                return false;

            return true;
        }

        public AnimatedState getState(bool duplex)
        {
            if (duplex)
                return this.duplex;

            return synapse;
        }
        #endregion

        #region właściwości

        public AnimatedElement Pre
        {
            get
            {
                return pre;
            }
            set
            {
                pre = value;
            }
        }

        public AnimatedElement Post
        {
            get
            {
                return post;
            }
            set
            {
                post = value;
            }
        }

        public AnimatedVector Vector
        {
            get
            {
                return vector;
            }
            set
            {
                vector = value;
            }
        } 

        public Synapse Synapse
        {
            get
            {
                return synapse.Synapse;
            }
        }

        public Synapse Duplex
        {
            get
            {
                return duplex.Synapse;
            }
        }

        public override float Depth
        {
            get
            {
                if (pre.Depth > post.Depth)
                    return pre.Depth - 0.001f;

                return post.Depth - 0.001f;
            }
        }

        #endregion
    }
}