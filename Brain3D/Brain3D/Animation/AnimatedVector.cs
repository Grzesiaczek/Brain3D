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

        Vector3 start;
        Vector3 end;

        #endregion

        public AnimatedVector(AnimatedElement source, AnimatedElement target)
        {
            this.source = source;
            this.target = target;

            pipe = new Pipe(new Circle(source.Position, 0.05f, 4), new Circle(target.Position, 0.05f, 4));
            drawables.Add(pipe);
        }

        public override void refresh()
        {
            base.refresh();

            pipe.Start.Position = source.Position;
            pipe.End.Position = target.Position;
            pipe.refresh();

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

        #endregion
    }
}
