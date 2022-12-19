using System;
using System.Runtime.InteropServices;

namespace SDL2.Types.FreeType
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Size
    {
        public FT_Face* Face;
        public FT_Generic Generic;
        public FT_Size_Metrics Metrics;
        public IntPtr Internal;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct FT_Size_Metrics
    {
        public ushort XPPem;
        public ushort YPPem;

        public long XScale;
        public long YScale;

        public long Ascender;
        public long Descender;
        public long Height;
        public long MaxAdvance;
    }
}