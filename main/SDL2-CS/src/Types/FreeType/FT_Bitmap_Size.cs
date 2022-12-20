using System.Runtime.InteropServices;

namespace SDL2.Types.FreeType
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FT_Bitmap_Size
    {
        public short Height;
        public short Width;

        public long Size;

        public long Xppem;
        public long Yppem;
    }
}