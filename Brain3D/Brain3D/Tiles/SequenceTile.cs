using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain3D
{
    class SequenceTile : Tile
    {
        public SequenceTile()
        { }

        public SequenceTile(String word)
        {
            this.word = word;
            prepare();
        }

        public SequenceTile(BuiltTile tile)
        {
            word = tile.Word;
            prepare();

            Left = tile.Left;
            Top = tile.Top;
        }
    }
}
