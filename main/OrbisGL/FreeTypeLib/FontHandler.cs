using System;
using System.Runtime.InteropServices;

namespace OrbisGL.FreeTypeLib
{
    public unsafe class FontFaceHandler : IDisposable
    {
        FT_Face* Face;
        public int CurrentSize { get; private set; }
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

            UnloadFont();
        }

        public bool SetFontSize(int FontSize)
        {
            bool Result = FT_Set_Pixel_Sizes(Face, 0, FontSize) >= 0;

            if (Result)
                CurrentSize = FontSize;

            return Result;
        }

        bool UnloadFont()
        {
            Disposed = true;
            return FT_Done_Face(Face) == 0;
        }

        [DllImport(FreeType.FreeTypeLib)]
        private static extern int FT_Set_Pixel_Sizes(FT_Face* face, int pixelWidth, int pixelHeight);

        [DllImport(FreeType.FreeTypeLib)]
        private static extern int FT_Done_Face(FT_Face* face);
    }
}
