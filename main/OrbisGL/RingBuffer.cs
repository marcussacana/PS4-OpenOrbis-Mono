using System;
using System.IO;
using System.Threading;

namespace OrbisGL
{
    public sealed class RingBuffer : Stream
    {
        private int Size, ReadOffset, WriteOffset, BufferedAmount;
        private long ReadLoop, WriteLoop;

        byte[] DataBuffer;

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        /// <summary>
        /// Get the total amount of data currently buffered in the ring buffer
        /// </summary>
        public override long Length => BufferedAmount;

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (ReadOffset >= Size)
            {
                ReadOffset = 0;
                ReadLoop++;
            }

            if (count > BufferedAmount)
                count = BufferedAmount;

            if (offset + count > buffer.Length)
                count = buffer.Length - offset;

            if (ReadOffset == WriteOffset && ReadLoop >= WriteLoop)
                return 0;

            int MaxBulkRead = Size - ReadOffset;
            int ReadAmount = Math.Min(count, MaxBulkRead);

            if (ReadOffset < WriteOffset)
            {
                MaxBulkRead = WriteOffset - ReadOffset;
                ReadAmount = Math.Min(ReadAmount, MaxBulkRead);
            }

            Array.Copy(DataBuffer, ReadOffset, buffer, offset, ReadAmount);

            count -= ReadAmount;
            ReadOffset += ReadAmount;
            BufferedAmount -= ReadAmount;

            if (count > 0)
                return Read(buffer, offset + ReadAmount, count) + ReadAmount;
            
            return ReadAmount;
        }

        public override void Write(byte[] buffer, int InOffset, int count)
        {
            if (count > Size)
                throw new ArgumentOutOfRangeException("count");

            if (WriteOffset >= Size)
            {
                WriteOffset = 0;
                WriteLoop++;
            }

            while (BufferedAmount + count >= Size)
                Thread.Sleep(100);

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
                WriteLoop++;
            }

            BufferedAmount += count;
        }

        protected override void Dispose(bool disposing)
        {
            DataBuffer = null;
            base.Dispose(disposing);
        }
    }
}
