using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain3D
{
    class BuiltTile : Tile
    {
        StringBuilder builder;

        #region konstruktory

        public BuiltTile()
        {
            prepare(10);
        }

        public BuiltTile(int left)
        {
            prepare(left);
        }

        public BuiltTile(Tile tile) : base(tile.Word)
        {
            prepare(tile.Left);
        }

        void prepare(int left)
        {
            word = "";
            Prepare();

            builder = new StringBuilder(word);
            Left = left;
            Top = 8;
        }

        public override void Initialize()
        {
            active = texturesBuild;
            normal = texturesBuild;
        }

        #endregion

        #region logika

        public void add(char key)
        {
            builder.Append(key);
            rename();
        }

        public bool erase()
        {
            if (builder.Length == 0)
                return true;

            builder.Remove(builder.Length - 1, 1);
            rename();

            return false;
        }

        void rename()
        {
            word = builder.ToString();
            width = (int)Fonts.SpriteVerdana.MeasureString(word).X + 20;

            recBackground.Width = width - 6;
            recBorder.Width = width;
        }

        #endregion
    }
}
