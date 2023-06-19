using System.Runtime.InteropServices;

namespace OrbisGL.FreeTypeLib
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FT_BBox
    {
        public FT_Pos XMin, YMin;
        public FT_Pos XMax, YMax;
    }
}