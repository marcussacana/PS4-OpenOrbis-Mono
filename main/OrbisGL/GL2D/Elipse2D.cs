using OrbisGL.GL;
using SharpGLES;
using static OrbisGL.GL2D.Coordinates2D;

namespace OrbisGL.GL2D
{
    public class Elipse2D : GLObject2D
    {
        readonly int AntiAliasingUniformLocation;
        readonly int ContourWidthUniformLocation;

        public float AntiAliasing { get; set; } = 6f;
        public float ContourWidth { get; set; } = 1.0f;

        public bool Fill { get; private set; }
        public Elipse2D(int Width, int Height, bool Fill)
        {
            this.Fill = Fill;
            this.Width = Width;
            this.Height = Height;

            var hProg = Shader.GetProgram(ResLoader.GetResource("VertexOffsetTexture"), ResLoader.GetResource(Fill ? "FragmentColorElipse" : "FragmentColorElipseContour"));
            Program = new GLProgram(hProg);

            Program.AddBufferAttribute("Position", AttributeType.Float, AttributeSize.Vector3);
            Program.AddBufferAttribute("uv", AttributeType.Float, AttributeSize.Vector2);

            RenderMode = (int)OrbisGL.RenderMode.Triangle;

            AntiAliasingUniformLocation = GLES20.GetUniformLocation(Program.Handler, "AntiAliasing");
            ContourWidthUniformLocation = GLES20.GetUniformLocation(Program.Handler, "ContourWidth");

            RefreshVertex();
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

            base.RefreshVertex();
        }

        public override void Draw(long Tick)
        {
            if (Fill)
                Program.SetUniform(AntiAliasingUniformLocation, AntiAliasing);
            else
                Program.SetUniform(ContourWidthUniformLocation, ContourWidth);

            base.Draw(Tick);
        }
    }
}
