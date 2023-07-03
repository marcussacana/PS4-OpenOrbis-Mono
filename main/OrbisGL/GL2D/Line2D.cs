using OrbisGL.GL;
using SharpGLES;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static OrbisGL.GL2D.Coordinates2D;
using Rectangle = OrbisGL.GL.Rectangle;

namespace OrbisGL.GL2D
{
    public class Line2D : GLObject2D
    {
        public RGBColor Color { get; set; } = RGBColor.Red;

        private readonly List<Line> Lines = new List<Line>();

        public byte Transparency { get; set; } = 255;

        public float LineWidth { get; set; } = 1;

        int ColorUniformLocation = -1;

        public Line2D(Line[] Lines, bool CloseLines) : this(CloseLines)
        {
            SetLines(Lines);
        }

        public Line2D(bool CloseLines)
        {
            var hProgram = Shader.GetProgram(ResLoader.GetResource("VertexOffset"), ResLoader.GetResource("FragmentColor"));
            Program = new GLProgram(hProgram);

            Program.AddBufferAttribute("Position", AttributeType.Float, AttributeSize.Vector3);

            RenderMode = (int) (CloseLines ? OrbisGL.RenderMode.SingleLine : OrbisGL.RenderMode.MultipleLines);

            ColorUniformLocation = GLES20.GetUniformLocation(Program.Handler, "Color");

            RefreshVertex();
        }

        public Line[] GetLines()
        {
            return Lines.ToArray();
        }

        public void SetLines(Line[] Lines)
        {
            this.Lines.Clear();
            this.Lines.AddRange(Lines);
            RefreshVertex();
        }

        public override void RefreshVertex()
        {
            ClearBuffers();

            foreach (var Line in Lines)
            {
                var Begin = new Vector3(XToPoint(Line.Begin.X), YToPoint(Line.Begin.Y), -1);
                var End = new Vector3(XToPoint(Line.End.X), YToPoint(Line.End.Y), -1);

                AddArray(Begin, End);
            }

            base.RefreshVertex();
        }

        public override void SetVisibleRectangle(Rectangle Rectangle)
        {
            ClearBuffers();

            foreach (var Line in Lines)
            {
                var vBegin = Line.Begin;
                var vEnd = Line.End;

                bool Intersected = Extend(Rectangle, ref vBegin, ref vEnd);

                if (!Intersected) {

                    if (Rectangle.IsInBounds(vBegin))
                        continue;

                    if (Rectangle.IsInBounds(vEnd))
                        continue;
                }

                var Begin = new Vector3(XToPoint(vBegin.X), YToPoint(vBegin.Y), -1);
                var End = new Vector3(XToPoint(vEnd.X), YToPoint(vEnd.Y), -1);

                AddArray(Begin, End);
            }

            SetChildrenVisibleRectangle(Rectangle);
        }


        // Stolen from: https://stackoverflow.com/a/7337243/4860216
        bool Intersection(Vector2 a1, Vector2 a2, Vector2 b1, PointF b2, ref Vector2 ans)
        {
            float x = ((a1.X * a2.Y - a1.Y * a2.X) * (b1.X - b2.X) - (a1.X - a2.X) * (b1.X * b2.Y - b1.Y * b2.X)) / ((a1.X - a2.X) * (b1.Y - b2.Y) - (a1.Y - a2.Y) * (b1.X - b2.X));
            float y = ((a1.X * a2.Y - a1.Y * a2.X) * (b1.Y - b2.Y) - (a1.Y - a2.Y) * (b1.X * b2.Y - b1.Y * b2.X)) / ((a1.X - a2.X) * (b1.Y - b2.Y) - (a1.Y - a2.Y) * (b1.X - b2.X));

            if (x == float.NaN || x == float.PositiveInfinity || x == float.NegativeInfinity || y == float.NaN || y == float.PositiveInfinity || y == float.NegativeInfinity)
            { 
                // the lines are equal or never intersect
                return false;
            }
            ans.X = x;
            ans.Y = y;
            return true;
        }

        bool Extend(Rectangle bounds, ref Vector2 start, ref Vector2 end)
        {

            List<Vector2> ansFinal = new List<Vector2>();
            var ansLeft = new Vector2();
            bool hitLeft = Intersection(start, end, new Vector2(bounds.X, bounds.Y), new Vector2(bounds.X, bounds.Y + bounds.Height), ref ansLeft);
            if (hitLeft && (ansLeft.Y < bounds.Y || ansLeft.Y > bounds.Y + bounds.Height)) hitLeft = false;
            if (hitLeft) ansFinal.Add(ansLeft);

            var ansTop = new Vector2();
            bool hitTop = Intersection(start, end, new Vector2(bounds.X, bounds.Y), new Vector2(bounds.X + bounds.Width, bounds.Y), ref ansTop);
            if (hitTop && (ansTop.X < bounds.X || ansTop.X > bounds.X + bounds.Width)) hitTop = false;
            if (hitTop) ansFinal.Add(ansTop);

            var ansRight = new Vector2();
            bool hitRight = Intersection(start, end, new Vector2(bounds.X + bounds.Width, bounds.Y), new Vector2(bounds.X + bounds.Width, bounds.Y + bounds.Height), ref ansRight);
            if (hitRight && (ansRight.Y < bounds.Y || ansRight.Y > bounds.Y + bounds.Height)) hitRight = false;
            if (hitRight) ansFinal.Add(ansRight);

            var ansBottom = new Vector2();
            bool hitBottom = Intersection(start, end, new Vector2(bounds.X, bounds.Y + bounds.Height), new Vector2(bounds.X + bounds.Height, bounds.Y + bounds.Height), ref ansBottom);
            if (hitBottom && (ansBottom.X < bounds.X || ansBottom.X > bounds.X + bounds.Width)) hitBottom = false;
            if (hitBottom) ansFinal.Add(ansBottom);

            if (!hitLeft && !hitTop && !hitRight && !hitBottom)
            {
                return false;
            }


            Vector2[] ans = ansFinal.Distinct().ToArray();
            if(ans.Length < 2)
            {
                throw new Exception("Corner case *wink*");
            }
            start.X = ans[0].X; start.Y = ans[0].Y;
            end.X = ans[1].X; end.Y = ans[1].Y;

            return true;
        }

        public override void Draw(long Tick)
        {
            Program.SetUniform(ColorUniformLocation, Color, Transparency);

            GLES20.LineWidth(LineWidth);//Not supported in PC GLES, but supported in PS4

            base.Draw(Tick);
        }
    }
}
