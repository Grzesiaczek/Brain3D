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
        Circle outerCircle;
        Circle changeCircle;

        Ring change;
        Ring outer;

        float value;

        public StateDisk(Vector3 position, float radius)
        {
            this.position = position;
            this.radius = radius;
            value = 0;

            innerCircle = new Circle(position, radius, 1);
            outerCircle = new Circle(position, radius, 1);
            borderCircle = new Circle(position, radius * 1.1f, 1);

            disk = new Disk(position, innerCircle, Color.LightYellow);
            outer = new Ring(innerCircle, outerCircle, Color.LightGreen);
            border = new Ring(outerCircle, borderCircle, Color.Purple);

            if(radius < 0.8f)
            {
                changeCircle = new Circle(position, radius, 1);
                change = new Ring(innerCircle, changeCircle, Color.Green);
                outer = new Ring(changeCircle, outerCircle, Color.LightGreen);

                drawables.Add(change);
                elements.Add(changeCircle);
            }

            elements.Add(innerCircle);
            elements.Add(outerCircle);
            elements.Add(borderCircle);

            drawables.Add(disk);
            drawables.Add(outer);
            drawables.Add(border);
        }

        public void setChange(CreationData data)
        {
            if(data.Change < 0)
            {
                change.Color = Color.IndianRed;
                changeCircle.Radius = radius - radius * data.Start;
            }
            else
            {
                change.Color = Color.Green;
                innerCircle.Radius = radius - radius * data.Weight;
            }

            changeRefresh();
        }

        public void setValue(float value)
        {
            innerCircle.Radius = radius * (1 - value);
            changeCircle.Radius = innerCircle.Radius;

            this.value = value;
            changeRefresh();
        }

        void changeRefresh()
        {
            innerCircle.refresh();
            changeCircle.refresh();

            disk.refresh();
            change.refresh();
            outer.refresh();
        }

        public void changeValue(float value)
        {
            this.value = value;

            if (value < 0)
            {
                innerCircle.Radius = -value * radius;
                disk.Color = Color.LightBlue;
                outer.Color = Color.LightYellow;
            }
            else if (value < 1)
            {
                innerCircle.Radius = radius * (1 - value);
                disk.Color = Color.LightYellow;
                outer.Color = Color.LightGreen;
            }

            innerCircle.refresh();
            disk.refresh();
            outer.refresh();
        }

        public void refract()
        {
            innerCircle.Radius = radius;
            disk.Color = Color.IndianRed;
            outer.Color = Color.LightYellow;

            innerCircle.refresh();
            disk.refresh();
            outer.refresh();
        }

        public void refract(float factor)
        {
            int red = 205 - (int)(factor * 32);
            int green = 92 + (int)(factor * 124);
            int blue = 92 + (int)(factor * 138);

            disk.Color = new Color(red, green, blue);
            disk.refresh();
        }
    }
}
