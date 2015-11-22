using System;
using System.Linq;

namespace Brain3D
{
    class ResponseSequence : Sequence
    {
        Melody melody;

        bool canPlay;

        public void Reload()
        {
            try
            {
                melody = new Melody(sequence.Select(t => t.Word).ToList());
                canPlay = true;
            }
            catch(Exception)
            {
                canPlay = false;
            }
        }

        public void Play(Player player)
        {
            if (canPlay)
            {
                player.Play(melody);
            }
        }

        public void Clear()
        {
            //TODO
        }
    }
}
