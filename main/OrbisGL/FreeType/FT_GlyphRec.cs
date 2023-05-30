using System;
using System.Runtime.InteropServices;

namespace OrbisGL.FreeType
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_GlyphRec
    {
        public void* Library;
        public void* Clazz;
        public FT_Glyph_Format Format;
        public FT_Vector Advance;
    }
}
