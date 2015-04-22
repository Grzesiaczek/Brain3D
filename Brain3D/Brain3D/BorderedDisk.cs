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
        protected Circle innerCircle;
        protected Circle borderCircle;

        protected Disk disk;
        protected Ring border;

        protected float radius;

        public BorderedDisk() { }

        public BorderedDisk(Vector3 position, float radius)
        {
            this.position = position;
            this.radius = radius;

            innerCircle = new Circle(position, radius, 1);
            borderCircle = new Circle(position, radius * 1.1f, 1);

            disk = new Disk(position, innerCircle, Color.LightYellow);
            border = new Ring(innerCircle, borderCircle, Color.Purple);

            elements.Add(innerCircle);
            elements.Add(borderCircle);

            drawables.Add(disk);
            drawables.Add(border);
        }

        public override Color Color
        {
            set
            {
                disk.Color = value;
                disk.refresh();
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

                foreach (GraphicsElement element in elements)
                    element.Position = position;
            }
        }
    }
}
