using Microsoft.Xna.Framework;

namespace Brain3D
{
    class StateDisk : BorderedDisk
    {
        Ring change;
        Ring outer;

        float r1 = 1;
        float r2 = 1;

        bool activated;

        public StateDisk(Vector3 position, bool changes = false)
        {
            this.position = position;
            color = Color.LightYellow;

            if (changes)
            {
                radius = 0.5f;
            }
            else
            {
                radius = 1.2f;
            }

            Prepare(changes);
        }

        public StateDisk(float value, float factor)
        {
            radius = 12;
            Prepare(true);

            SetValue(value);
            SetFactor(factor);
        }

        public StateDisk(Change value, float factor)
        {
            radius = 12;
            Prepare(true);

            SetChange(value.Start, value.Finish);
            SetFactor(factor);
        }

        void Prepare(bool changes)
        {
            disk = new Disk(position, pattern, color, radius);
            outer = new Ring(position, pattern, Color.LightGreen);
            border = new Ring(position, pattern, Color.DarkSlateBlue);

            if (changes)
            {
                change = new Ring(position, pattern, Color.Green);
                outer = new Ring(position, pattern, Color.LightGreen);
                drawables.Add(change);
            }

            Resize(true);

            drawables.Add(disk);
            drawables.Add(outer);
            drawables.Add(border);
        }

        public void SetFactor(float factor)
        {
            color = Color.LightYellow;
            color.R -= (byte)(12 * factor);
            color.G -= (byte)(48 * factor);
            color.B -= (byte)(60 * factor);

            if (!activated)
            {
                disk.Color = color;
            }
        }

        public void SetChange(float source, float target)
        {
            if(target < source)
            {
                change.Color = Color.IndianRed;
                r1 = 1 - source;
                r2 = 1 - target;
                
            }
            else
            {
                change.Color = Color.SeaGreen;
                r1 = 1 - target;
                r2 = 1 - source;
            }

            Resize();
            Rescale();
        }

        public void SetValue(float value)
        {
            value = 1 - value;

            r1 = value;
            r2 = value;

            Resize();
            disk.Rescale();
            outer.Rescale();
        }

        public void ChangeValue(float value)
        {
            if (value < 0)
            {
                SetValue(value + 1);
                disk.Color = Color.LightBlue;
                outer.Color = color;
            }
            else if (value <= 1)
            {
                SetValue(value);
                disk.Color = color;
                outer.Color = Color.LightGreen;
            }

            Rescale();
        }

        public void Refract()
        {
            SetValue(0);
            disk.Color = Color.IndianRed;
            outer.Color = Color.IndianRed;

            disk.refresh();
            outer.refresh();
        }

        public void Refract(float factor)
        {
            int red = 205 - (int)(factor * 32);
            int green = 92 + (int)(factor * 124);
            int blue = 92 + (int)(factor * 138);

            disk.Color = new Color(red, green, blue);
        }

        public void Resize(bool all = false)
        {
            disk.Radius = r1 * radius;
            outer.R1 = r2 * radius;

            if(change != null)
            {
                change.R1 = r1 * radius;
                change.R2 = r2 * radius;
            }

            if(all)
            {
                outer.R2 = radius;
                border.R1 = radius;
                border.R2 = radius * 1.12f;
            }
        }

        public void Activate()
        {
            border.Color = Color.IndianRed;
            disk.Color = Color.LightCoral;
            activated = true;
        }

        public void Idle()
        {
            border.Color = Color.Purple;
            disk.Color = color;
            activated = false;
        }

        public void Hover()
        {
            border.Color = Color.MediumSeaGreen;
        }

        public float Radius
        {
            set
            {
                radius = value;
                Resize(true);
            }
        }
    }
}
