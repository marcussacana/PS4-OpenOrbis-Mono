﻿using System.Collections.Generic;
using OrbisGL.FreeType;
using OrbisGL.GL;
using SharpGLES;
using System;
using static OrbisGL.GL2D.Coordinates2D;
using System.Numerics;
using System.Linq;
using System.IO;

namespace OrbisGL.GL2D
{
    public unsafe class Text2D : GLObject2D
    {
        Texture FontTexture;

        public Vector4[] GlyphsSpace { get; private set; } = null;
        public string Text { get; private set; } = null;
        public FontFaceHandler Face { get; set; }

        int ColorUniformLocation = -1;
        int TextureUniformLocation = -1;

        public RGBColor Color { get; set; } = RGBColor.White;

        public byte Transparency = 255;

        public Text2D(string FontPath, int FontSize) : this(GetFont(FontPath, FontSize)) { }
        public Text2D(FontFaceHandler Font)
        {
            var hProgram = Shader.GetProgram(ResLoader.GetResource("VertexOffsetTexture"), ResLoader.GetResource("FragmentFont"));
            Program = new GLProgram(hProgram);

            ColorUniformLocation = GLES20.GetUniformLocation(hProgram, "Color");
            TextureUniformLocation = GLES20.GetUniformLocation(hProgram, "Texture");

            Program.AddBufferAttribute("Position", AttributeType.Float, AttributeSize.Vector3);
            Program.AddBufferAttribute("uv", AttributeType.Float, AttributeSize.Vector2);

            FontTexture = new Texture(true);
            Face = Font;
        }

        static Dictionary<string, FontFaceHandler> FontCache = new Dictionary<string, FontFaceHandler>();

        internal static FontFaceHandler GetFont(string FontPath, int FontSize)
        {
            if (FontPath == null || !File.Exists(FontPath))
            {
                foreach (var CurrentFont in FontCache.Values)
                {
                    if (CurrentFont.Disposed)
                        continue;

                    return CurrentFont;
                }
            }

            string FontKey = $"{FontPath}-{FontSize}";

            if (FontCache.ContainsKey(FontKey))
            {
                var CurrentFont = FontCache[FontKey];
                if (!CurrentFont.Disposed)
                    return CurrentFont;
            }

            if (!FreeType.FreeType.LoadFont(FontPath, FontSize, out FontFaceHandler Font))
            {
                throw new Exception("Failed to Load the Font");
            }


            FontCache[FontKey] = Font;

            return Font;
            
        }

        public static void ClearFontCache()
        {
            foreach (var Font in FontCache.Values)
            {
                FreeType.FreeType.UnloadFont(Font);
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

        

        public void SetText(string Text)
        {
            this.Text = Text;

            if (Text == null)
            {
                FontTexture.SetData(1, 1, new byte[4], PixelFormat.RGBA);
                GlyphsSpace = null;
                return;
            }

            FreeType.FreeType.MeasureText(Text, Face, out int Width, out int Height, out Vector4[] Glyphs);
            
            base.Width = Width;
            base.Height = Height;
            GlyphsSpace = Glyphs;


            byte[] Buffer = new byte[Width * Height * 4];

            FreeType.FreeType.RenderText(Buffer, Width, Height, Text, Face, RGBColor.White);

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
