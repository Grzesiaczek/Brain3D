using System;
using System.Collections.Generic;
using NAudio.Wave;
using NAudio.Utils;
using AudioSynthesis.Synthesis;

namespace SoundProvider
{
    public class SynthWaveProvider : IWaveProvider
    {

        public struct Message
        {
            public int channel;
            public int command;
            public int data1;
            public int data2;
        }
 
        public Queue<Message> msgQueue = new Queue<Message>();
        public volatile Object locker = new Object();
        private CircularBuffer circularBuffer;
        private WaveFormat waveFormat;
        private byte[] sbuff;
        private Synthesizer synth;

        public SynthWaveProvider(Synthesizer synth)
        {
            this.synth = synth;
            waveFormat = new WaveFormat(synth.SampleRate, 16, synth.AudioChannels);
            int bufferSize = (int)Math.Ceiling((2.0 * waveFormat.AverageBytesPerSecond) / synth.RawBufferSize) * synth.RawBufferSize;
            circularBuffer = new CircularBuffer(bufferSize);
            sbuff = new byte[synth.RawBufferSize];
        }

        public WaveFormat WaveFormat
        {
            get { return waveFormat; }
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            while (circularBuffer.Count < count)
            {
                lock (locker)
                {
                    if (msgQueue.Count > 0)
                        while (msgQueue.Count > 0)
                            processMessage(msgQueue.Dequeue());

                    synth.GetNext(sbuff);
                    circularBuffer.Write(sbuff, 0, sbuff.Length);
                }
            }

            return circularBuffer.Read(buffer, offset, count);
        }

        public void Reset()
        {
            circularBuffer.Reset();
        }

        private void processMessage(Message msg)
        {
            synth.ProcessMidiMessage(msg.channel, msg.command, msg.data1, msg.data2);
        }
    }
}
