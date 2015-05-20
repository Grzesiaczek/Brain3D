using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class BorderedDisk : CompositeElement
    {
        protected static Circle framework;

        protected Disk disk;
        protected Ring border;

        protected float radius;

        public BorderedDisk() { }

        public BorderedDisk(Vector3 position, Color color, float radius)
        {
            this.position = position;
            this.radius = radius;

            disk = new Disk(position, framework, color, radius);
            border = new Ring(position, framework, Color.Purple);

            drawables.Add(disk);
            drawables.Add(border);
        }

        public static void initializeCircle()
        {
            framework = new Circle(Vector3.Zero, 1);
            framework.rotate();
        }

        public override Color Color
        {
            set
            {
                disk.Color = value;
                disk.repaint();
            }
        }
    }
}
