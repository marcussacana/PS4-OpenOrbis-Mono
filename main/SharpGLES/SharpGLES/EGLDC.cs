using System;
using System.Runtime.InteropServices;

namespace SharpGLES
{
	public static class EGLDC
	{
		const string Name = "User32.dll";

		[DllImport(Name)]
		public static extern IntPtr GetDC(IntPtr hwnd);

		[DllImport(Name)]
		public static extern void ReleaseDC(IntPtr hWnd, IntPtr hDC);
	}
}
