using OrbisGL.GL;
using SharpGLES;
using static OrbisGL.GL2D.Coordinates2D;

namespace OrbisGL.GL2D
{
    public class RoundedRectangle2D : GLObject2D
    {
        readonly int BorderUniformLocation;
        readonly int ColorUniformLocation;
        public byte Transparecy { get; set; } = 255;

        public RGBColor Color { get; set; } = RGBColor.White;

        public float RoundLevel { get; set; } = 0.8f;
        public RoundedRectangle2D(int Width, int Height)
        {
            var hProg = Shader.GetProgram(ResLoader.GetResource("VertexOffsetTexture"), ResLoader.GetResource("FragmentColorRounded"));
            Program = new GLProgram(hProg);

            Program.AddBufferAttribute("Position", AttributeType.Float, AttributeSize.Vector3);
            Program.AddBufferAttribute("uv", AttributeType.Float, AttributeSize.Vector2);

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

            if (true)
            {
                AddIndex(0, 1, 2, 1, 2, 3);
                RenderMode = (int)OrbisGL.RenderMode.Triangle;
            }
            else
            {
                AddIndex(0, 1, 3, 2);
                RenderMode = (int)OrbisGL.RenderMode.ClosedLine;
            }

            BorderUniformLocation = GLES20.GetUniformLocation(Program.Handler, "Border");
            ColorUniformLocation = GLES20.GetUniformLocation(Program.Handler, "Color");

            Program.SetUniform("Resolution", (float)Width, (float)Height);
        }

        public override void Draw(long Tick)
        {
            Program.SetUniform(BorderUniformLocation, RoundLevel);
            Program.SetUniform(ColorUniformLocation, Color, Transparecy);
            base.Draw(Tick);
        }
    }
}
