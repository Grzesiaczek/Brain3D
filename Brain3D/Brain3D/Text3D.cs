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
        static TextBatch batch;
        static VectorFont font;

        protected Text text;
        protected Matrix matrix;
        protected Matrix constant;

        protected Vector3 shift;
        protected float scale;

        public Text3D(String data, Vector3 position)
        {
            this.position = position;

            text = font.Fill(data);
            color = Color.DarkSlateBlue;

            scale = 0.07f;
            shift = new Vector3(-text.Width / 2, -text.Height * 0.3f, 0);
            constant = Matrix.CreateTranslation(shift) * Matrix.CreateScale(scale) * Matrix.CreateRotationY((float)Math.PI);

            refresh();
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
            batch.Begin();
            batch.DrawText(text, matrix, color);
            batch.End();
        }

        public String Text
        {
            set
            {
                text = font.Fill(value);
                shift = new Vector3(-text.Width / 2, -text.Height * 0.3f, 0);
                constant = Matrix.CreateTranslation(shift) * Matrix.CreateScale(scale) * Matrix.CreateRotationY((float)Math.PI);
                refresh();
            }
        }

        public override Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
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
