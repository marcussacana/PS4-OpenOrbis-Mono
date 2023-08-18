using OrbisGL.GL;
using SharpGLES;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Management.Instrumentation;
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
        private readonly List<Line> Lines = new List<Line>();

        public float LineWidth { get; set; } = 1;

        public Line2D(Line[] Lines, bool CloseLines) : this(CloseLines)
        {
            SetLines(Lines);
        }

        public Line2D(bool CloseLines)
        {
            var hProgram = Shader.GetProgram(ResLoader.GetResource("VertexOffsetTexture"), ResLoader.GetResource("FragmentColorUV"));
            Program = new GLProgram(hProgram);

            Program.AddBufferAttribute("Position", AttributeType.Float, AttributeSize.Vector3);
            Program.AddBufferAttribute("uv", AttributeType.Float, AttributeSize.Vector2);

            RenderMode = (int) (CloseLines ? OrbisGL.RenderMode.SingleLine : OrbisGL.RenderMode.MultipleLines);

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
            /*
            if (_Rect != null && Rectangle.IsEmpty())
            {
                SetVisibleRectangle(Rectangle);
                return;
            }
            */

            ClearBuffers();

            if (!Lines.Any())
            {
                base.RefreshVertex();
                return;
            }

            var Rectangles = Lines.Select(x => GetLineRectangle(x.Begin, x.End));
            var MaxRectangle = GetMaxRectangle(Rectangles);

            var ZoomWidth = (int)(Coordinates2D.Width * Zoom);
            var ZoomHeight = (int)(Coordinates2D.Height * Zoom);

            foreach (var Line in Lines)
            {
                var BeginPos = Line.Begin - MaxRectangle.Position;
                var EndPos = Line.End - MaxRectangle.Position;

                //var Rectangle = GetLineRectangle(Line.Begin, Line.End);

                GetLineUV(MaxRectangle, BeginPos, EndPos, out Vector2 UV1, out Vector2 UV2);

                var Begin = new Vector3(XToPoint(BeginPos.X, ZoomWidth), YToPoint(BeginPos.Y, ZoomHeight), -1);
                var End = new Vector3(XToPoint(EndPos.X, ZoomWidth), YToPoint(EndPos.Y, ZoomHeight), -1);
                
                AddArray(Begin);
                AddArray(UV1);
                AddArray(End);
                AddArray(UV2);
            }

            Width = (int)MaxRectangle.Width;
            Height = (int)MaxRectangle.Height;

            base.RefreshVertex();
        }

        private static Rectangle GetMaxRectangle(IEnumerable<Rectangle> Rectangles)
        {
            float MinX = Rectangles.Min(x => x.X);
            float MinY = Rectangles.Min(x => x.Y);
            float MaxX = Rectangles.Max(x => x.Right);
            float MaxY = Rectangles.Max(x => x.Bottom);

            float MaxWidth = Math.Max(MaxX - MinX, 1);
            float MaxHeight = Math.Max(MaxY - MinY, 1);

            return new Rectangle(MinX, MinY, MaxWidth, MaxHeight);
        }

        private void GetLineUV(Rectangle Area, Vector2 XY1, Vector2 XY2, out Vector2 UV1, out Vector2 UV2)
        {
            UV1 = new Vector2(0, 0);
            UV2 = new Vector2(0, 0);

            /*
            UV1.X = XY1.X < XY2.X ? 0 : 1;
            UV2.X = XY1.X < XY2.X ? 1 : 0;
            UV1.Y = XY1.Y < XY2.Y ? 0 : 1;
            UV2.Y = XY1.Y < XY2.Y ? 1 : 0;
            */

            UV1.X = GetU(XY1.X, (int)Area.Width);
            UV2.X = GetU(XY2.X, (int)Area.Width);
            UV1.Y = GetV(XY1.Y, (int)Area.Height);
            UV2.Y = GetV(XY2.Y, (int)Area.Height);
        }

        private Rectangle GetLineRectangle(Vector2 XY1, Vector2 XY2)
        {
            float MinX = Math.Min(XY1.X, XY2.X);
            float MinY = Math.Min(XY1.Y, XY2.Y);
            float MaxX = Math.Max(XY1.X, XY2.X);
            float MaxY = Math.Max(XY1.Y, XY2.Y);

            float Width = MaxX - MinX;
            float Height = MaxY - MinY;

            return new Rectangle(MinX, MinY, Width, Height);
        }

        public override void Draw(long Tick)
        {
            GLES20.LineWidth(LineWidth);//Not supported in PC GLES, but supported in PS4

            base.Draw(Tick);
        }
    }
}
