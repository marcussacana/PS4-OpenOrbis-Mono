using OrbisGL.GL;
using System.Numerics;
using SharpGLES;
using static OrbisGL.GL2D.Coordinates2D;

namespace OrbisGL.GL2D
{
    public class Rectangle2D : GLObject2D
    {
        bool FillMode;

        float _Rotate = 0f;
        public float Rotate
        {
            get => _Rotate;
            set {
                _Rotate = value;
                RefreshVertex();
            }
        }

        public float ContourWidth { get; set; } = 1.0f;

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
            if (VisibleRectangle != null)
            {
                SetVisibleRectangle(VisibleRectangle.Value);
                return;
            }

            ClearBuffers();

            //   0 ---------- 1
            //   |            |
            //   |            |
            //   |            |
            //   2 ---------- 3


            var PointA = new Vector2(0, 0);
            var PointB = new Vector2(Width, 0);
            var PointC = new Vector2(0, Height);
            var PointD = new Vector2(Width, Height);

            var Center = PointD / 2f;

            PointA = RotatePoint(PointA, Center, Rotate);
            PointB = RotatePoint(PointB, Center, Rotate);
            PointC = RotatePoint(PointC, Center, Rotate);
            PointD = RotatePoint(PointD, Center, Rotate);

            AddArray(PointA.ToPoint(), -1);//0
            AddArray(PointB.ToPoint(), -1);//1
            AddArray(PointC.ToPoint(), -1);//2
            AddArray(PointD.ToPoint(), -1);//3

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


            var PointA = new Vector2(Rectangle.X, Rectangle.Y);
            var PointB = new Vector2(Rectangle.Width, Rectangle.Y);
            var PointC = new Vector2(Rectangle.X, Rectangle.Height);
            var PointD = new Vector2(Rectangle.Width, Rectangle.Height);

            var Center = PointD / 2f;

            PointA = RotatePoint(PointA, Center, Rotate);
            PointB = RotatePoint(PointB, Center, Rotate);
            PointC = RotatePoint(PointC, Center, Rotate);
            PointD = RotatePoint(PointD, Center, Rotate);

            AddArray(PointA.ToPoint(), -1);//0
            AddArray(PointB.ToPoint(), -1);//1
            AddArray(PointC.ToPoint(), -1);//2
            AddArray(PointD.ToPoint(), -1);//3

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

        public override void Draw(long Tick)
        {
            if (!FillMode)
                GLES20.LineWidth(ContourWidth);
            
            base.Draw(Tick);
        }
    }
}
