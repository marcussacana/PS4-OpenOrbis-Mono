using System.Collections.Generic;
using OrbisGL.FreeType;
using OrbisGL.GL;
using SharpGLES;
using System;
using static OrbisGL.GL2D.Coordinates2D;

namespace OrbisGL.GL2D
{
    public unsafe class Text2D : GLObject2D
    {
        Texture FontTexture;
        public string Text { get; private set; } = null;
        public FT_Face* Face { get; set; }

        int ColorUniformLocation = -1;
        int TextureUniformLocation = -1;

        public RGBColor Color { get; set; } = RGBColor.White;

        public byte Transparency = 255;

        public Text2D(string FontPath, int FontSize) : this(GetFont(FontPath, FontSize)) { }
        public Text2D(FT_Face* Font)
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

        static Dictionary<string, IntPtr> FontCache = new Dictionary<string, IntPtr>();

        static FT_Face* GetFont(string FontPath, int FontSize)
        {
            string FontKey = $"{FontPath}-{FontSize}";

            if (FontCache.ContainsKey(FontKey))
            {
                return (FT_Face*)FontCache[FontKey];
            }
            else
            {
                if (!Render.LoadFont(FontPath, FontSize, out FT_Face* Font))
                {
                    throw new Exception("Failed to Load the Font");
                }


                FontCache[FontKey] = new IntPtr(Font);

                return Font;
            }
        }

        
        public void SetText(string Text)
        {
            this.Text = Text;
            Render.MeasureText(Text, Face, out int Width, out int Height);

            byte[] Buffer = new byte[Width * Height * 4];

            Render.RenderText(Buffer, Width, Height, Text, Face, RGBColor.White);

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
            base.Draw(Tick);
        }
    }
}
