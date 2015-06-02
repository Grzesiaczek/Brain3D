using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class StateDisk : BorderedDisk
    {
        Ring change;
        Ring outer;

        float r1 = 1;
        float r2 = 1;

        public StateDisk(Vector3 position, bool changes = false)
        {
            this.position = position;
            this.radius = radius;
            color = Color.LightYellow;

            if (changes)
                radius = 0.5f;
            else
                radius = 1.2f;

            prepare(changes);
        }

        public StateDisk(float value, float factor)
        {
            radius = 12;
            prepare(true);

            setValue(value);
            setFactor(factor);
        }

        public StateDisk(Change value, float factor)
        {
            radius = 12;
            prepare(true);

            setChange(value.Start, value.Finish);
            setFactor(factor);
        }

        void prepare(bool changes)
        {
            disk = new Disk(position, pattern, color, radius);
            outer = new Ring(position, pattern, Color.LightGreen);
            border = new Ring(position, pattern, Color.Purple);

            if (changes)
            {
                change = new Ring(position, pattern, Color.Green);
                outer = new Ring(position, pattern, Color.LightGreen);
                drawables.Add(change);
            }

            resize(true);

            drawables.Add(disk);
            drawables.Add(outer);
            drawables.Add(border);
        }

        public void setFactor(float factor)
        {
            color = Color.LightYellow;
            color.R -= (byte)(12 * factor);
            color.G -= (byte)(48 * factor);
            color.B -= (byte)(60 * factor);
            disk.Color = color;
        }

        public void setChange(float source, float target)
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

            resize();
            rescale();
        }

        public void setValue(float value)
        {
            value = 1 - value;

            r1 = value;
            r2 = value;

            resize();
            disk.rescale();
            outer.rescale();
        }

        public void changeValue(float value)
        {
            if (value < 0)
            {
                setValue(value + 1);
                disk.Color = Color.LightBlue;
                outer.Color = color;
            }
            else if (value <= 1)
            {
                setValue(value);
                disk.Color = color;
                outer.Color = Color.LightGreen;
            }

            rescale();
        }

        public void refract()
        {
            setValue(0);
            disk.Color = Color.IndianRed;
            outer.Color = Color.IndianRed;

            disk.refresh();
            outer.refresh();
        }

        public void refract(float factor)
        {
            int red = 205 - (int)(factor * 32);
            int green = 92 + (int)(factor * 124);
            int blue = 92 + (int)(factor * 138);

            disk.Color = new Color(red, green, blue);
        }

        public void resize(bool all = false)
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
                border.R2 = radius * 1.1f;
            }
        }

        public void activate()
        {
            border.Color = Color.RosyBrown;
        }

        public void idle()
        {
            border.Color = Color.Purple;
        }

        public void hover()
        {
            border.Color = Color.MediumSeaGreen;
        }

        public float Radius
        {
            set
            {
                radius = value;
                resize(true);
            }
        }
    }
}
