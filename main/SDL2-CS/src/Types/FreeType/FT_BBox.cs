using System.Runtime.InteropServices;

namespace SDL2.Types.FreeType
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FT_BBox
    {
        public long XMin;
        public long YMin;
        public long XMax;
        public long YMax;
    }
}