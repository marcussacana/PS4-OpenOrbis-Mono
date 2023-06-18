using OrbisGL.GL;
using System;
using System.Runtime.InteropServices;
using Orbis.Internals;
using System.IO;
using System.Numerics;

namespace OrbisGL.FreeType
{
    public static class FreeType
    {
        static FreeType()
        {
            if (FT_Library == IntPtr.Zero)
            {
#if ORBIS
                if (FT_Init_FreeType(out FT_Library) != 0)
                    Kernel.Log("ERROR: Failed to Initialized the FreeType Environment");
#else

                FT_Init_FreeType(out FT_Library);
#endif
            }

#if ORBIS
            DefaultFace = Path.Combine(IO.GetAppBaseDirectory(), "assets", "fonts", "Gontserrat-Regular.ttf");
#else
            DefaultFace = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "segoeui.ttf");
#endif
        }

        public readonly static string DefaultFace;

        public readonly static IntPtr FT_Library = IntPtr.Zero;

        public static unsafe bool LoadFont(string FontPath, int FontSize, out FontFaceHandler Face)
        {
            Face = default;

            var rc = FT_New_Face(FT_Library, FontPath, 0, out var FontFace);

            if (rc != 0)
                return false;

            rc = FT_Set_Pixel_Sizes(FontFace, 0, FontSize);

            Face = FontFace;

            return rc >= 0;
        }

        public static unsafe bool UnloadFont(FontFaceHandler Face)
        {
            return FT_Done_Face(Face) == 0;
        }

        public static unsafe void MeasureText(string Text, FontFaceHandler FontFace, out int Width, out int Height, out Vector4[] GlyphsRectangles)
        {

            FT_Face* Face = FontFace;

            GlyphsRectangles = new Vector4[Text.Length];

            int rc;
            int totalWidth = 0;
            int lineWidth = 0;
            int maxHeight = 0;

            // Iterate each character of the text to measure its size
            for (int n = 0; n < Text.Length; n++)
            {
                // Handle newline characters
                if (Text[n] == '\n')
                {
                    totalWidth = Math.Max(totalWidth, lineWidth);
                    lineWidth = 0;
                    maxHeight += ((int)Face->Size->Metrics.Height >> 6);
                    continue;
                }

                // Get the glyph for the ASCII code
                var glyph_index = FT_Get_Char_Index(Face, Text[n]);

                // Load the glyph
                rc = FT_Load_Glyph(Face, glyph_index, FT_Load_Flag.FT_LOAD_NO_BITMAP);

                // Get the glyph slot for bitmap and font metrics
                FT_GlyphSlot* slot = Face->Glyph;

                if (rc != 0)
                    continue;

                int XOffset = lineWidth;
                int YOffset = maxHeight;
                int GlyphWidth = slot->Advance.X;
                int GlyphHeight = slot->Advance.Y;

                GlyphsRectangles[n] = new Vector4(XOffset, YOffset, GlyphWidth, GlyphHeight);

                // Increment total width for the character
                lineWidth += GlyphWidth;
            }

            totalWidth = Math.Max(totalWidth, lineWidth);
            maxHeight += ((int)Face->Size->Metrics.Height >> 6);

            Width = totalWidth;
            Height = maxHeight;
        }

        public unsafe static int RenderText(byte[] Buffer, int BufferWidth, int BufferHeight, string Text, FontFaceHandler Face, RGBColor FGColor)
        {
            fixed (byte* pBuffer = Buffer)
            {

                return RenderText(pBuffer, BufferWidth, BufferHeight, Text, Face, FGColor);
            }
        }

