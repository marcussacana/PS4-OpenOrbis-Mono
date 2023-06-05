using OrbisGL.GL;
using static OrbisGL.GL2D.Coordinates2D;

namespace OrbisGL.GL2D
{
    public class Rectangle2D : GLObject2D
    {
        bool FillMode;

        public byte Transparecy { get; set; } = 255;

        public RGBColor Color { get; set; } = RGBColor.White;
        public Rectangle2D(int Width, int Height, bool Fill) : this(Width, Height, Fill, null)
        {

        }
        public Rectangle2D(int Width, int Height, bool Fill, string CustomFragmentShader = null)
        {
            this.Width = Width;
            this.Height = Height;

            var hProgram = Shader.GetProgram(ResLoader.GetResource("VertexOffset"), CustomFragmentShader ?? ResLoader.GetResource("FragmentColor"));
            Program = new GLProgram(hProgram);

            Program.AddBufferAttribute("Position", AttributeType.Float, AttributeSize.Vector3);

            FillMode = Fill;

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
            AddArray(XToPoint(Width), YToPoint(0), -1);//1
            AddArray(XToPoint(0), YToPoint(Height), -1);//2
            AddArray(XToPoint(Width), YToPoint(Height), -1);//3

            if (FillMode)
            {
                AddIndex(0, 1, 2, 1, 2, 3);
                RenderMode = (int)OrbisGL.RenderMode.Triangle;
            }
            else
            {
                AddIndex(0, 1, 3, 2);
                RenderMode = (int)OrbisGL.RenderMode.ClosedLine;
            }

            base.RefreshVertex();
        }

        public override void SetVisibleRectangle(float X, float Y, int Width, int Height)
        {
            ClearBuffers();

            //   0 ---------- 1
            //   |            |
            //   |            |
            //   |            |
            //   2 ---------- 3

            AddArray(XToPoint(X), YToPoint(Y), -1);//0
            AddArray(XToPoint(Width), YToPoint(Y), -1);//1
            AddArray(XToPoint(X), YToPoint(Height), -1);//2
            AddArray(XToPoint(Width), YToPoint(Height), -1);//3

            if (FillMode)
            {
                AddIndex(0, 1, 2, 1, 2, 3);
                RenderMode = (int)OrbisGL.RenderMode.Triangle;
            }
            else
            {
                AddIndex(0, 1, 3, 2);
                RenderMode = (int)OrbisGL.RenderMode.ClosedLine;
            }

            SetChildrenVisibleRectangle(X, Y, Width, Height);
        }

        public override void ClearVisibleRectangle()
        {
            RefreshVertex();
            ClearChildrenVisibleRectangle();
        }

        public override void Draw(long Tick)
        {
            Program.SetUniform("Color", Color, Transparecy);

            base.Draw(Tick);
        }
    }
}
