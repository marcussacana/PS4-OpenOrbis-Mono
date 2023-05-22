using OrbisGL.GL;
using SharpGLES;
using System.Numerics;

namespace OrbisGL.GL2D
{
    public class GLObject2D : GLObject
    {
        protected GLObject2D Parent = null;
        public bool InRoot => Parent == null;

        /// <summary>
        /// Represent an offset of the object drawing location,
        /// Calculate the offset with <see cref="Coordinates2D.XOffset">XOffset</see> and <see cref="Coordinates2D.YOffset">YOffset</see>
        /// </summary>
        public Vector3 Offset { get; set; }

        protected Vector3 AbsoluteOffset => Parent?.AbsoluteOffset + Offset ?? Offset;


        int OffsetUniform = int.MinValue;

        public void UpdateUniforms()
        {
            if (OffsetUniform >= 0)
            {
                Program.SetUniform(OffsetUniform, AbsoluteOffset);
            }
            else if (OffsetUniform == int.MinValue)
            {
                OffsetUniform = GLES20.GetUniformLocation(Program.Handler, "Offset");
                Program.SetUniform(OffsetUniform, AbsoluteOffset);
            }
        }

        public override void Draw(long Tick)
        {
            UpdateUniforms();
            base.Draw(Tick);
        }
    }
}
