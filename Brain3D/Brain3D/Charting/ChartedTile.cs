using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class ChartedTile : Tile
    {
        Tuple<Texture2D, Texture2D> tex;

        public ChartedTile(String name, Point corner, Color color)
        {
            this.word = name;
            Vector2 size = font.MeasureString(name);

            width = 100;
            height = 40;

            text = new Vector2(corner.X + 50 - size.X / 2, corner.Y + 12);

            recBorder = new Rectangle(corner.X, corner.Y, width, height);
            recBackground = new Rectangle(corner.X + 3, corner.Y + 3, width - 6, height - 6);

            active = getTextures(color, Color.DarkBlue);
            normal = getTextures(color, Color.Purple);
            tex = getTextures(color, Color.Wheat);
        }
    
        public void hover()
        {
            texBackground = tex.Item1;
            texBorder = tex.Item2;
        }

        public override int Top
        {
            set
            {
                recBorder.Y = value;
                recBackground.Y = value + 3;
                text.Y = value + 9;
            }
        }
    }
}
