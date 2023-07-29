using OrbisGL.GL;
using SharpGLES;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;

namespace OrbisGL.GL2D
{
    public abstract class GLObject2D : GLObject
    {
        public bool Visible { get; set; } = true;

        public int Width { get; set; }
        public int Height { get; set; }
        public bool InRoot => Parent == null;
        public RGBColor Color { get; set; } = RGBColor.White;
        public byte Opacity { get; set; } = 255; 

        public IEnumerable<GLObject2D> Childs => Children;

        public Vector2 Position
        {
            get => _Position;
            set
            {
                _Position = value;
                Offset = new Vector2(Coordinates2D.XOffset * value.X, Coordinates2D.YOffset * value.Y);
            }
        }

        public Rectangle? VisibleRectangle { get; protected set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle(Position.X, Position.Y, Width, Height);
            }
            set 
            {
                Position = value.Position;
                Width = (int)value.Width;
                Height = (int)value.Height;
            }
        }


        protected GLObject2D Parent = null;

        private List<GLObject2D> Children = new List<GLObject2D>();

        /// <summary>
        /// Represent an offset of the object drawing location,
        /// Calculate the offset with <see cref="Coordinates2D.XOffset">XOffset</see> and <see cref="Coordinates2D.YOffset">YOffset</see>
        /// </summary>
        protected Vector2 Offset { get; set; }


        private Vector2 _Position;

        protected Vector2 AbsoluteOffset => Parent?.AbsoluteOffset + Offset ?? Offset;

        protected Vector2 AbsolutePosition => Parent?.AbsolutePosition + Position ?? Position;


        int OffsetUniform = int.MinValue;
        int VisibleUniform = int.MinValue;
        int ColorUniform = int.MinValue;
        int ResolutionUniform = int.MinValue;

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
                Program.SetUniform(VisibleUniform, VisibleRectUV);
            }
            else if (VisibleUniform == int.MinValue)
            {
                VisibleUniform = GLES20.GetUniformLocation(Program.Handler, "VisibleRect");
                Program.SetUniform(VisibleUniform, VisibleRectUV);
            }

            if (ColorUniform >= 0)
            {
                Program.SetUniform(ColorUniform, Color, Opacity);
            }
            else if (ColorUniform == int.MinValue)
            {
                ColorUniform = GLES20.GetUniformLocation(Program.Handler, "Color");
                Program.SetUniform(ColorUniform, Color, Opacity);
            }

            if (ResolutionUniform >= 0)
            {
                Program.SetUniform(ResolutionUniform, (float)Width, Height);
            }
            else if (ResolutionUniform == int.MinValue)
            {
                ResolutionUniform = GLES20.GetUniformLocation(Program.Handler, "Resolution");
                Program.SetUniform(ResolutionUniform, (float)Width, Height);
            }
        }

        private bool InvisibleRect = false;
        private Rectangle VisibleRectUV = Vector4.Zero;

        public void SetVisibleRectangle(float X, float Y, int Width, int Height) => SetVisibleRectangle(new Rectangle(X, Y, Width, Height));
        public virtual void SetVisibleRectangle(Rectangle Parent)
        {
            if (Parent.IsEmpty())
            { 
                InvisibleRect = true;
                return;
            }

            InvisibleRect = false;

            float MinU = Coordinates2D.GetU(Parent.X, Width);
            float MaxU = Coordinates2D.GetU(Parent.Width, Width);

            float MinV = Coordinates2D.GetV(Parent.Y, Height);
            float MaxV = Coordinates2D.GetV(Parent.Height, Height);

            VisibleRectUV = new Vector4(MinU, MinV, MaxU, MaxV);

            SetChildrenVisibleRectangle(Parent);
        }

        public virtual void ClearVisibleRectangle()
        {
            VisibleRectUV = Vector4.Zero;

            ClearChildrenVisibleRectangle();
        }

        protected void SetChildrenVisibleRectangle(Rectangle Area)
        {
            VisibleRectangle = Area;

            var AbsArea = Area;
            AbsArea.Position += AbsolutePosition;

            foreach (var Child in Childs)
            { 
                var AbsChildArea = new Rectangle(Child.AbsolutePosition.X, Child.AbsolutePosition.Y, Child.Width, Child.Height);
                var Bounds = Rectangle.GetChildBounds(AbsArea, AbsChildArea);

                Child.SetVisibleRectangle(Bounds);
            }
        }

        protected void ClearChildrenVisibleRectangle()
        {
            VisibleRectangle = null;
            InvisibleRect = false;

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
            if (!Visible || InvisibleRect)
                return;

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

        public override void Dispose()
        {
            foreach (var Child in Children)
            {
                Child.Dispose();
            }
            
            base.Dispose();
        }
    }
}
