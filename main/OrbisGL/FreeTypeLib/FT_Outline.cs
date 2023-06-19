using System.Runtime.InteropServices;

namespace OrbisGL.FreeTypeLib
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Outline
    {
        public short NumContours;
        public short NumPoints;

        public FT_Vector* Points;
        public byte* Tags;
        public short* Contours;

        public int Flags;
    }
}