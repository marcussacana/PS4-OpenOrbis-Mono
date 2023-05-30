using System;
using System.Runtime.InteropServices;

namespace OrbisGL.FreeType
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

        public FT_Fixed XScale;
        public FT_Fixed YScale;

        public FT_Pos Ascender;
        public FT_Pos Descender;
        public FT_Pos Height;
        public FT_Pos MaxAdvance;
    }
}