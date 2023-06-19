using System;

namespace OrbisGL.FreeTypeLib
{
    public unsafe class FontFaceHandler : IDisposable
    {
        FT_Face* Face;

        public bool Disposed { get; private set; }

        public FontFaceHandler(FT_Face* Font) { this.Face = Font; }

        public static implicit operator FT_Face*(FontFaceHandler Handler)
        {
            return Handler.Face;
        }

        public static implicit operator FontFaceHandler(FT_Face* Face)
        {
            return new FontFaceHandler(Face);
        }

        public void Dispose()
        {
            if (Disposed)
                return;

            Disposed = true;
            FreeType.UnloadFont(Face);
        }
    }
}
