using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain3D
{
    class SpriteComposite : SpriteElement
    {
        protected List<SpriteElement> sprites;

        public SpriteComposite()
        {
            sprites = new List<SpriteElement>();
        }

        public override void Draw()
        {
            foreach (SpriteElement sprite in sprites)
                sprite.Draw();
        }

        public override void Show()
        {
            foreach (SpriteElement sprite in sprites)
                sprite.Show();
        }

        public override void Hide()
        {
            foreach (SpriteElement sprite in sprites)
                sprite.Hide();
        }
    }
}
