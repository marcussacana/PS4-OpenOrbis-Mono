using OrbisGL.GL;
using System.Numerics;
using static OrbisGL.GL2D.Coordinates2D;

namespace OrbisGL.GL2D
{
    public class Rectangle2D : GLObject2D
    {
        bool FillMode;

        public Rectangle2D(Rectangle Rectangle, bool Fill) : this((int)Rectangle.Width, (int)Rectangle.Height, Fill)
        {
            Position = new Vector2(Rectangle.X, Rectangle.Y);
        }

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

        public override void SetVisibleRectangle(Rectangle Rectangle)
        {
            ClearBuffers();

            //   0 ---------- 1
            //   |            |
            //   |            |
            //   |            |
            //   2 ---------- 3

            AddArray(XToPoint(Rectangle.X), YToPoint(Rectangle.Y), -1);//0
            AddArray(XToPoint(Rectangle.Width), YToPoint(Rectangle.Y), -1);//1
            AddArray(XToPoint(Rectangle.X), YToPoint(Rectangle.Height), -1);//2
            AddArray(XToPoint(Rectangle.Width), YToPoint(Rectangle.Height), -1);//3

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

            SetChildrenVisibleRectangle(Rectangle);
        }

        public override void ClearVisibleRectangle()
        {
            RefreshVertex();
            ClearChildrenVisibleRectangle();
        }
    }
}
