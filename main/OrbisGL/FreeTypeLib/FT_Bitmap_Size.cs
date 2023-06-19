using System.Runtime.InteropServices;

namespace OrbisGL.FreeTypeLib
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FT_Bitmap_Size
    {
        public short Height;
        public short Width;

        public FT_Pos Size;

        public FT_Pos Xppem;
        public FT_Pos Yppem;
    }
}