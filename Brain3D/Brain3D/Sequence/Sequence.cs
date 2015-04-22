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
    class Sequence : CompositeElement
    {
        protected List<SequenceElement> sequence;
        protected BuiltElement builder;
        protected int position = 10;

        public Sequence()
        {
            sequence = new List<SequenceElement>();
        }

        #region logika

        public void add(SequenceElement element)
        {
            sequence.Add(element);
        }

        public virtual void clear()
        {
            sequence.Clear();
        }

        public override void draw()
        {
            foreach (SequenceElement element in sequence)
                element.draw();

            if(builder != null)
                builder.draw();
        }

        public void arrange()
        {
            int position = 10;

            foreach(SequenceElement element in sequence)
            {
                element.Top = 8;
                element.Left = position;
                position = element.Right + 10;
            }
        }

        #endregion

        #region budowa

        public void add(char key)
        {
            if (builder == null)
            {
                builder = new BuiltElement("");
                builder.Left = position;
                builder.Top = 8;
            }

            builder.add(key);
        }

        public bool erase()
        {
            if (!builder.erase())
                return false;

            if (sequence.Count == 0)
                return true;

            SequenceElement last = sequence.Last<SequenceElement>();
            builder = new BuiltElement(last);
            sequence.Remove(last);

            return false;
        }

        #endregion

        #region właściwości

        public int Count
        {
            get
            {
                return sequence.Count;
            }
        }

        #endregion
    }
}
