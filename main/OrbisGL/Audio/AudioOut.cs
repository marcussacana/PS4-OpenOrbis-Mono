using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrbisGL.Audio
{
    public class AudioOut : IDisposable
    {
        RingBuffer Buffer;


        int Channels;
        int Grain;
        int Sampling;

        Thread SoundThread;
        public AudioOut(int Channels, int Grain, int SamplingRate = 48000)
        {
            this.Channels = Channels;
            this.Grain = Grain;
            this.Sampling = SamplingRate;
        }

        public void Play(RingBuffer PCMBuffer)
        {
            Buffer = PCMBuffer;
            SoundThread = new Thread(Player);
            SoundThread.Start();
        }

        private void Player()
        {
            int handle = sceAudioOutOpen(OrbisGL.Constants.);
            while (true)
            {
                
            }
        }

        public void Stop()
        {
            SoundThread.Abort();
        }

        public void Dispose()
        {
            SoundThread.Abort();
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct OrbisAudioOutPortState
        {
            public ushort output;             // SceAudioOutStateOutput (bitwise OR)
            public byte channel;              // SceAudioOutStateChannel
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] reserved8_1;        // reserved
            public short volume;
            public ushort rerouteCounter;
            public ulong flag;                // SceAudioOutStateFlag (bitwise OR)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public ulong[] reserved64;        // reserved
        }


        [DllImport("libSceAudioOut.sprx")]
        extern int sceAudioOutClose(int handle);


        [DllImport("libSceAudioOut.sprx")]
        extern int sceAudioOutGetPortState(int handle, ref OrbisAudioOutPortState state);


        [DllImport("libSceAudioOut.sprx")]
        extern int sceAudioOutInit();


        [DllImport("libSceAudioOut.sprx")]
        extern int sceAudioOutOpen(int userId, int type, int index, uint len, uint freq, uint param);


        [DllImport("libSceAudioOut.sprx")]
        extern int sceAudioOutOutput(int handle, const void* p);


        [DllImport("libSceAudioOut.sprx")]
        extern int sceAudioOutOutputs();


        [DllImport("libSceAudioOut.sprx")]
        extern int sceAudioOutSetVolume();


        [DllImport("libSceAudioOut.sprx")]
        extern int sceAudioOutInitIpmiGetSession();
    }
}
