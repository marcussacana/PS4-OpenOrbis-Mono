using System;
using System.Numerics;
using System.IO;
using System.Collections.Generic;
using SharpGLES;
using OrbisGL.GL;
using OrbisGL.FreeTypeLib;
using static OrbisGL.GL2D.Coordinates2D;

namespace OrbisGL.GL2D
{
    public unsafe class Text2D : GLObject2D
    {
        Texture FontTexture;

        public GlyphInfo[] GlyphsInfo { get; private set; } = null;
        public string Text { get; private set; } = null;
        public FontFaceHandler Font { get; set; }

        int ColorUniformLocation = -1;
        int TextureUniformLocation = -1;

        public RGBColor Color { get; set; } = RGBColor.White;

        public byte Transparency = 255;

        public int FontSize { get; set; }

        public Text2D(string FontPath, int FontSize) : this(GetFont(FontPath, FontSize, out _), FontSize) { }
        public Text2D(FontFaceHandler Font, int FontSize)
        {
            this.FontSize = FontSize;

            var hProgram = Shader.GetProgram(ResLoader.GetResource("VertexOffsetTexture"), ResLoader.GetResource("FragmentFont"));
            Program = new GLProgram(hProgram);

            ColorUniformLocation = GLES20.GetUniformLocation(hProgram, "Color");
            TextureUniformLocation = GLES20.GetUniformLocation(hProgram, "Texture");

            Program.AddBufferAttribute("Position", AttributeType.Float, AttributeSize.Vector3);
            Program.AddBufferAttribute("uv", AttributeType.Float, AttributeSize.Vector2);

            FontTexture = new Texture(true);
            this.Font = Font;
        }

        static Dictionary<string, FontFaceHandler> FontCache = new Dictionary<string, FontFaceHandler>();

        internal static FontFaceHandler GetFont(string FontPath, int FontSize, out bool Success)
        {
            if (FontPath == null)
                FontPath = FreeType.DefaultFace;

            string FontKey = $"{FontPath}";

            if (FontCache.ContainsKey(FontKey))
            {
                Success = true;
                var CurrentFont = FontCache[FontKey];

                if (!CurrentFont.Disposed)
                {
                    FreeType.SetFontSize(CurrentFont, FontSize);
                    return CurrentFont;
                }
            }

            if (!FreeType.LoadFont(FontPath, FontSize, out FontFaceHandler Font))
            {
                Success = false;
                foreach (var CurrentFont in FontCache.Values)
                {
                    if (CurrentFont.Disposed)
                        continue;

                    return CurrentFont;
                }

                throw new Exception("Failed to Load the Font");
            }


            FontCache[FontKey] = Font;

            Success = true;
            return Font;
            
        }

        public static void ClearFontCache()
        {
            foreach (var Font in FontCache.Values)
            {
                FreeType.UnloadFont(Font);
            }

            FontCache.Clear();
        }

        public override void RefreshVertex()
        {
            if (Text == null)
                return;

            SetText(Text);
            base.RefreshVertex();
        }

        public void SetFontSize(int FontSize)
        {
            FreeType.SetFontSize(Font, FontSize);
            RefreshVertex();
        }

        public void SetText(string Text)
        {
            this.Text = Text;

            if (Text == null)
            {
                FontTexture.SetData(1, 1, new byte[4], PixelFormat.RGBA);
                GlyphsInfo = null;
                return;
            }

            FreeType.SetFontSize(Font, FontSize);

            FreeType.MeasureText(Text, Font, out int Width, out int Height, out GlyphInfo[] Glyphs);
            
            base.Width = Width;
            base.Height = Height;
            GlyphsInfo = Glyphs;


            byte[] Buffer = new byte[Width * Height * 4];

            FreeType.RenderText(Buffer, Width, Height, Text, Font, RGBColor.White);

            FontTexture.SetData(Width, Height, Buffer, PixelFormat.RGBA);


            //   0 ---------- 1
            //   |            |
            //   |            |
            //   |            |
            //   2 ---------- 3

            ClearBuffers();

            AddArray(XToPoint(0), YToPoint(0), -1);//0
            AddArray(0, 0);

            AddArray(XToPoint(Width), YToPoint(0), -1);//1
            AddArray(1, 0);

            AddArray(XToPoint(0), YToPoint(Height), -1);//2
            AddArray(0, 1);
            
            AddArray(XToPoint(Width), YToPoint(Height), -1);//3
            AddArray(1, 1);


            AddIndex(0, 1, 2, 1, 2, 3);
        }

        public override void Draw(long Tick)
        {
            if (Text != null)
            {
                Program.SetUniform(ColorUniformLocation, Color, Transparency);
                Program.SetUniform(TextureUniformLocation, FontTexture.Active());
            } 
            else
            {
                return;
            } 

            base.Draw(Tick);
        }
    }
}
