using System;
using System.Collections.Generic;
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

        AnimatedElement source;
        AnimatedElement target;

        Pipe pipe;
        Vector2 angle;

        #endregion

        public AnimatedVector(AnimatedElement source, AnimatedElement target)
        {
            this.source = source;
            this.target = target;
            refreshAngle();

            pipe = new Pipe(new Circle(source.pointVector(-angle), 0.08f, 4), new Circle(target.pointVector(angle), 0.08f, 4));
            display.add(pipe);
        }

        public override void refresh()
        {
            base.refresh();
            refreshAngle();

            pipe.Start.Position = source.pointVector(-angle);
            pipe.End.Position = target.pointVector(angle);
            pipe.refresh();
        }

        void refreshAngle()
        {
            Vector3 v1 = source.Screen;
            Vector3 v2 = target.Screen;

            angle = new Vector2(v2.X - v1.X, v2.Y - v1.Y);
            angle.Normalize();
        }

        #region właściwości

        public Vector2 Angle
        {
            get
            {
                return angle;
            }
        }

        public Vector3 Source
        {
            get
            {
                return source.Position;
            }
        }

        public Vector3 Target
        {
            get
            {
                return target.Position;
            }
        }

        public Vector3 Vector
        {
            get
            {
                return End - Start;
            }
        }

        public Vector3 Start
        {
            get
            {
                return pipe.Start.Position;
            }
        }

        public Vector3 End
        {
            get
            {
                return pipe.End.Position;
            }
        }

        #endregion
    }
}
