using OrbisGL.GL;
using OrbisGL.GL2D;
using SharpGLES;
using System;
using System.Numerics;
using static OrbisGL.GL2D.Coordinates2D;
using V = System.Numerics.Vector2;

namespace OrbisGL.Input
{
    internal class Cursor : GLObject2D
    {
        private bool IsContour;

        private Cursor Contour;
        public Cursor() : this(false)
        {
            Contour = new Cursor(true);
            Contour.Opacity = 100;
            AddChild(Contour);
        }

        private Cursor(bool Contour)
        {
            var hProg = Shader.GetProgram(ResLoader.GetResource("VertexOffset"), ResLoader.GetResource("FragmentColor"));
            Program = new GLProgram(hProg);

            Program.AddBufferAttribute("Position", AttributeType.Float, AttributeSize.Vector3);

            IsContour = Contour;

            RenderMode = Contour ? (int)OrbisGL.RenderMode.ClosedLine : (int)OrbisGL.RenderMode.Triangle;

            RefreshVertex();
        }

        public override void RefreshVertex()
        {
            ClearBuffers();

            float Height = Math.Max(this.Width, this.Height);//19
            float Width = Height * 0.631579f;                //12



            //Yeah, I calculated every point manually
            var Points = new V[] {
                new V(0,                  0),                  //0
                new V(0,                  Height * 0.7894739f),//1
                new V(Width * 0.6666667f, Height * 0.421053f), //2
                new V(Width,              Height * 0.578948f), //3
                new V(Width * 0.3333334f, Height * 0.578948f), //4
                new V(Width * 0.6666667f, Height),             //5
                new V(Width * 0.8300000f, Height * 0.942106f), //6
                new V(Width * 0.4166667f, Height * 0.421053f), //7
                new V(Width * 0.5530000f, Height * 0.579000f), //8
                new V(Width * 0.3500000f, Height * 0.600000f), //9
            };


            for (int i = 0; i < Points.Length; i++)
            {
                var cPoint = Points[i];
                var nPoint = new V(XToPoint(cPoint.X), YToPoint(cPoint.Y));
                AddArray(new Vector3(nPoint, -1));
            }


            if (IsContour)
                AddIndex(0, 1, 9,    5, 6, 8,    3);
            else
                AddIndex(0, 1, 2,    0, 3, 4,    4, 5, 7,   5, 6, 7);

            if (!IsContour && Contour != null)
            {
                Contour.Width = this.Width;
                Contour.Height = this.Height;
            }

            base.RefreshVertex();
        }

        public override void Draw(long Tick)
        {
            if (!IsContour)
                Contour.Color = Color.Negative();
            else
                GLES20.LineWidth(0.5f);

            base.Draw(Tick);
        }
    }
}
