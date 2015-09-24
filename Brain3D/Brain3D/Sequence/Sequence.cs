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

        public void Add(Tile element)
        {
            sequence.Add(element);
            element.Show();
        }

        public void Remove(Tile element)
        {
            sequence.Remove(element);
            element.Hide();
        }

        public override void Show()
        {
            foreach (Tile tile in sequence)
                tile.Show();
        }

        public override void Hide()
        {
            foreach (Tile tile in sequence)
                tile.Hide();
        }

        protected virtual void Arrange()
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

        public void Add(char key)
        {
            if (builder == null)
                return;

            builder.add(key);
        }

        public virtual void Space()
        {
            if (builder.Word.Length == 0)
                return;

            Remove(builder);
            Add(CreateTile(builder));

            builder = new BuiltTile(builder.Right + 10);
            Add(builder);
        }

        public bool Erase()
        {
            if (!builder.erase())
                return false;

            if (sequence.Count == 1)
                return true;

            Remove(builder);

            Tile last = sequence.Last();
            Remove(last);

            builder = new BuiltTile(last);
            Add(builder);

            return false;
        }

        public virtual bool Execute()
        {
            if (sequence.Count == 1 && builder.Word.Length == 0)
                return false;

            Remove(builder);
            Add(CreateTile(builder));
            builder = null;

            return true;
        }

        protected virtual Tile CreateTile(BuiltTile builder)
        {
            return new SequenceTile(builder);
        }

        #endregion
    }
}
