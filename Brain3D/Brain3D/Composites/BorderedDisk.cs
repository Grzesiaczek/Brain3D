using Microsoft.Xna.Framework;

namespace Brain3D
{
    class BorderedDisk : DrawableComposite
    {
        protected static Circle pattern;

        protected Disk disk;
        protected Ring border;

        protected float radius;

        public BorderedDisk() { }

        public BorderedDisk(Vector3 position)
        {
            this.position = position;
            radius = 0.5f;

            disk = new Disk(position, pattern, Color.LightYellow, radius);
            border = new Ring(position, pattern, Color.DarkSlateBlue);

            border.R1 = radius;
            border.R2 = radius * 1.12f;

            drawables.Add(disk);
            drawables.Add(border);
        }

        public static void InitializeCircle()
        {
            pattern = new Circle(1);
            pattern.Rotate();
        }

        public override bool Cursor(int x, int y)
        {
            if (!visible)
            {
                return false;
            }

            Vector3 near = device.Viewport.Unproject(new Vector3(x, y, 0), effect.Projection, effect.View, effect.World);
            Vector3 far = device.Viewport.Unproject(new Vector3(x, y, 1), effect.Projection, effect.View, effect.World);

            Vector3 direction = far - near;
            direction.Normalize();

            if (new BoundingSphere(position, radius).Intersects(new Ray(near, direction)) == null)
            {
                return false;
            }

            return true;
        }

        public override Color Color
        {
            set
            {
                color = value;
                disk.Color = value;
                disk.Repaint();
            }
        }
    }
}
