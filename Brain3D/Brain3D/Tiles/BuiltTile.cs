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
            builder = new StringBuilder(word);
            Left = left;
            Top = 10;
        }

        public override void initialize()
        {
            active = texturesBuilt;
            normal = texturesBuilt;
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
            width = (int)font.MeasureString(word).X + 20;

            recBackground.Width = width - 8;
            recBorder.Width = width;
        }

        #endregion
    }
}
