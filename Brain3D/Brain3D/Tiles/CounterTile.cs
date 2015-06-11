using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Brain3D
{
    class CounterTile : Tile
    {
        Point point;
        int value;

        public CounterTile(Point point, int value)
        {
            this.point = point;
            this.value = value;

            width = 60;
            height = 32;
            reload();

            recBorder = new Rectangle(point.X, point.Y, width, height);
            recBackground = new Rectangle(point.X + 3, point.Y + 3, width - 6, height - 6);

            initialize();
            idle();
        }

        void reload()
        {
            if (value < 0)
                word = "";
            else
                word = value.ToString();

            float x = ((float)width - Fonts.SpriteVerdana.MeasureString(word).X) / 2;
            text = new Vector2(point.X + x, point.Y + 6);
        }

        public override int Top
        {
            set
            {
                point.Y = value;
                recBorder.Y = value;
                recBackground.Y = value + 3;
                text.Y = value + 6;
            }
        }

        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                reload();
            }
        }
    }
}
