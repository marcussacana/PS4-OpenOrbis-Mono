using OrbisGL.GL;
using SharpGLES;
using static OrbisGL.GL2D.Coordinates2D;

namespace OrbisGL.GL2D
{
    public class RoundedRectangle2D : GLObject2D
    {
        readonly int BorderUniformLocation;
        readonly int ColorUniformLocation;
        readonly int ContourWidthUniformLocation;

        public byte Transparency { get; set; } = 255;

        public RGBColor Color { get; set; } = RGBColor.White;

        public float RoundLevel { get; set; } = 0.8f;

        public float ContourWidth { get; set; } = 1.0f;

        public bool Fill { get; private set; }

        public RoundedRectangle2D(int Width, int Height, bool Fill)
        {
            this.Fill = Fill;

            var hProg = Shader.GetProgram(ResLoader.GetResource("VertexOffsetTexture"), ResLoader.GetResource(Fill ? "FragmentColorRounded" : "FragmentColorRoundedContour"));
            Program = new GLProgram(hProg);

            Program.AddBufferAttribute("Position", AttributeType.Float, AttributeSize.Vector3);
            Program.AddBufferAttribute("uv", AttributeType.Float, AttributeSize.Vector2);

            RenderMode = (int)OrbisGL.RenderMode.Triangle;

            BorderUniformLocation = GLES20.GetUniformLocation(Program.Handler, "Border");
            ColorUniformLocation = GLES20.GetUniformLocation(Program.Handler, "Color");
            ContourWidthUniformLocation = GLES20.GetUniformLocation(Program.Handler, "ContourWidth");

            Program.SetUniform("Resolution", (float)Width, (float)Height);
        }

        public override void RefreshVertex()
        {
            ClearBuffers();

            //   0 ---------- 1
            //   |            |
            //   |            |
            //   |            |
            //   2 ---------- 3

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
            Program.SetUniform(BorderUniformLocation, RoundLevel);
            Program.SetUniform(ColorUniformLocation, Color, Transparency);

            if (!Fill)
                Program.SetUniform(ContourWidthUniformLocation, ContourWidth);

            base.Draw(Tick);
        }
    }
}
