using OrbisGL.GL;
using SharpGLES;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static OrbisGL.GL2D.Coordinates2D;

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

        public override void Draw(long Tick)
        {
            Program.SetUniform(ColorUniformLocation, Color, Transparency);

            GLES20.LineWidth(LineWidth);

            base.Draw(Tick);
        }
    }
}
