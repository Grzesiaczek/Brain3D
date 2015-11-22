using System;
using System.Collections.Generic;
using System.Linq;
using Sanford.Multimedia.Midi;

namespace Brain3D
{
    class Player
    {
        private OutputDevice device;
        private Melody melody;

        public Player()
        {
            device = new OutputDevice(0);
        }

        public void Play(Melody melody)
        {
            Stop();
            this.melody = melody;
            melody.Play(device);
        }

        public void Play(String seq)
        {
            List<string> data = seq.Split(' ').ToList();
            Play(new Melody(data));
        }

        public void Stop()
        {
            if (melody != null)
            {
                melody.Stop();
            }
        }

        public void SetInstrument(int code)
        {
            if (code > -1 && code < 128)
            {
                device.Close();
                device = new OutputDevice(code);
            }
        }

        public void Close()
        {
            device.Close();
        }
    }
}
