using System;
using System.Runtime.InteropServices;

namespace OrbisGL.FreeType
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Face
    {
        public FT_Long NumFaces;
        public FT_Long FaceIndex;

        public FT_Long FaceFlags;
        public FT_Long StyleFlags;

        public FT_Long NumGlyphs;

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

        public FT_List SizesList;
        public FT_Generic AutoHint;
        public void* Extensions;
        public IntPtr Internal;
    }
}