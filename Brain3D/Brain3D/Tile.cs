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

        protected static SpriteFont font;

        protected Rectangle recBackground;
        protected Rectangle recBorder;

        protected Texture2D texBackground;
        protected Texture2D texBorder;

        protected String name;
        protected Vector2 text;

        protected int width;
        protected int height;

        protected Tuple<Texture2D, Texture2D> active;
        protected Tuple<Texture2D, Texture2D> normal;

        protected static Tuple<Texture2D, Texture2D> texturesActive;
        protected static Tuple<Texture2D, Texture2D> texturesBuilt;
        protected static Tuple<Texture2D, Texture2D> texturesNormal;

        #endregion

        public Tile(String name)
        {
            this.name = name;

            Vector2 size = font.MeasureString(name);

            width = 16 + (int)size.X;
            height = 8 + (int)size.Y;
            text = new Vector2(8, 8);

            if (name.Length == 0)
                height += (int)font.MeasureString("0").Y;

            recBorder = new Rectangle(0, 0, width, height);
            recBackground = new Rectangle(0, 0, width - 8, height - 8);

            initialize();
            idle();
        }

        public static void initializeTextures()
        {
            font = content.Load<SpriteFont>("Sequence");

            texturesActive = getTextures(Color.LightSkyBlue, Color.IndianRed);
            texturesBuilt = getTextures(Color.GreenYellow, Color.Purple);
            texturesNormal = getTextures(Color.LightYellow, Color.Thistle);
            /*
            color[0] = Color.LightCyan;
            backgroundReceptor = new Texture2D(device, 1, 1);
            backgroundReceptor.SetData<Color>(color);

            color[0] = Color.Purple;
            borderReceptor = new Texture2D(device, 1, 1);
            borderReceptor.SetData<Color>(color);

            color[0] = Color.PaleVioletRed;
            backgroundActiveReceptor = new Texture2D(device, 1, 1);
            backgroundActiveReceptor.SetData<Color>(color);

            color[0] = Color.IndianRed;
            borderActiveReceptor = new Texture2D(device, 1, 1);
            borderActiveReceptor.SetData<Color>(color);*/
        }

        static Tuple<Texture2D, Texture2D> getTextures(Color backgroundColor, Color borderColor)
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

        #region grafika

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

        public override void draw()
        {
            batch.Draw(texBorder, recBorder, Color.White);
            batch.Draw(texBackground, recBackground, Color.White);
            batch.DrawString(font, name, text, Color.DarkSlateBlue);
        }

        #endregion

        #region właściwości

        public String Name
        {
            get
            {
                return name;
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
                recBackground.X = value + 4;
                text.X = value + 8;
            }
        }

        public int Right
        {
            get
            {
                return recBorder.Right;
            }
        }

        public int Top
        {
            set
            {
                recBorder.Y = value;
                recBackground.Y = value + 4;
                text.Y = value + 5;
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
