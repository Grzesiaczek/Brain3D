using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class AnimatedVector : DrawableComposite
    {
        #region deklaracje

        AnimatedNeuron source;
        AnimatedNeuron target;

        Pipe pipe;
        Vector2 angle;

        Vector3 direction;
        Vector3 vector;

        #endregion

        public AnimatedVector(AnimatedNeuron source, AnimatedNeuron target)
        {
            this.source = source;
            this.target = target;

            pipe = new Pipe(source.pointVector(-angle), target.pointVector(angle), 0.1f, 0.1f, 0);
            drawables.Add(pipe);

            refreshAngle();
        }

        public override void rotate()
        {
            refreshAngle();
            pipe.Source = source.pointVector(-angle);
            pipe.Target = target.pointVector(angle);
            pipe.rotate();
        }

        void refreshAngle()
        {
            Vector3 v1 = source.Screen;
            Vector3 v2 = target.Screen;

            angle = new Vector2(v2.X - v1.X, v2.Y - v1.Y);
            angle.Normalize();

            vector = pipe.Target - pipe.Source;
            direction = target.Position - source.Position;
            direction.Normalize();
        }

        #region właściwości

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
                direction = target.Position - source.Position;
                direction.Normalize();
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

        public Vector3 Vector
        {
            get
            {
                return vector;
            }
        }

        #endregion
    }
}
