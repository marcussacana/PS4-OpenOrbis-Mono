using System;
using System.Runtime.InteropServices;

namespace SDL2.Types.FreeType
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Face
    {
        public long NumFaces;
        public long FaceIndex;

        public long FaceFlags;
        public long StyleFlags;

        public long NumGlyphs;

        public char* FamilyName;
        public char* StyleName;

        public int NumFixedSizes;
        public FT_Bitmap_Size* AvailableSizes;

        public int NumCharmaps;
        public FT_Charmap** Charmaps;

        public FT_Generic Generic;

        public FT_BBox BBox;

        public ushort UnitsPerEM;
        public short Ascender;
        public short Descender;
        public short Height;

        public short MaxAdvanceWidth;
        public short MaxAdvanceHeight;

        public short UnderlinePosition;
        public short UnderlineThickness;

        public FT_GlyphSlot* Glyph;
        public FT_Size* Size;
        public FT_Charmap* Charmap;

        public IntPtr Driver;
        public IntPtr Memory;
        public IntPtr Stream;

        public IntPtr SizesList;
        public FT_Generic AutoHint;
        public void* Extensions;
        public IntPtr Internal;
    }
}