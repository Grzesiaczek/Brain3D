using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class Branch : SpriteElement
    {
        static Texture2D texture;

        Leaf source;
        Leaf target;

        Rectangle rectangle;
        float angle;

        public Branch(Leaf source, Leaf target)
        {
            this.source = source;
            this.target = target;
            initialize();            
        }

        public static void initializeTexture()
        {
            Color[] color = new Color[1];
            color[0] = Color.LightSkyBlue;

            texture = new Texture2D(device, 1, 1);
            texture.SetData<Color>(color);
        }

        public void initialize()
        {
            Vector2 line = new Vector2((float)(target.X - source.X), (float)(target.Y - source.Y));
            float length = line.Length();
            angle = (float)Math.Asin((double)line.Y / length);
            rectangle = new Rectangle(source.X, source.Y, (int)length, 2);
        }

        public override void draw()
        {
            batch.Draw(texture, rectangle, null, Color.Red, angle, new Vector2(0, 0), SpriteEffects.None, 0);
        }
    }
}
