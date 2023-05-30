using System.Runtime.InteropServices;

namespace OrbisGL.FreeType
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