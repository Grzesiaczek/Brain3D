using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    abstract class Tile : SpriteElement
    {
        #region deklaracje

        protected Rectangle recBackground;
        protected Rectangle recBorder;

        protected Texture2D texBackground;
        protected Texture2D texBorder;

        protected String word;
        protected Vector2 text;

        protected Tuple<Texture2D, Texture2D> active;
        protected Tuple<Texture2D, Texture2D> normal;

        protected int width;
        protected int height;

        protected static Tuple<Texture2D, Texture2D> texturesActive;
        protected static Tuple<Texture2D, Texture2D> texturesBuilt;
        protected static Tuple<Texture2D, Texture2D> texturesNeuron;
        protected static Tuple<Texture2D, Texture2D> texturesNormal;
        protected static Tuple<Texture2D, Texture2D> texturesRefract;
        protected static Tuple<Texture2D, Texture2D> texturesShot;

        #endregion

        public Tile() { }

        public Tile(String name)
        {
            this.word = name;
            prepare();            
        }

        protected void prepare()
        {
            Vector2 size = Fonts.SpriteVerdana.MeasureString(word);

            width = 20 + (int)size.X;
            height = 12 + (int)size.Y;
            text = new Vector2(10, 11);

            if (word.Length == 0)
                height += (int)Fonts.SpriteVerdana.MeasureString("0").Y;

            recBorder = new Rectangle(0, 0, width, height);
            recBackground = new Rectangle(0, 0, width - 6, height - 6);

            initialize();
            idle();
        }

        public static void initializeTextures()
        {
            texturesActive = getTextures(Color.RosyBrown, Color.IndianRed);
            texturesBuilt = getTextures(Color.GreenYellow, Color.Purple);
            texturesNeuron = getTextures(Color.LightGreen, Color.DarkGoldenrod);
            texturesNormal = getTextures(Color.LightYellow, Color.DarkSlateBlue);
            texturesRefract = getTextures(Color.MediumVioletRed, Color.SkyBlue);
            texturesShot = getTextures(Color.Azure, Color.IndianRed);
        }

        protected static Tuple<Texture2D, Texture2D> getTextures(Color backgroundColor, Color borderColor)
        {
            Color[] color = new Color[1];
            Texture2D background;
            Texture2D border;

            color[0] = backgroundColor;
            background = new Texture2D(device, 1, 1);
            background.SetData<Color>(color);

            color[0] = borderColor;
            border = new Texture2D(device, 1, 1);
            border.SetData<Color>(color);

            return new Tuple<Texture2D, Texture2D>(background, border);
        }

        public void activate()
        {
            texBackground = active.Item1;
            texBorder = active.Item2;
        }

        public void idle()
        {
            texBackground = normal.Item1;
            texBorder = normal.Item2;
        }

        public virtual void initialize()
        {
            active = texturesActive;
            normal = texturesNormal;
        }

        public override bool cursor(int x, int y)
        {
            if (x < Left)
                return false;

            if (x > Right)
                return false;

            if (y < Top)
                return false;

            if (y > Bottom)
                return false;

            return true;
        }

        public override void draw()
        {
            batch.Draw(texBorder, recBorder, Color.White);
            batch.Draw(texBackground, recBackground, Color.White);
            batch.DrawString(Fonts.SpriteVerdana, word, text, Color.Black);
        }

        #region właściwości

        public String Word
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

        public int X
        {
            get
            {
                return recBorder.Center.X;
            }
        }

        public int Y
        {
            get
            {
                return recBorder.Center.Y;
            }
        }

        #endregion
    }
}
