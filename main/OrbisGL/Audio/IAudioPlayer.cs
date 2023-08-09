using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Audio
{
    public interface IAudioPlayer : IDisposable
    {
        bool Playing { get; }
        TimeSpan? Duration { get; }

        /// <summary>
        /// Loads an audio file (does not start the player)
        /// </summary>
        /// <param name="File">the audio file data</param>
        void Open(Stream File);

        /// <summary>
        /// Stop and close the Audio Stream
        /// </summary>
        void Close();

        /// <summary>
        /// Pause the audio player
        /// </summary>
        void Pause();

        /// <summary>
        /// Resume the audio player or start it
        /// </summary>
        void Resume();

        /// <summary>
        /// Changes the current player time
        /// </summary>
        /// <param name="Duration"></param>
        void SkipTo(TimeSpan Duration);

        /// <summary>
        /// Sets the output audio driver
        /// </summary>
        /// <param name="Driver"></param>
        void SetAudioDriver(IAudioOut Driver);
    }
}
