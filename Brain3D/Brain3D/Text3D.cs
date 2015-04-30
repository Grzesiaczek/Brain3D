using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Fonts;
using Nuclex.Graphics;

namespace Brain3D
{
    class Text3D : DrawableElement
    {
        protected static TextBatch batch;
        protected static VectorFont font;

        protected Text text;
        protected Matrix matrix;
        protected Matrix constant;

        protected Vector3 shift;
        protected float scale;

        bool suppress;

        protected Text3D() { }

        public Text3D(String data, Vector3 position)
        {
            this.position = position;
            color = Color.DarkSlateBlue;
            scale = 0.07f;
            Text = data;
        }

        public static void initialize()
        {
            batch = new TextBatch(device);
            font = content.Load<VectorFont>("Standard");
        }

        public override void refresh()
        {
            matrix = constant * camera.Rotation * Matrix.CreateTranslation(position);
        }

        public override void draw()
        {
            if (suppress)
                return;

            batch.Begin();
            batch.DrawText(text, matrix, color);
            batch.End();
        }

        public String Text
        {
            set
            {
                text = font.Fill(value);
                shift = new Vector3(-text.Width / 2, 0, 0);
                constant = Matrix.CreateTranslation(shift) * Matrix.CreateScale(scale) * Matrix.CreateRotationY((float)Math.PI);
                refresh();
            }
        }

        public bool Suppress
        {
            set
            {
                suppress = value;
            }
        }

        public static Matrix ViewProjection
        {
            set
            {
                batch.ViewProjection = value;
            }
        }
    }
}
