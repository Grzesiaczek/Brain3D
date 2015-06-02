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

        public override void draw()
        {
            foreach (SpriteElement sprite in sprites)
                sprite.draw();
        }

        public override void show()
        {
            foreach (SpriteElement sprite in sprites)
                sprite.show();
        }

        public override void hide()
        {
            foreach (SpriteElement sprite in sprites)
                sprite.hide();
        }
    }
}
