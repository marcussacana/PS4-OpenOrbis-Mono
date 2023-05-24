using OrbisGL.GL;
using SharpGLES;
using static OrbisGL.GL2D.Coordinates2D;

namespace OrbisGL.GL2D
{
    public class Elipse2D : GLObject2D
    {
        readonly int AntiAlisingUniformLocation;
        readonly int ColorUniformLocation;
        public byte Transparecy { get; set; } = 255;

        public RGBColor Color { get; set; } = RGBColor.White;

        public float AntiAlising { get; set; } = 6f;
        public Elipse2D(int Width, int Height)
        {
            var hProg = Shader.GetProgram(ResLoader.GetResource("VertexOffsetTexture"), ResLoader.GetResource("FragmentColorElipse"));
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

            AddIndex(0, 1, 2, 1, 2, 3);
            RenderMode = (int)OrbisGL.RenderMode.Triangle;

            AntiAlisingUniformLocation = GLES20.GetUniformLocation(Program.Handler, "AntiAlising");
            ColorUniformLocation = GLES20.GetUniformLocation(Program.Handler, "Color");

            Program.SetUniform("Resolution", (float)Width, (float)Height);
        }

        public override void Draw(long Tick)
        {
            Program.SetUniform(AntiAlisingUniformLocation, AntiAlising);
            Program.SetUniform(ColorUniformLocation, Color, Transparecy);
            base.Draw(Tick);
        }
    }
}
