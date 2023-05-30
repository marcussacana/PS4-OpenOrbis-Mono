using System.Runtime.InteropServices;

namespace OrbisGL.FreeType
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FT_Glyph_Metrics
    {
        public FT_Pos Width;
        public FT_Pos Height;

        public FT_Pos HoriBearingX;
        public FT_Pos HoriBearingY;
        public FT_Pos HoriAdvance;

        public FT_Pos VertBearingX;
        public FT_Pos VertBearingY;
        public FT_Pos VertAdvance;
    }
}