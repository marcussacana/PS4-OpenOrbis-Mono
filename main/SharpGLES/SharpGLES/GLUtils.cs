using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SharpGLES
{
	public static class GLUtils
	{
		public static void TexImage2D(int target, int level, Image image, int border)
		{
			TexImage2D(target, level, image, border, false);
		}

		public static void TexImage2D(int target, int level, Image image, int border, bool preMultiplyAlpha)
		{
			PixelFormat format = PixelFormat.Format32bppArgb;

			Bitmap bitmap = new Bitmap(image.Width, image.Height, format);

			Graphics graphics = Graphics.FromImage(bitmap);

			graphics.DrawImage(image, 0, 0, image.Width, image.Height);

			BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, format);

			int size = data.Stride * data.Height;

			byte[] buffer = new byte[size];

			Marshal.Copy(data.Scan0, buffer, 0, size);

			bitmap.UnlockBits(data);

			bitmap.Dispose();

			if (preMultiplyAlpha)
			{
				for (int i = 0; i < buffer.Length; i += 4)
				{
					int b = buffer[i];
					int g = buffer[i + 1];
					int r = buffer[i + 2];
					int a = buffer[i + 3];
				
					buffer[i] = (byte)(r * a / 256);
					buffer[i + 1] = (byte)(g * a / 256);
					buffer[i + 2] = (byte)(b * a / 256);
					buffer[i + 3] = (byte)a;
				}
			}
			else
			{
				for (int i = 0; i < buffer.Length; i += 4)
				{
					byte b = buffer[i];
					byte r = buffer[i + 2];

					buffer[i] = r;
					buffer[i + 2] = b;
				}
			}

			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

			IntPtr address = handle.AddrOfPinnedObject();

			GLES20.TexImage2D(target, level, GLES20.GL_RGBA, image.Width, image.Height, border, GLES20.GL_RGBA, GLES20.GL_UNSIGNED_BYTE, address);

			handle.Free();
		}
	}
}
