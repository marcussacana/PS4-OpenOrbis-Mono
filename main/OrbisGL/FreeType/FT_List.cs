using System;
using System.Runtime.InteropServices;

namespace OrbisGL.FreeType
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_List
    {
        public IntPtr Head;
        public IntPtr Tail;
    }
}
