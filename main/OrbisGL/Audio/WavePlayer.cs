using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrbisGL.Audio
{
    public class WavePlayer : IAudioPlayer
    {
        bool Stopped = true;
        bool Paused;
        BinaryReader Stream;
        IAudioOut Driver;

        WAVRIFFHEADER Header;
        FORMATCHUNK Format;
        LISTCHUNK? List;
        CUECHUNK? Cue;
        FACTCHUNK? Fact;

        long DataOffset;
        long DataSize;

        Thread PlayerThread = null;

        public TimeSpan? Duration { get; private set; }

        public bool Playing => !Paused && Stream != null && !Stopped;

        public void Close()
        {
            Stopped = true;
        }

        public void Dispose()
        {
            Close();
            Stream?.Dispose();
        }

        public void Open(Stream File)
        {
            Stream = new BinaryReader(File);
            ParseHeader();
        }

        void ParseHeader()
        {
            var Header = new CHUNKINFO<WAVRIFFHEADER>();
            Header = ReadChunkInfo();
            Header.Data.RiffType.Data = Stream.ReadChars(4);

            if (Header.ChunkID != "RIFF" || Header.Data.RiffType != "WAVE")
                throw new NotSupportedException("Invalid or Unsupported WAV file");

            while (Stream.PeekChar() != -1)
            {
                ReadChunk();
            }
        }

        private void ReadChunk()
        {
            var Info = ReadChunkInfo();
            long NextChunkPos = Stream.BaseStream.Position + Info.ChunkSize;
            switch (Info.ChunkID)
            {
                case "fmt ":
                    var Format = new CHUNKINFO<FORMATCHUNK>();
                    Format = Info;
                    Format.Data.WFormatTag = Stream.ReadInt16();
                    Format.Data.WChannels = Stream.ReadUInt16();
                    Format.Data.DSamplesPerSec = Stream.ReadUInt32();
                    Format.Data.DAvgBytesPerSec = Stream.ReadUInt32();
                    Format.Data.WBlockAlign = Stream.ReadUInt16();
                    Format.Data.WSamplesPerBlock = Stream.ReadUInt16();

                    this.Format = Format.Data;
                    break;
                case "LIST":
                    var List = new CHUNKINFO<LISTCHUNK>();
                    List = Info;
                    List.Data.ChunkType.Data = Stream.ReadChars(4);
                    List.Data.Subchunks = new List<LISTSUBCHUNK>();
                    while (Stream.BaseStream.Position < NextChunkPos)
                        List.Data.Subchunks.Add(ReadSubChunk());

                    this.List = List.Data;
                    break;
                case "fact":
                    var Fact = new CHUNKINFO<FACTCHUNK>();
                    Fact = Info;
                    Fact.Data.UncompressedSize = Stream.ReadUInt32();

                    this.Fact = Fact.Data;
                    break;
                case "cue ":
                    var Cue = new CHUNKINFO<CUECHUNK>();
                    Cue = Info;
                    Cue.Data.DwCuePoints = Stream.ReadInt32();

                    Cue.Data.Points = new CUEPOINT[Cue.Data.DwCuePoints];
                    for (int i = 0; i < Cue.Data.Points.Length; i++)
                    {
                        Cue.Data.Points[i] = new CUEPOINT() {
                            DwIdentifier = Stream.ReadInt32(),
                            DwPosition = Stream.ReadInt32(),
                            FccChunk = new ID()
                            {
                                Data = Stream.ReadChars(4)
                            },
                            DwChunkStart = Stream.ReadInt32(),
                            DwBlockStart = Stream.ReadInt32(),
                            DwSampleOffset = Stream.ReadInt32()
                        };
                    }

                    this.Cue = Cue.Data;
                    break;
                case "data":
                    DataOffset = Stream.BaseStream.Position;
                    DataSize = Info.ChunkSize;
                    break;
            }

            Stream.BaseStream.Position = NextChunkPos;
        }

        private LISTSUBCHUNK ReadSubChunk()
        {
            CHUNKINFO<LISTSUBCHUNK> Info = ReadChunkInfo();
            var Size = Info.ChunkSize + (Info.ChunkSize % 1);
            long NextChunkPos = Stream.BaseStream.Position + Size;

            Info.Data.ListData = Stream.ReadBytes(Info.ChunkSize);

            Stream.BaseStream.Position = NextChunkPos;

            return Info.Data;
        }

        CHUNKINFO ReadChunkInfo()
        {
            var Info = new CHUNKINFO();
            Info.ChunkID.Data = Stream.ReadChars(4);
            Info.ChunkSize = Stream.ReadInt32();
            return Info;
        }

        public void Pause()
        {
            Paused = true;
        }

        public void Resume()
        {
            if (PlayerThread == null)
            {
                PlayerThread = new Thread(Player);
                PlayerThread.Name = "WavePlayer";
                PlayerThread.Start();
            }

            Paused = false;
        }

        private void Player()
        {
            int BlockSize = Format.WChannels * sizeof(short) * (int)Format.DSamplesPerSec;
            
            using (var Buffer = new RingBuffer(BlockSize*2))
            {
                Stream.BaseStream.Position = DataOffset;
                var EndPos = DataOffset + DataSize;

                const int Grain = 256;
        
                Driver.SetProprieties(Format.WChannels, Grain, Format.DSamplesPerSec);
                Driver.Play(Buffer);

                Duration = TimeSpan.Zero;

                byte[] DataBuffer = new byte[BlockSize];

                Stopped = false;

                try
                {
                    while (Stream.BaseStream.Position < EndPos && !Stopped)
                    {
                        int Readed = Stream.Read(DataBuffer, 0, DataBuffer.Length);
                        Buffer.Write(DataBuffer, 0, Readed);

                        Duration += TimeSpan.FromSeconds(1);

                        while (Paused && !Stopped)
                            Thread.Sleep(100);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    Stopped = true;
                    Driver.Stop();
                    PlayerThread = null;
                }
            }
        }

        public void SetAudioDriver(IAudioOut Driver)
        {
            this.Driver = Driver;
        }

        public void SkipTo(TimeSpan Duration)
        {
            this.Duration = Duration;
            Stream.BaseStream.Position = (long)(Format.DAvgBytesPerSec * Duration.TotalSeconds) + DataOffset;

        }

        struct ID
        {
            public char[] Data;
            public string Value => new string(Data);

            public static implicit operator string(ID ID)
            {
                return ID.Value;
            }
        }

        struct WAVRIFFHEADER
        {
            public ID RiffType;
        }

        struct FORMATCHUNK
        {
            public short WFormatTag;
            public ushort WChannels;
            public uint DSamplesPerSec;
            public uint DAvgBytesPerSec;
            public ushort WBlockAlign;
            public ushort WBitsPerSample;
            public ushort Wcbsize;
            public ushort WSamplesPerBlock;
            public byte[] UnknownData;
        }

        struct CHUNKINFO<T> where T : struct
        {
            public ID ChunkID;
            public int ChunkSize;
            public T Data;

            public static implicit operator CHUNKINFO<T>(CHUNKINFO Data)
            {
                return new CHUNKINFO<T>()
                {
                    ChunkID = Data.ChunkID,
                    ChunkSize = Data.ChunkSize,
                    Data = default
                };
            }
        }
        struct CHUNKINFO
        {
            public ID ChunkID;
            public int ChunkSize;
        }

        struct DATACHUNK
        {
            public byte[] WaveformData;
        }

        struct FACTCHUNK
        {
            public uint UncompressedSize;
        }

        struct CUEPOINT
        {
            public int DwIdentifier;
            public int DwPosition;
            public ID FccChunk;
            public int DwChunkStart;
            public int DwBlockStart;
            public int DwSampleOffset;
        }

        struct CUECHUNK
        {
            public int DwCuePoints;
            public CUEPOINT[] Points;
            public byte[] UnknownData;
        }

        struct LISTSUBCHUNK
        {
            public byte[] ListData;
        }

        struct LISTCHUNK
        {
            public ID ChunkType;
            public List<LISTSUBCHUNK> Subchunks;
        }
    }
}
