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

        bool initialized;

        public StateDisk(Vector3 position, Color color, float radius)
        {
            this.position = position;
            this.color = color;
            this.radius = radius;

            disk = new Disk(position, framework, color, radius);
            outer = new Ring(position, framework, Color.LightGreen);
            border = new Ring(position, framework, Color.Purple);

            if(radius < 0.8f)
            {
                change = new Ring(position, framework, Color.Green);
                outer = new Ring(position, framework, Color.LightGreen);

                drawables.Add(change);
            }

            border.R1 = radius;
            border.R2 = radius * 1.1f;

            outer.R1 = radius;
            outer.R2 = radius;

            drawables.Add(disk);
            drawables.Add(outer);
            drawables.Add(border);
        }

        public void setChange(CreationData data)
        {
            float r1 = 0;
            float r2 = 0;

            if(data.Change < 0)
            {
                change.Color = Color.IndianRed;
                r1 = radius - radius * data.Weight;
                r2 = radius - radius * data.Start;
            }
            else
            {
                change.Color = Color.Green;
                r1 = radius - radius * data.Start;
                r2 = radius - radius * data.Weight;
            }

            setChange(r1, r2);
        }

        void setChange(float r1, float r2)
        {
            disk.Radius = r1;
            change.R1 = r1;

            change.R2 = r2;
            outer.R1 = r2;

            changeRefresh();
        }

        public void setValue(float value)
        {
            value = radius * (1 - value);
            //setChange(value, value);

            disk.Radius = value;
            outer.R1 = value;
        }

        void changeRefresh()
        {
            if (initialized)
            {
                disk.move();
                change.move();
                outer.move();
            }
        }

        public void changeValue(float value)
        {
            if (value < 0)
            {
                setValue(value + 1);
                disk.Color = Color.LightBlue;
                outer.Color = color;
            }
            else if (value < 1)
            {
                setValue(value);
                disk.Color = color;
                outer.Color = Color.LightGreen;
            }

            disk.refresh();
            outer.refresh();
        }

        public void refract()
        {
            setValue(0);
            disk.Color = Color.IndianRed;
            outer.Color = color;

            disk.refresh();
            outer.refresh();
        }

        public void refract(float factor)
        {
            int red = 205 - (int)(factor * 32);
            int green = 92 + (int)(factor * 124);
            int blue = 92 + (int)(factor * 138);

            disk.Color = new Color(red, green, blue);
            disk.repaint();
        }

        public override void initialize()
        {
            base.initialize();
            initialized = true;
        }
    }
}
