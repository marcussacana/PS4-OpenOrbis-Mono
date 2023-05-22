using OrbisGL.GL;
using static OrbisGL.GL2D.Coordinates2D;

namespace OrbisGL.GL2D
{
    public class Rectangle2D : GLObject2D
    {
        public byte Transparecy { get; set; } = 255;

        public RGBColor Color { get; set; } = RGBColor.White;
        public Rectangle2D(int Width, int Height, bool Fill) : this(Width, Height, Fill, null)
        {

        }
        public Rectangle2D(int Width, int Height, bool Fill, string CustomFragmentShader = null)
        {
            var hProgram = Shader.GetProgram(ResLoader.GetResource("VertexOffset"), CustomFragmentShader ?? ResLoader.GetResource("FragmentColor"));
            Program = new GLProgram(hProgram);

            Program.AddBufferAttribute("Position", AttributeType.Float, AttributeSize.Vector3);

            //   0 ---------- 1
            //   |            |
            //   |            |
            //   |            |
            //   2 ---------- 3

            AddArray(XToPoint(0),     YToPoint(0),      0);//0
            AddArray(XToPoint(Width), YToPoint(0),      0);//1
            AddArray(XToPoint(0),     YToPoint(Height), 0);//2
            AddArray(XToPoint(Width), YToPoint(Height), 0);//3

            if (Fill)
            {
                AddIndex(0, 1, 2, 1, 2, 3);
                RenderMode = (int)OrbisGL.RenderMode.Triangle;
            }
            else
            {
                AddIndex(0, 1, 3, 2);
                RenderMode = (int)OrbisGL.RenderMode.ClosedLine;
            }
        }

        public Rectangle2D(GLObject2D Parent, int Width, int Height, bool Fill) : this (Width, Height, Fill)
        {
            this.Parent = Parent;
        }

        public override void Draw(long Tick)
        {
            Program.SetUniform("Color", Color, Transparecy);

            base.Draw(Tick);
        }
    }
}
