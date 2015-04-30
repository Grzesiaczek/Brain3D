using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class Text2D : DrawableElement
    {
        static SpriteBatch batch;

        SpriteFont font;
        String text;

        Vector2 location;
        Vector2 corner;

        int width;

        public Text2D(String text, SpriteFont font, Vector2 location, Color color, int width = 0)
        {
            this.text = text;
            this.font = font;
            this.location = location;
            this.color = color;
            this.width = width;

            corner = location;
        }

        public override void draw()
        {
            batch.Begin();
            batch.DrawString(font, text, corner, color);
            batch.End();
        }

        public String Text
        {
            set
            {
                text = value;

                if (width == 0)
                    return;

                corner = location;
                corner.X += width - font.MeasureString(text).X / 2;
            }
        }

        public Vector2 Location
        {
            set
            {
                corner += value - location;
                location = value;
            }
        }

        public static SpriteBatch Batch
        {
            set
            {
                batch = value;
            }
        }
    }
}
