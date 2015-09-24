using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brain3D
{
    class ResponseSequence : Sequence
    {
        Melody melody;

        public void Reload()
        {
            melody = new Melody(sequence.Select(t => t.Word).ToList());
        }

        public void Play(Player player)
        {
            player.Play(melody);
        }

        public void Clear()
        {
            //TODO
        }
    }
}
