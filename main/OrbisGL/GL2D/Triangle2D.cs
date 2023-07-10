using OrbisGL.GL;
using SharpGLES;
using SixLabors.ImageSharp;
using System.Numerics;
using static OrbisGL.GL2D.Coordinates2D;

namespace OrbisGL.GL2D
{
    public class Triangle2D : GLObject2D
    {
        private readonly int RotateUniformLocation;
        private readonly int BorderUniformLocation;
        private readonly int ColorUniformLocation;

        public float RoundLevel { get; set; } = 0.0f;
        public RGBColor Color { get; set; } = RGBColor.White;
        public byte Transparency { get; set; } = 255;

        public Degrees Rotation { get; set; } = 0;

        public enum Degrees : int {
            Degree0 = 0,
            Degree45 = 5,
            Degree90 = 10,
            Degree135 = 15,
            Degree180 = 20,
            Degree225 = 25,
            Degree270 = 30,
            Degree315 = 35
        }

        public Triangle2D(GL.Rectangle Rectangle) : this((int)Rectangle.Width, (int)Rectangle.Height)
        {
            Position = new Vector2(Rectangle.X, Rectangle.Y);
        }
        public Triangle2D(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;

            var hProg = Shader.GetProgram(ResLoader.GetResource("VertexOffsetTexture"), ResLoader.GetResource("FragmentColorRoundedTriangle"));
            Program = new GLProgram(hProg);

            Program.AddBufferAttribute("Position", AttributeType.Float, AttributeSize.Vector3);
            Program.AddBufferAttribute("uv", AttributeType.Float, AttributeSize.Vector2);

            RenderMode = (int)OrbisGL.RenderMode.Triangle;

            BorderUniformLocation = GLES20.GetUniformLocation(Program.Handler, "Border");
            RotateUniformLocation = GLES20.GetUniformLocation(Program.Handler, "Rotate");
            ColorUniformLocation = GLES20.GetUniformLocation(Program.Handler, "Color");

            Program.SetUniform("Resolution", (float)Width, (float)Height);

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
            Program.SetUniform(RotateUniformLocation, (float)Rotation / 10);
            Program.SetUniform(BorderUniformLocation, RoundLevel);
            Program.SetUniform(ColorUniformLocation, Color, Transparency);

            base.Draw(Tick);
        }
    }
}
