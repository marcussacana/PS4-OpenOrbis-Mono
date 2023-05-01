using System;
using System.Runtime.InteropServices;

namespace SharpGLES
{
	public class GLBuffer<T>
		where T : new()
	{
		private T[] _buffer;
		private GCHandle _handle;
		private IntPtr _address;
		private bool _disposed;
		private int _position;
		private int _stride;
		public int capacity;

		public GLBuffer(int capacity)
		{
			this.capacity = capacity;

			_buffer = new T[capacity];

			_handle = GCHandle.Alloc(_buffer, GCHandleType.Pinned);

			_address = _handle.AddrOfPinnedObject();

			_stride = Marshal.SizeOf(typeof(T));
		}

		public void Position(int position)
		{
			_position = position;
		}

		public static implicit operator IntPtr(GLBuffer<T> array)
		{
			return IntPtr.Add(array._address, array._position * array._stride);
		}

		public void Put(T[] data, int offset, int length)
		{
			Array.Copy(data, _position, _buffer, offset, length);

			_position += length;
		}

		public void Put(T[] data)
		{
			Put(data, _position, data.Length);

			_position += data.Length;
		}

		public int Limit()
		{
			return capacity - _position;
		}

		public T this[int index]
		{
			get
			{
				return _buffer[index];
			}
			set
			{
				_buffer[index] = value;
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				_disposed = true;

				_handle.Free();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~GLBuffer()
		{
			Dispose(false);
		}
	}
}
