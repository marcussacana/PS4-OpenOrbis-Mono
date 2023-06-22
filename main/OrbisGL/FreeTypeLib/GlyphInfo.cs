using OrbisGL.GL;

namespace OrbisGL.FreeTypeLib
{
    public struct GlyphInfo
    {
        public GlyphInfo(float X, float Y, float Width, float Height, char Char, int Index)
        {
            Area = new Rectangle(X, Y, Width, Height);
            this.Char = Char;
            this.Index = Index;
        }
        public int Index;
        public char Char;
        public Rectangle Area;
    }
}
