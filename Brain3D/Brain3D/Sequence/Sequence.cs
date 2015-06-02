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
    class Sequence : SpriteComposite
    {
        protected List<Tile> sequence;
        protected BuiltTile builder;

        public Sequence()
        {
            sequence = new List<Tile>();
        }

        #region logika

        public void add(Tile element)
        {
            sequence.Add(element);
            element.show();
        }

        public void remove(Tile element)
        {
            sequence.Remove(element);
            element.hide();
        }

        public override void show()
        {
            foreach (Tile tile in sequence)
                tile.show();
        }

        public override void hide()
        {
            foreach (Tile tile in sequence)
                tile.hide();
        }

        public void arrange()
        {
            int position = 10;

            foreach (Tile element in sequence)
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
                return;

            builder.add(key);
        }

        public virtual void space()
        {
            if (builder.Word.Length == 0)
                return;

            remove(builder);
            add(createTile(builder));

            builder = new BuiltTile(builder.Right + 10);
            add(builder);
        }

        public bool erase()
        {
            if (!builder.erase())
                return false;

            if (sequence.Count == 1)
                return true;

            remove(builder);

            Tile last = sequence.Last();
            remove(last);

            builder = new BuiltTile(last);
            add(builder);

            return false;
        }

        public bool execute()
        {
            if (sequence.Count == 1 && builder.Word.Length == 0)
                return false;

            remove(builder);
            add(createTile(builder));
            builder = null;

            return true;
        }

        protected Tile createTile(BuiltTile builder)
        {
            return new SequenceTile(builder);
        }

        #endregion
    }
}
