using OrbisGL.GL;
using SharpGLES;
using System.Numerics;
using static OrbisGL.GL2D.Coordinates2D;

namespace OrbisGL.GL2D
{
    internal class Texture2D : GLObject2D
    {
        int TextureUniformLocation;

        float _Rotate = 0f;
        public float Rotate
        {
            get => _Rotate;
            set
            {
                _Rotate = value;
                RefreshVertex();
            }
        }

        public Texture Texture { get; set; }
        public Texture2D()
        {
            var hProgram = Shader.GetProgram(ResLoader.GetResource("VertexOffsetTexture"), ResLoader.GetResource("FragmentFont"));
            Program = new GLProgram(hProgram);

            TextureUniformLocation = GLES20.GetUniformLocation(hProgram, "Texture");

            Program.AddBufferAttribute("Position", AttributeType.Float, AttributeSize.Vector3);
            Program.AddBufferAttribute("uv", AttributeType.Float, AttributeSize.Vector2);

            RefreshVertex();
        }

        public override void RefreshVertex()
        {
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

            ClearBuffers();

            AddArray(PointA.ToPoint(), -1);//0
            AddArray(0, 0);

            AddArray(PointB.ToPoint(), -1);//1
            AddArray(1, 0);

            AddArray(PointC.ToPoint(), -1);//2
            AddArray(0, 1);

            AddArray(PointD.ToPoint(), -1);//3
            AddArray(1, 1);

            AddIndex(0, 1, 2, 1, 2, 3);

            base.RefreshVertex();
        }
        public override void Draw(long Tick)
        {
            if (Texture != null)
            {
                Program.SetUniform(TextureUniformLocation, Texture.Active());
            }
            base.Draw(Tick);
        }
    }
}
