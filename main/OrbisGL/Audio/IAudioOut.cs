using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Audio
{
    public interface IAudioOut : IDisposable
    {
        void SetProprieties(int Channels, uint Grain, uint SamplingRate = 48000);
        void Play(RingBuffer PCMBuffer);
        void Stop();
    }
}
