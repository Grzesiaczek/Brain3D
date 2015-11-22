using System;
using System.Collections.Generic;
using System.Timers;
using Sanford.Multimedia.Midi;

namespace Brain3D
{
    public class Melody
    {
        Dictionary<int, HashSet<Sound>> start;
        Dictionary<int, HashSet<Sound>> finish;

        Timer timer;
        OutputDevice device;
        object locker = new object();

        int time;
        int length;

        public Melody(List<string> melody)
        {
            start = new Dictionary<int, HashSet<Sound>>();
            finish = new Dictionary<int, HashSet<Sound>>();

            time = 0;

            foreach (string s in melody)
            {
                Sound sound = new Sound(s);

                if (!start.ContainsKey(time))
                {
                    start.Add(time, new HashSet<Sound>());
                }

                start[time].Add(sound);
                time += sound.Length;

                if (!finish.ContainsKey(time))
                {
                    finish.Add(time, new HashSet<Sound>());
                }

                finish[time].Add(sound);
            }

            length = time;

            timer = new Timer();
            timer.Interval = 64;
            timer.Elapsed += new ElapsedEventHandler(Tick);
        }

        public void Play(OutputDevice device)
        {
            this.device = device;
            time = 0;
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        void Tick(object sender, ElapsedEventArgs e)
        {
            lock (locker)
            {
                if (finish.ContainsKey(time))
                {
                    foreach (Sound sound in finish[time])
                    {
                        KeyUp(sound.Code);
                    }
                }

                if (start.ContainsKey(time))
                {
                    foreach (Sound sound in start[time])
                    {
                        KeyDown(sound.Code);
                    }
                }

                if (time++ == length)
                {
                    Stop();
                }
            }
        }

        private void KeyDown(int key)
        {
            device.Send(new ChannelMessage(ChannelCommand.NoteOn, 0, key, 127));
        }

        private void KeyUp(int key)
        {
            device.Send(new ChannelMessage(ChannelCommand.NoteOff, 0, key, 0));
        }
    }
}