        public static unsafe int RenderText(byte* Buffer, int BufferWidth, int BufferHeight, string Text, FontFaceHandler FontFace, RGBColor FGColor)
        {

            FT_Face* Face = FontFace;

            int rc;
            int xOffset = 0;
            int yOffset = 0;
            long totalWidth = 0;
            uint* pixels = (uint*)Buffer;

            if (pixels == null)
                return -1;

            // Get the glyph slot for bitmap and font metrics
            FT_GlyphSlot* slot = Face->Glyph;

            // Iterate each character of the text to write to the screen
            for (int n = 0; n < Text.Length; n++)
            {

                // Get the glyph for the ASCII code
                var glyph_index = FT_Get_Char_Index(Face, Text[n]);

                // Load and render in 8-bit color
                rc = FT_Load_Glyph(Face, glyph_index, FT_Load_Flag.FT_LOAD_DEFAULT);

                if (rc != 0)
                    continue;

                rc = FT_Render_Glyph(slot, FT_Render_Mode.FT_RENDER_MODE_NORMAL);

                if (rc != 0)
                    continue;

                // If we get a newline, increment the y offset, reset the x offset, and skip to the next character
                if (Text[n] == '\n')
                {
                    xOffset = 0;
                    yOffset += (int)slot->Bitmap.Width * 2;

                    continue;
                }

                // Parse and write the bitmap to the frame buffer
                for (int yPos = 0; yPos < slot->Bitmap.Rows; yPos++)
                {
                    for (int xPos = 0; xPos < slot->Bitmap.Width; xPos++)
                    {
                        // Decode the 8-bit bitmap
                        byte pixel = slot->Bitmap.Buffer[(yPos * slot->Bitmap.Width) + xPos];

                        // Get new pixel coordinates to account for the character position and baseline, as well as newlines
                        long x = xPos + xOffset + ((int)slot->Metrics.HoriBearingX / 64);
                        long y = Face->Size->Metrics.YPPem + yPos + yOffset - slot->BitmapTop;

                        // Calculate final color values
                        var r = (pixel * FGColor.R) / 255;
                        var g = (pixel * FGColor.G) / 255;
                        var b = (pixel * FGColor.B) / 255;

                        // We need to do bounds checking before commiting the pixel write due to our transformations, or we
                        // could write out-of-bounds of the texture's pixel array
                        if (x < 0 || y < 0 || x >= BufferWidth || y >= BufferHeight)
                            continue;

                        if (pixel != 0x00)
                        {
                            // Get pixel location based on pitch
                            long pixelIdx = (y * BufferWidth) + x;

                            // Encode to ARGB
                            uint encodedColor = (uint)((pixel << 24) + (r << 16) + (g << 8) + b);

                            // Draw to the pixel buffer
                            pixels[pixelIdx] = encodedColor;
                        }
                    }
                }

                // Increment x offset for the next character
                xOffset += slot->Advance.X;
                totalWidth += slot->Bitmap.Width;
            }

            return (int)totalWidth;
        }

        enum FT_Load_Flag : uint
        {
            FT_LOAD_DEFAULT = 0x0,
            FT_LOAD_NO_SCALE = 1 << 0,
            FT_LOAD_NO_HINTING = 1 << 1,
            FT_LOAD_RENDER = 1 << 2,
            FT_LOAD_NO_BITMAP = 1 << 3,
            FT_LOAD_VERTICAL_LAYOUT = 1 << 4,
            FT_LOAD_FORCE_AUTOHINT = 1 << 5,
            FT_LOAD_CROP_BITMAP = 1 << 6,
            FT_LOAD_PEDANTIC = 1 << 7,
            FT_LOAD_IGNORE_GLOBAL_ADVANCE_WIDTH = 1 << 9,
            FT_LOAD_NO_RECURSE = 1 << 10,
            FT_LOAD_IGNORE_TRANSFORM = 1 << 11,
            FT_LOAD_MONOCHROME = 1 << 12,
            FT_LOAD_LINEAR_DESIGN = 1 << 13,
            FT_LOAD_NO_AUTOHINT = 1 << 15
        }

        enum FT_Render_Mode
        {
            FT_RENDER_MODE_NORMAL = 0,
            FT_RENDER_MODE_MONO,
            FT_RENDER_MODE_LCD,
            FT_RENDER_MODE_LCD_V,
            FT_RENDER_MODE_MAX
        }
#if ORBIS
        private const string FreeTypeLib = "libSceFreeTypeOl";
#else
        private const string FreeTypeLib = "freetype";
#endif

        [DllImport(FreeTypeLib)]
        private static extern int FT_Init_FreeType(out IntPtr ftLib);

        [DllImport(FreeTypeLib, CharSet = CharSet.Ansi)]
        private static extern unsafe int FT_New_Face(IntPtr ftLib, string fontPath, int faceIndex, out FT_Face* face);

        [DllImport(FreeTypeLib)]
        private static extern unsafe int FT_Done_Face(FT_Face* face);

        [DllImport(FreeTypeLib)]
        private static extern unsafe int FT_Set_Pixel_Sizes(FT_Face* face, int pixelWidth, int pixelHeight);

        [DllImport(FreeTypeLib)]
        private static extern unsafe int FT_Load_Glyph(FT_Face* face, uint glyphIndex, FT_Load_Flag loadFlags);

        [DllImport(FreeTypeLib)]
        private static extern unsafe int FT_Render_Glyph(FT_GlyphSlot* slot, FT_Render_Mode renderMode);

        [DllImport(FreeTypeLib)]
        private static extern unsafe int FT_Get_Glyph(FT_GlyphSlot* Slot, FT_GlyphRec* Glyph);

        [DllImport(FreeTypeLib)]
        private static extern unsafe uint FT_Get_Char_Index(FT_Face* face, char c);
    }
}
