using System;
using System.Runtime.InteropServices;
using SDL2.Interface;
using SDL2.Types;
using SDL2.Types.FreeType;

namespace SDL2.Object
{
    
    //Text rendering stolen from: https://github.com/OpenOrbis/OpenOrbis-PS4-Toolchain/blob/master/samples/SDL2/SDL2/TTF.cpp
    public class TextElement : Element
    {
        static IntPtr FT_Library = IntPtr.Zero;
        
        private string _Text;
        public string Text {
            get => _Text;
            set
            {
                _Text = value;
                RefreshTexture();
                Invalidated = true;
            }
        }

        private unsafe FT_Face* _Face = null;
        public unsafe FT_Face* Face
        {
            get => _Face;
            set
            {
                _Face = value;
                RefreshTexture();
                Invalidated = true;
            }
        }
        
        private Color _BackgroundColor = new Color(0, 0, 0);
        public Color BackgroundColor
        {
            get => _BackgroundColor;
            set
            {
                _BackgroundColor = value;
                RefreshTexture();
                Invalidated = true;
            }
        }
        
        private Color _ForegroundColor = new Color(255, 255, 255);
        public Color ForegroundColor
        {
            get => _ForegroundColor;
            set
            {
                _ForegroundColor = value;
                RefreshTexture();
                Invalidated = true;
            }
        }
        

        public TextElement(Element Parent, string Name) : base(Parent, Name)
        {
            if (FT_Library == IntPtr.Zero)
            {
                if (FT_Init_FreeType(out FT_Library) != 0)
                    throw new Exception("Failed to Initialized the FreeType Environment");
            }

            _Text = Name;
        }

        unsafe void RefreshTexture()
        {
            if (Face == null)
                throw new Exception("Font Face Not Defined");
            
            if (Texture != null && Texture.Handler != IntPtr.Zero)
                SDL.SDL_DestroyTexture(Texture.Handler);

            var pTexture = CreateText();
            Texture = new Texture(pTexture);
        }

        unsafe IntPtr CreateText()
        {
            int w = Text.Length * Face->Size->Metrics.XPPem;
            int h = Face->Size->Metrics.YPPem * 2;

            var tempSurface = (SDL.SDL_Surface*)SDL.SDL_CreateRGBSurface(0, w, h, 32, 0, 0, 0, 0).ToPointer();

            int TotalWidth = SetText(tempSurface, Text, Face, ForegroundColor, BackgroundColor);

            this.Size = new Size(TotalWidth, h);

            uint colorKey;

            if (ForegroundColor.R == 0 && ForegroundColor.G == 0 && ForegroundColor.B == 0)
                colorKey = SDL.SDL_MapRGB(tempSurface->format, 255, 255, 255);
            else
                colorKey = SDL.SDL_MapRGB(tempSurface->format, 0, 0, 0);

            SDL.SDL_SetColorKey(new IntPtr(tempSurface), 1, colorKey);

            // Derive the texture and make it blendable
            var finalTexture = SDL.SDL_CreateTextureFromSurface(Renderer.Handler, new IntPtr(tempSurface));
            SDL.SDL_SetTextureBlendMode(finalTexture, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

            // We no longer need the surface, we only need it for texture creation
            SDL.SDL_FreeSurface(new IntPtr(tempSurface));

            return finalTexture;
        }

        
        public unsafe static bool InitFont(TextElement Element, string FontPath, int FontSize)
        {
            var rc = FT_New_Face(FT_Library, FontPath, 0, out var FontFace);

            if (rc < 0)
                return false;

            rc = FT_Set_Pixel_Sizes(FontFace, 0, FontSize);

            Element.Face = FontFace;
            
            return rc >= 0;
        }
        
        unsafe int SetText(SDL.SDL_Surface* surface, string txt, FT_Face* face, Color fgColor, Color bgColor)
        {
            int rc;
            int xOffset = 0;
            int yOffset = 0;
            long totalWidth = 0;
            uint* pixels = (uint*)surface->pixels;

            if (pixels == null)
                return -1;

            // Get the glyph slot for bitmap and font metrics
            FT_GlyphSlot* slot = face->Glyph;

            // Iterate each character of the text to write to the screen
            for (int n = 0; n < txt.Length; n++)
            {

                // Get the glyph for the ASCII code
                var glyph_index = FT_Get_Char_Index(face, txt[n]);

                // Load and render in 8-bit color
                rc = FT_Load_Glyph(face, glyph_index, FT_Load_Flag.FT_LOAD_DEFAULT);

                if (rc != 0)
                    continue;

                rc = FT_Render_Glyph(slot, FT_Render_Mode.FT_RENDER_MODE_NORMAL);

                if (rc != 0)
                    continue;
                
                // If we get a newline, increment the y offset, reset the x offset, and skip to the next character
                if (txt[n] == '\n')
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
                        long x = xPos + xOffset + (slot->Metrics.HoriBearingX / 64);
                        long y = face->Size->Metrics.YPPem + yPos + yOffset - slot->BitmapTop;

                        // Calculate final color values
                        //var r = (bgColor.R * (255 - 255) + pixel * fgColor.R * 255 / 255) / 255;
                        //var g = (bgColor.G * (255 - 255) + pixel * fgColor.G * 255 / 255) / 255;
                        //var b = (bgColor.B * (255 - 255) + pixel * fgColor.B * 255 / 255) / 255;
                        var r = (pixel * fgColor.R) / 255;
                        var g = (pixel * fgColor.G) / 255;
                        var b = (pixel * fgColor.B) / 255;

                        // We need to do bounds checking before commiting the pixel write due to our transformations, or we
                        // could write out-of-bounds of the texture's pixel array
                        if (x < 0 || y < 0 || x >= surface->w || y >= surface->h)
                            continue;

                        if (pixel != 0x00)
                        {
                            // Get pixel location based on pitch
                            long pixelIdx = (y * surface->w) + x;

                            // Encode to 24-bit color
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

        public sealed override Element Parent { get; set; }
        public sealed override Renderer Renderer { get; set; }
        public override INative Texture { get; set; }
        
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


        [DllImport("libSceFreeTypeOl", CallingConvention = CallingConvention.Cdecl)]
        private static extern int FT_Init_FreeType(out IntPtr ftLib);
        
        [DllImport("libSceFreeTypeOl", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe int FT_New_Face(IntPtr ftLib, string fontPath, int faceIndex, out FT_Face* face);

        [DllImport("libSceFreeTypeOl", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe int FT_Set_Pixel_Sizes(FT_Face* face, int pixelWidth, int pixelHeight);

        [DllImport("libSceFreeTypeOl", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe int FT_Load_Glyph(FT_Face* face, uint glyphIndex, FT_Load_Flag loadFlags);

        [DllImport("libSceFreeTypeOl", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe int FT_Render_Glyph(FT_GlyphSlot* slot, FT_Render_Mode renderMode);

        [DllImport("libSceFreeTypeOl", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe uint FT_Get_Char_Index(FT_Face* face, char c);
    }
}