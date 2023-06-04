﻿using OrbisGL.GL;
using SharpGLES;
using static OrbisGL.GL2D.Coordinates2D;

namespace OrbisGL.GL2D
{
    internal class Texture2D : GLObject2D
    {
        int TextureUniformLocation;
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

            ClearBuffers();

            AddArray(XToPoint(0), YToPoint(0), -1);//0
            AddArray(MinU, MinV);


            AddArray(XToPoint(Width), YToPoint(0), -1);//1
            AddArray(MaxU, MinV);

            AddArray(XToPoint(0), YToPoint(Height), -1);//2
            AddArray(MinU, MaxV);

            AddArray(XToPoint(Width), YToPoint(Height), -1);//3
            AddArray(MaxU, MaxV);


            AddIndex(0, 1, 2, 1, 2, 3);
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
