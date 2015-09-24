using System;
using System.IO;
using AudioSynthesis.Synthesis;
using NAudio.Wave;

namespace SoundProvider
{
    public sealed class SynthThread : IDisposable
    {
        private Synthesizer synth;
        private DirectSoundOut direct_out;
        private SynthWaveProvider provider;

        int samplerate = 44100;
        int latency = 100;
        int buffersize = 441;
        int buffercount = 3;
        int poly = 100;

        public SynthThread()
        {
            synth = new Synthesizer(samplerate, 2, buffersize, buffercount, poly);
            provider = new SynthWaveProvider(synth);
            direct_out = new DirectSoundOut(latency);
            direct_out.Init(provider);
        }

        public void LoadBank(string bankfile)
        {
            Stop();
            synth.LoadBank(new MyFile(bankfile));
        }

        public void Play()
        {
            if (synth.SoundBank == null)
                return;

            direct_out.Play();
        }

        public void Stop()
        {
            lock (provider.locker)
            {
                synth.NoteOffAll(true);
                synth.ResetSynthControls();
                synth.ResetPrograms();
                provider.Reset();
            }
        }

        public void AddMessage(SynthWaveProvider.Message msg)
        {
            lock (provider.locker)
            {
                provider.msgQueue.Enqueue(msg);
            }
        }

        public void Close()
        {
            Stop();
            direct_out.Stop();
            direct_out.Dispose();
            synth.UnloadBank();
        }

        public void Dispose()
        {
            Close();
        }
    }
}
