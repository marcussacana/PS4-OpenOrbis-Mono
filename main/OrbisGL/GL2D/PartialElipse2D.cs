using OrbisGL.GL;
using SharpGLES;
using System;
using static OrbisGL.GL2D.Coordinates2D;

namespace OrbisGL.GL2D
{
    public class PartialElipse2D : GLObject2D
    {
        readonly int CircleConfigUniformLocation;

        /// <summary>
        /// From 0.00 to 1.00, when 1 a pizza like geometry will be rendered
        /// </summary>
        public float Thickness { get; set; } = 0.1f;

        /// <summary>
        /// From -3.14 to 3.14, the min/max value represents the center left angle of the circle.
        /// </summary>
        public float StartAngle { get; set; } = -(float)Math.PI;

        /// <summary>
        /// From 3.14 to -3.14, the min/max value represents the center left angle of the circle.
        /// </summary>
        public float EndAngle { get; set; } = (float)Math.PI;


        /// <summary>
        /// Whe true the <see cref="Thickness"/> is ignored, same effect of set <see cref="Thickness"/> to 1.0
        /// </summary>
        public bool Fill { get; private set; }

        public PartialElipse2D(int Width, int Height, bool Fill)
        {
            this.Fill = Fill;
            this.Width = Width;
            this.Height = Height;

            var hProg = Shader.GetProgram(ResLoader.GetResource("VertexOffsetTexture"), ResLoader.GetResource("FragmentColorElipsePartial"));
            Program = new GLProgram(hProg);

            Program.AddBufferAttribute("Position", AttributeType.Float, AttributeSize.Vector3);
            Program.AddBufferAttribute("uv", AttributeType.Float, AttributeSize.Vector2);

            RenderMode = (int)OrbisGL.RenderMode.Triangle;

            CircleConfigUniformLocation = GLES20.GetUniformLocation(Program.Handler, "CircleConfig");//vec3(startAngle, endAngle, Thickness)

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

            var ZoomWidth = (int)(Coordinates2D.Width * Zoom);
            var ZoomHeight = (int)(Coordinates2D.Height * Zoom);

            AddArray(XToPoint(0, ZoomWidth), YToPoint(0, ZoomHeight), -1);//0
            AddArray(0, 0);

            AddArray(XToPoint(Width, ZoomWidth), YToPoint(0, ZoomHeight), -1);//1
            AddArray(1, 0);

            AddArray(XToPoint(0, ZoomWidth), YToPoint(Height, ZoomHeight), -1);//2
            AddArray(0, 1);

            AddArray(XToPoint(Width, ZoomWidth), YToPoint(Height, ZoomHeight), -1);//3
            AddArray(1, 1);

            AddIndex(0, 1, 2, 1, 2, 3);

            base.RefreshVertex();
        }

        public override void Draw(long Tick)
        {
            if (Fill)
                Program.SetUniform(CircleConfigUniformLocation, StartAngle, EndAngle, 1.0f);
            else
                Program.SetUniform(CircleConfigUniformLocation, StartAngle, EndAngle, Thickness);

            base.Draw(Tick);
        }
    }
}
