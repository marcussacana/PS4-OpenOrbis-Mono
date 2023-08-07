using System;
using System.IO;
using System.Threading;

namespace OrbisGL
{
    public sealed class RingBuffer : Stream
    {
        int Size, ReadOffset, WriteOffset, BufferedAmount;

        byte[] DataBuffer;

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => Size;

        public override long Position { get => BufferedAmount; set => throw new NotImplementedException(); }

        public RingBuffer(int Size) { 
            this.Size = Size;
            DataBuffer = new byte[Size];
        }

        public override void Flush()
        {

        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override unsafe int Read(byte[] buffer, int OutOffset, int count)
        {
            if (ReadOffset >= Size)
                ReadOffset = 0;

            if (count > BufferedAmount)
                count = BufferedAmount;

            if (OutOffset + count > buffer.Length)
                count = buffer.Length - OutOffset;

            if (ReadOffset == WriteOffset)
                return 0;

            int MaxBulkRead = Size - ReadOffset;
            int ReadAmount = Math.Min(count, MaxBulkRead);

            if (ReadOffset < WriteOffset)
            {
                MaxBulkRead = WriteOffset - ReadOffset;
                ReadAmount = Math.Min(ReadAmount, MaxBulkRead);
            }

            Array.Copy(DataBuffer, ReadOffset, buffer, OutOffset, ReadAmount);

            count -= ReadAmount;
            ReadOffset += ReadAmount;
            BufferedAmount -= ReadAmount;

            if (ReadOffset >= Size)
                ReadOffset = 0;

            if (count > 0)
                return Read(buffer, OutOffset + ReadAmount, count) + ReadAmount;

            if (AntiOverflow.CurrentCount == 0)
                AntiOverflow.Release();

            return ReadAmount;
        }

        SemaphoreSlim AntiOverflow = new SemaphoreSlim(1, 1);
        public override void Write(byte[] buffer, int InOffset, int count)
        {
            if (count > Size)
                throw new ArgumentOutOfRangeException("count");

            if (WriteOffset >= Size)
                WriteOffset = 0;


            AntiOverflow.Wait();

            while (count > Size - BufferedAmount)
                AntiOverflow.Wait();

            try
            {
                if (WriteOffset + count <= Size)
                {
                    // Write the entire chunk at once if it fits in the buffer.
                    Array.Copy(buffer, InOffset, DataBuffer, WriteOffset, count);

                    WriteOffset += count;
                }
                else
                {
                    // Write in two parts if the chunk wraps around the end of the buffer.
                    int firstPartSize = Size - WriteOffset;
                    int secondPartSize = count - firstPartSize;

                    Array.Copy(buffer, InOffset, DataBuffer, WriteOffset, firstPartSize);
                    Array.Copy(buffer, InOffset + firstPartSize, DataBuffer, 0, secondPartSize);

                    WriteOffset = secondPartSize;
                }

                BufferedAmount += count;
            }
            finally
            {
                if (AntiOverflow.CurrentCount == 0)
                    AntiOverflow.Release();
            }
        }

        protected override void Dispose(bool disposing)
        {
            AntiOverflow.Dispose();
            base.Dispose(disposing);
        }
    }
}
