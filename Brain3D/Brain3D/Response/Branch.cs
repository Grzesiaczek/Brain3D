﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Brain3D
{
    class Branch : DrawableComposite
    {
        Leaf source;
        Leaf target;

        Pipe pipe;

        public Branch(Leaf source, Leaf target)
        {
            this.source = source;
            this.target = target;

            pipe = new Pipe(source.Position, target.Position, 0.005f, 0.005f, 0);
            drawables.Add(pipe);
            pipe.Scale = 1;
        }

        public override void Move()
        {
            pipe.Source = source.Position;
            pipe.Target = target.Position;
            pipe.Move();
        }

        public Leaf Source
        {
            get
            {
                return source;
            }
        }

        public Leaf Target
        {
            get
            {
                return target;
            }
        }
    }
}
