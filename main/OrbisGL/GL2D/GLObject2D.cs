using OrbisGL.GL;
using SharpGLES;
using System.Numerics;
using System.Security.Cryptography;

namespace OrbisGL.GL2D
{
    public abstract class GLObject2D : GLObject
    {
        public int Width { get; set; }
        public int Height { get; set; }

        protected GLObject2D Parent = null;
        public bool InRoot => Parent == null;

        /// <summary>
        /// Represent an offset of the object drawing location,
        /// Calculate the offset with <see cref="Coordinates2D.XOffset">XOffset</see> and <see cref="Coordinates2D.YOffset">YOffset</see>
        /// </summary>
        protected Vector3 Offset { get; set; }


        private Vector3 _Position;

        public Vector3 Position
        {
            get => _Position; 
            set
            {
                _Position = value;
                Offset = new Vector3(Coordinates2D.XOffset * value.X, Coordinates2D.YOffset * value.Y, value.Z);
            }
        }

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

        protected float MinU = 0;
        protected float MaxU = 1;
        protected float MinV = 0;
        protected float MaxV = 1;

        public virtual void SetVisibleRectangle(int X, int Y, int Width, int Height)
        {
            MinU = Coordinates2D.GetU(X, this.Width);
            MaxU = Coordinates2D.GetU(X + Width, this.Width);

            MinV = Coordinates2D.GetV(Y, this.Height);
            MaxV = Coordinates2D.GetV(Y + Height, this.Height);

            RefreshVertex();
        }

        public virtual void ClearVisibleRectangle()
        {
            MinU = 0;
            MaxU = 1;
            MinV = 0;
            MaxV = 1;

            RefreshVertex();
        }

        public abstract void RefreshVertex();

        public override void Draw(long Tick)
        {
            UpdateUniforms();
            base.Draw(Tick);
        }
    }
}
