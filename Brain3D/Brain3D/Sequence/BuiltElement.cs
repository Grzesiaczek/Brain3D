using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain3D
{
    class BuiltElement : SequenceElement
    {
        StringBuilder builder;

        #region konstruktory

        public BuiltElement(String name) : base(name)
        {
            changeType(SequenceElementType.Built);
            builder = new StringBuilder(name);
        }

        public BuiltElement(SequenceElement element) : base(element.Name)
        {
            changeType(SequenceElementType.Built);
            builder = new StringBuilder(name);
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
