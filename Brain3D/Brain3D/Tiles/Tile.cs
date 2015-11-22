using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    abstract class Tile : SpriteElement
    {
        #region deklaracje

        enum TileMode { Special, Standard }

        protected Rectangle recBackground;
        protected Rectangle recBorder;

        protected Texture2D texBackground;
        protected Texture2D texBorder;

        protected string word;
        protected Vector2 text;

        protected Tuple<Texture2D, Texture2D> active;
        protected Tuple<Texture2D, Texture2D> normal;

        protected int width;
        protected int height;

        protected static Tuple<Texture2D, Texture2D> texturesActive;
        protected static Tuple<Texture2D, Texture2D> texturesBuild;
        protected static Tuple<Texture2D, Texture2D> texturesNeuron;
        protected static Tuple<Texture2D, Texture2D> texturesNormal;
        protected static Tuple<Texture2D, Texture2D> texturesRefract;
        protected static Tuple<Texture2D, Texture2D> texturesShoot;
        protected static Tuple<Texture2D, Texture2D> texturesActive2;
        protected static Tuple<Texture2D, Texture2D> texturesNormal2;

        TileMode mode;

        #endregion

        public Tile()
        {
            mode = TileMode.Standard;
        }

        public Tile(string name)
        {
            word = name;
            Prepare();            
        }

        protected void Prepare()
        {
            Vector2 size = Fonts.SpriteVerdana.MeasureString(word);
            mode = TileMode.Standard;

            width = 20 + (int)size.X;
            height = 12 + (int)size.Y;
            text = new Vector2(10, 11);

            if (word.Length == 0)
            {
                height += (int)Fonts.SpriteVerdana.MeasureString("0").Y;
            }

            recBorder = new Rectangle(0, 0, width, height);
            recBackground = new Rectangle(0, 0, width - 6, height - 6);

            Initialize();
            Idle();
        }

        public static void InitializeTextures()
        {
            texturesActive = GetTextures(Color.RosyBrown, Color.IndianRed);
            texturesBuild = GetTextures(Color.GreenYellow, Color.Purple);
            texturesNeuron = GetTextures(Color.LightGreen, Color.DarkGoldenrod);
            texturesNormal = GetTextures(Color.LightYellow, Color.DarkSlateBlue);
            texturesRefract = GetTextures(Color.MediumVioletRed, Color.SkyBlue);
            texturesShoot = GetTextures(Color.Azure, Color.IndianRed);
            texturesActive2 = GetTextures(Color.MediumPurple, Color.IndianRed);
            texturesNormal2 = GetTextures(Color.LightGreen, Color.DarkSlateBlue);
        }

        protected static Tuple<Texture2D, Texture2D> GetTextures(Color backgroundColor, Color borderColor)
        {
            Texture2D background = GetTexture(backgroundColor);
            Texture2D border = GetTexture(borderColor);

            return new Tuple<Texture2D, Texture2D>(background, border);
        }

        static Texture2D GetTexture(Color color)
        {
            Texture2D texture;
            Color[] colors = new Color[1];
            colors[0] = color;

            texture = new Texture2D(device, 1, 1);
            texture.SetData(colors);
            return texture;
        }

        public void Activate()
        {
            texBackground = active.Item1;
            texBorder = active.Item2;
        }

        public void Idle()
        {
            texBackground = normal.Item1;
            texBorder = normal.Item2;
        }

        public void Switch()
        {
            if (mode == TileMode.Standard)
            {
                mode = TileMode.Special;
            }
            else
            {
                mode = TileMode.Special;
            }

            Initialize();
            Activate();
        }

        public virtual void Initialize()
        {
            if (mode == TileMode.Standard)
            {
                active = texturesActive;
                normal = texturesNormal;
            }
            else
            {
                active = texturesActive2;
                normal = texturesNormal2;
            }
        }

        public override bool Cursor(int x, int y)
        {
            if (x < Left)
            {
                return false;
            }

            if (x > Right)
            {
                return false;
            }

            if (y < Top)
            {
                return false;
            }

            if (y > Bottom)
            {
                return false;
            }

            return true;
        }

        public override void Draw()
        {
            batch.Draw(texBorder, recBorder, Color.White);
            batch.Draw(texBackground, recBackground, Color.White);
            batch.DrawString(Fonts.SpriteVerdana, word, text, Color.Black);
        }

        #region właściwości

        public string Word
        {
            get
            {
                return word;
            }
        }

        public int Left
        {
            get
            {
                return recBorder.Left;
            }
            set
            {
                recBorder.X = value;
                recBackground.X = value + 3;
                text.X = value + 10;
            }
        }

        public int Right
        {
            get
            {
                return recBorder.Right;
            }
        }

        public virtual int Top
        {
            get
            {
                return recBorder.Top;
            }
            set
            {
                recBorder.Y = value;
                recBackground.Y = value + 3;
                text.Y = value + 7;
            }
        }

        public int Bottom
        {
            get
            {
                return recBorder.Bottom;
            }
        }

        #endregion
    }
}
