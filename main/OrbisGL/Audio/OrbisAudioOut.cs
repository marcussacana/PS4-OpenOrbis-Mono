using Orbis.Internals;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using static OrbisGL.Constants;

namespace OrbisGL.Audio
{
    public class OrbisAudioOut : IAudioOut
    {
        RingBuffer Buffer;

        private int handle;
        
        bool StopPlayer = false;

        int Channels;
        uint Grain;
        uint Sampling;

        Thread SoundThread;

        private static bool Initialized;

        public void SetProprieties(int Channels, uint Grain, uint SamplingRate = 48000)
        {
            if (!(new uint[] { 256, 512, 768, 1024, 1280, 1536, 1792, 2048 }).Contains(Grain))
                throw new ArgumentException("Grain must be one of the given values:\n256, 512, 768, 1024, 1280, 1536, 1792, 2048");

            this.Channels = Channels;
            this.Grain = Grain;
            this.Sampling = SamplingRate;
        }

        public void Play(RingBuffer PCMBuffer)
        {
            if (!Initialized)
            {
                var Rst = sceAudioOutInit();
                
                if (Rst < 0)
                    throw new Exception($"Failed to Init the Audio Driver, Error Code: {Rst}");
                
                Initialized = true;
            }

            if (SoundThread != null)
            {
                StopPlayer = true;
                while (SoundThread != null)
                    Thread.Sleep(100);
            }
            
            Buffer = PCMBuffer;

            SoundThread = new Thread(Player);
            SoundThread.Name = "AudioOut";
            SoundThread.Start();
        }

        private unsafe void Player()
        {
            var Param = (uint)(Channels > 2 ? SCE_AUDIO_OUT_PARAM_FORMAT_S16_8CH : SCE_AUDIO_OUT_PARAM_FORMAT_S16_STEREO);

            handle = sceAudioOutOpen(
                SCE_USER_SERVICE_USER_ID_SYSTEM, 
                SCE_AUDIO_OUT_PORT_TYPE_MAIN, 0,
                Grain, Sampling, Param);

            if (handle < 0)
                throw new Exception("Failed to Initialize the Audio Driver");

            SetVolume(80);

            int BlockSize = (int)(Grain * Channels * sizeof(short));

            byte[] ReadBuffer = new byte[BlockSize];

            var WavBufferA = new byte[Grain * (int)OrbisAudioOutChannel.MAX];
            var WavBufferB = new byte[Grain * (int)OrbisAudioOutChannel.MAX];

            var fWavBufferA = new byte[Grain * (int)OrbisAudioOutChannel.MAX];
            var fWavBufferB = new byte[Grain * (int)OrbisAudioOutChannel.MAX];

            bool CurrentBuffer = false;

            fixed (byte* pWavBufferA = WavBufferA, pWavBufferB = WavBufferB)
            fixed (byte* pfWaveBufferA = fWavBufferA, pfWaveBufferB = fWavBufferB)
            {
                while (!StopPlayer)
                {
                    short* WaveBuffer = (short*)(CurrentBuffer ? pWavBufferA : pWavBufferB);
                    float* fWaveBuffer = (float*)(CurrentBuffer ? pfWaveBufferA : pfWaveBufferB);

                    if (Buffer.Length >= BlockSize)
                    {
                        int Readed = Buffer.Read(CurrentBuffer ? WavBufferA : WavBufferB, 0, BlockSize);

                        if (Readed < BlockSize)
                        {
                            for (int i = Readed / 2; i < Channels * sizeof(short); i++)
                            {
                                WaveBuffer[i / 2] = 0;
                            }
                        }

                        if (Param == SCE_AUDIO_OUT_PARAM_FORMAT_FLOAT_8CH)
                        {
                            for (var i = 0; i < Grain * Channels; i++)
                            {
                                fWaveBuffer[i] = WaveBuffer[i] / 32768.0f;
                            }

                            sceAudioOutOutput(handle, fWaveBuffer);
                        }
                        else
                        {
                           sceAudioOutOutput(handle, WaveBuffer);
                        }

                        CurrentBuffer = !CurrentBuffer;
                    }
                    else
                    {
                        Kernel.sceKernelUsleep(1000);
                    }
                }
            }

            sceAudioOutOutput(handle, null);
            sceAudioOutClose(handle);
            StopPlayer = false;
            SoundThread = null;
        }

        public void SetVolume(byte Value)
        {
            int[] Volume = new int[(int)OrbisAudioOutChannel.MAX];

            for (int i = 0; i < Volume.Length; i++)
            {
                Volume[i] = (int)(ORBIS_AUDIO_VOLUME_0DB * (Value/100f));
            }

            sceAudioOutSetVolume(handle, ORBIS_AUDIO_VOLUME_FLAG_ALL, Volume);
        }

        public void Stop()
        {
            StopPlayer = true;
        }

        public void Dispose()
        {
            StopPlayer = true;
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
        static extern int sceAudioOutClose(int handle);


        [DllImport("libSceAudioOut.sprx")]
        static extern int sceAudioOutGetPortState(int handle, ref OrbisAudioOutPortState state);


        [DllImport("libSceAudioOut.sprx")]
        static extern int sceAudioOutInit();


        [DllImport("libSceAudioOut.sprx")]
        static extern int sceAudioOutOpen(int userId, int type, int index, uint len, uint freq, uint param);


        [DllImport("libSceAudioOut.sprx")]
        static unsafe extern int sceAudioOutOutput(int handle, void* Buffer);


        [DllImport("libSceAudioOut.sprx")]
        static extern int sceAudioOutOutputs();


        [DllImport("libSceAudioOut.sprx")]
        static extern int sceAudioOutSetVolume(int handle, int flags, int[] Volume);


        [DllImport("libSceAudioOut.sprx")]
        static extern int sceAudioOutInitIpmiGetSession();
    }
}
