using System.Runtime.InteropServices;

namespace OrbisGL.FreeType
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FT_Vector
    {
        private FT_Pos _x;
        private FT_Pos _y;

        public int X => ((int)_x >> 6);
        public int Y => ((int)_y >> 6);
    }
}