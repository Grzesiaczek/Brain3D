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

        public BuiltTile(String name) : base(name)
        {
            builder = new StringBuilder(name);
        }

        public BuiltTile(Tile element) : base(element.Name)
        {
            builder = new StringBuilder(name);
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
            builder.Remove(builder.Length - 1, 1);
            rename();

            if (builder.Length == 0)
                return true;

            return false;
        }

        void rename()
        {
            name = builder.ToString();
            width = 16 + (int)font.MeasureString(name).X;

            recBackground.Width = width - 8;
            recBorder.Width = width;
        }

        #endregion
    }
}
