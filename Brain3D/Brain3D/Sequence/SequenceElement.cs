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
    abstract class SequenceElement : DrawableElement
    {
        #region deklaracje

        protected static SpriteBatch batch;
        protected static SpriteFont font;

        protected Rectangle recBackground;
        protected Rectangle recBorder;

        protected Texture2D texBackground;
        protected Texture2D texBorder;

        protected String name;
        protected Vector2 text;

        protected int width;
        protected int height;

        static Texture2D backgroundActive;
        static Texture2D backgroundBuilt;
        static Texture2D backgroundNormal;
        static Texture2D backgroundReceptor;
        static Texture2D backgroundActiveReceptor;

        static Texture2D borderActive;
        static Texture2D borderBuilt;
        static Texture2D borderNormal;
        static Texture2D borderReceptor;
        static Texture2D borderActiveReceptor;

        #endregion

        public SequenceElement(String name)
        {
            this.name = name;

            Vector2 size = font.MeasureString(name);
            color = Color.DarkSlateBlue;

            width = 16 + (int)size.X;
            height = 8 + (int)size.Y;
            text = new Vector2(8, 8);

            if (name.Length == 0)
                height += (int)font.MeasureString("0").Y;

            recBorder = new Rectangle(0, 0, width, height);
            recBackground = new Rectangle(0, 0, width - 8, height - 8);
        }

        public static void initialize()
        {
            batch = new SpriteBatch(device);
            font = content.Load<SpriteFont>("Sequence");

            Color[] color = new Color[1];

            color[0] = Color.LightSkyBlue;
            backgroundActive = new Texture2D(device, 1, 1);
            backgroundActive.SetData<Color>(color);

            color[0] = Color.IndianRed;
            borderActive = new Texture2D(device, 1, 1);
            borderActive.SetData<Color>(color);

            color[0] = Color.GreenYellow;
            backgroundBuilt = new Texture2D(device, 1, 1);
            backgroundBuilt.SetData<Color>(color);

            color[0] = Color.Purple;
            borderBuilt = new Texture2D(device, 1, 1);
            borderBuilt.SetData<Color>(color);

            color[0] = Color.LightYellow;
            backgroundNormal = new Texture2D(device, 1, 1);
            backgroundNormal.SetData<Color>(color);

            color[0] = Color.Thistle;
            borderNormal = new Texture2D(device, 1, 1);
            borderNormal.SetData<Color>(color);

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
            borderActiveReceptor.SetData<Color>(color);
        }

        #region grafika

        public void changeType(SequenceElementType type)
        {
            switch(type)
            {
                case SequenceElementType.Activated:

                    break;

                case SequenceElementType.Active:
                    texBackground = backgroundActive;
                    texBorder = borderActive;
                    break;

                case SequenceElementType.Built:
                    texBackground = backgroundBuilt;
                    texBorder = borderBuilt;
                    break;

                case SequenceElementType.Normal:
                    texBackground = backgroundNormal;
                    texBorder = borderNormal;
                    break;

                case SequenceElementType.Receptor:
                    texBackground = backgroundReceptor;
                    texBorder = borderReceptor;
                    break;

                case SequenceElementType.ActiveReceptor:
                    texBackground = backgroundActiveReceptor;
                    texBorder = borderActiveReceptor;
                    break;
            }
        }

        public override void draw()
        {
            batch.Begin();
            batch.Draw(texBorder, recBorder, Color.White);
            batch.Draw(texBackground, recBackground, Color.White);
            batch.DrawString(font, name, text, Color.DarkSlateBlue);
            batch.End();
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
                return recBorder.X;
            }
            set
            {
                recBorder.X = value;
                recBackground.X = value + 4;
                text.X = value + 8;
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

        public int Right
        {
            get
            {
                return recBorder.Right;
            }
        }

        #endregion
    }
}
