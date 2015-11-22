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
            Reload();

            recBorder = new Rectangle(point.X, point.Y, width, height);
            recBackground = new Rectangle(point.X + 3, point.Y + 3, width - 6, height - 6);

            Initialize();
            Idle();
        }

        void Reload()
        {
            if (value < 0)
            {
                word = string.Empty;
            }
            else
            {
                word = value.ToString();
            }

            float x = (width - Fonts.SpriteVerdana.MeasureString(word).X) / 2;
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
                Reload();
            }
        }
    }
}
