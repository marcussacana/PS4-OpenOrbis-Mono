using OrbisGL.GL;
using SharpGLES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace OrbisGL.GL2D
{
    public abstract class GLObject2D : GLObject
    {
        public int Width { get; set; }
        public int Height { get; set; }

        protected GLObject2D Parent = null;
        public bool InRoot => Parent == null;

        public IEnumerable<GLObject2D> Childs => Children;

        private List<GLObject2D> Children = new List<GLObject2D>();

        /// <summary>
        /// Represent an offset of the object drawing location,
        /// Calculate the offset with <see cref="Coordinates2D.XOffset">XOffset</see> and <see cref="Coordinates2D.YOffset">YOffset</see>
        /// </summary>
        protected Vector2 Offset { get; set; }


        private Vector2 _Position;

        public Vector2 Position
        {
            get => _Position; 
            set
            {
                _Position = value;
                Offset = new Vector2(Coordinates2D.XOffset * value.X, Coordinates2D.YOffset * value.Y);
            }
        }

        protected Vector2 AbsoluteOffset => Parent?.AbsoluteOffset + Offset ?? Offset;

        int OffsetUniform = int.MinValue;
        int VisibleUniform = int.MinValue;

        public void UpdateUniforms()
        {
            if (OffsetUniform >= 0)
            {
                Program.SetUniform(OffsetUniform, AbsoluteOffset.X, AbsoluteOffset.Y, 1);
            }
            else if (OffsetUniform == int.MinValue)
            {
                OffsetUniform = GLES20.GetUniformLocation(Program.Handler, "Offset");
                Program.SetUniform(OffsetUniform, AbsoluteOffset.X, AbsoluteOffset.Y, 1);
            }

            if (VisibleUniform >= 0)
            {
                Program.SetUniform(VisibleUniform, VisibleRect);
            }
            else if (VisibleUniform == int.MinValue)
            {
                VisibleUniform = GLES20.GetUniformLocation(Program.Handler, "VisibleRect");
                Program.SetUniform(VisibleUniform, VisibleRect);
            }
        }

        private Vector4 VisibleRect = Vector4.Zero;

        //cut with uv not working, try with shader
        public virtual void SetVisibleRectangle(float X, float Y, int Width, int Height)
        {
            float MinU = Coordinates2D.GetU(X, this.Width);
            float MaxU = Coordinates2D.GetU(Width, this.Width);

            float MinV = Coordinates2D.GetV(Y, this.Height);
            float MaxV = Coordinates2D.GetV(Height, this.Height);

            VisibleRect = new Vector4(MinU, MinV, MaxU, MaxV);

            SetChildrenVisibleRectangle(X, Y, Width, Height);
        }

        public virtual void ClearVisibleRectangle()
        {
            VisibleRect = Vector4.Zero;

            ClearChildrenVisibleRectangle();
        }

        protected void SetChildrenVisibleRectangle(float X, float Y, int Width, int Height)
        {
            foreach (var Child in Childs)
            {
                float ChildX = Math.Max(0, X - Child.Position.X);
                float ChildY = Math.Max(0, Y - Child.Position.Y);

                int ChildWidth = Math.Min(Child.Width, Width - (int)Child.Position.X);
                int ChildHeight = Math.Min(Child.Height, Height - (int)Child.Position.Y);

                Child.SetVisibleRectangle(ChildX, ChildY, ChildWidth, ChildHeight);
            }
        }

        protected void ClearChildrenVisibleRectangle()
        {
            foreach (var Child in Childs)
            {
                Child.ClearVisibleRectangle();
            }
        }

        public virtual void RefreshVertex()
        {
            foreach (var Child in Childs)
                Child.RefreshVertex();
        }

        public override void Draw(long Tick)
        {
            if (Program != null)
            {
                UpdateUniforms();
                base.Draw(Tick);
            }

            foreach (var Child in Children)
            {
                Child.Draw(Tick);
            }
        }

        public void AddChild(GLObject2D Child)
        {
            Children.Add(Child);
            Child.Parent = this;
            Child.RefreshVertex();
        }

        public void RemoveChild(GLObject2D Child)
        {
            Children.Remove(Child);
            Child.Parent = null;
            Child.RefreshVertex();
        }

        public void RemoveChildren(bool Dispose)
        {
            foreach (var Child in Children)
            {
                Child.Parent = null;

                if (Dispose)
                    Child.Dispose();
                else
                    Child.RefreshVertex();
            }

            Children.Clear();
        }
    }
}
