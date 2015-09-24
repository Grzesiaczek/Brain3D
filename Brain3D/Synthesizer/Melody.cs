using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SoundProvider
{
    public class Melody
    {
        Dictionary<int, HashSet<Sound>> start;
        Dictionary<int, HashSet<Sound>> finish;

        SynthThread synth;
        Timer timer;

        int time;
        int length;

        public Melody(SynthThread synth, List<String> melody)
        {
            start = new Dictionary<int, HashSet<Sound>>();
            finish = new Dictionary<int, HashSet<Sound>>();

            this.synth = synth;
            time = 0;

            foreach(String s in melody)
            {
                Sound sound = new Sound(s);

                if (!start.ContainsKey(time))
                    start.Add(time, new HashSet<Sound>());

                start[time].Add(sound);
                time += sound.Length;

                if (!finish.ContainsKey(time))
                    finish.Add(time, new HashSet<Sound>());

                finish[time].Add(sound);
            }

            length = time;

            timer = new Timer();
            timer.Interval = 64;
        }

        public void play()
        {
            time = 0;
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(tick);
        }

        public void stop()
        {
            timer.Stop();
        }

        void tick(object sender, ElapsedEventArgs e)
        {
            if(start.ContainsKey(time))
                foreach (Sound sound in start[time])
                    keyDown(sound.Code);

            if(finish.ContainsKey(time))
                foreach (Sound sound in finish[time])
                    keyUp(sound.Code);

            if (time++ == length)
                stop();
        }

        private void keyDown(int key)
        {
            SynthWaveProvider.Message msg = new SynthWaveProvider.Message();
            msg.channel = 0;
            msg.command = 0x90;
            msg.data1 = key;
            msg.data2 = 127;
            synth.AddMessage(msg);
        }

        private void keyUp(int key)
        {
            SynthWaveProvider.Message msg = new SynthWaveProvider.Message();
            msg.channel = 0;
            msg.command = 0x80;
            msg.data1 = key;
            msg.data2 = 64;
            synth.AddMessage(msg);
        }
    }
}
