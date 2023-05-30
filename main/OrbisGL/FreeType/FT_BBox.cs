using System.Runtime.InteropServices;

namespace OrbisGL.FreeType
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FT_BBox
    {
        public FT_Pos XMin, YMin;
        public FT_Pos XMax, YMax;
    }
}