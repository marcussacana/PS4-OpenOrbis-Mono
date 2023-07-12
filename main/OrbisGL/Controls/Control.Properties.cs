using OrbisGL.GL;
using OrbisGL.GL2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace OrbisGL.Controls
{
    public abstract partial class Control : IRenderable
    {

        public static RGBColor DefaultPrimaryBackgroundColor = new RGBColor(0, 95, 184);
        public static RGBColor DefaultPrimaryForegroundColor = RGBColor.White;
        public static RGBColor DefaultBackgroundColor = new RGBColor(251);
        public static RGBColor DefaultForegroundColor = new RGBColor(50, 49, 48);

        RGBColor PrimaryBackColor = DefaultPrimaryBackgroundColor;
        public RGBColor PrimaryBackgroundColor
        {
            get => PrimaryBackColor;
            set
            {
                PrimaryBackColor = value;
                Invalidate();
            }
        }

        RGBColor PrimaryForeColor = DefaultPrimaryForegroundColor;
        public RGBColor PrimaryForegroundColor
        {
            get => PrimaryForeColor;
            set
            {
                PrimaryForeColor = value;
                Invalidate();
            }
        }

        RGBColor BackColor = DefaultBackgroundColor;
        public RGBColor BackgroundColor
        {
            get => BackColor;
            set
            {
                BackColor = value;
                Invalidate();
            }
        }

        RGBColor ForeColor = DefaultForegroundColor;
        public RGBColor ForegroundColor
        { 
            get => ForeColor;
            set 
            { 
                ForeColor = value;
                Invalidate();
            }
        }

        public Control RootControl
        {
            get
            {
                if (Parent == null)
                    return this;

                return Parent.RootControl;
            }
        }

        public Control FocusedControl
        {
            get
            {
                foreach (var Child in Children)
                {
                    var ChildFocus = Child.FocusedControl;

                    if (ChildFocus != null)
                        return ChildFocus;
                }

                if (_Focused)
                    return this;

                return null;
            }
        }

        public bool IsMouseHover { get; private set; }

        public long LastDrawTick { get; private set; }

        public abstract bool Focusable { get; }

        public abstract string Name { get; }
        public virtual string Text { get; set; }

        bool _Focused;
        public bool Focused { get => _Focused | Children.Any(x => x.Focused); }

        bool _RectInvisible = false;

        bool _Visible = true;
        public bool Visible
        {
            get => _Visible && (Parent == null || Parent.Visible) && !_RectInvisible; 
            set
            {
                _Visible = value;
                Invalidate();
            }
        }

        bool _Enabled = true;
        public bool Enabled
        {
            get => _Enabled && (Parent == null || Parent.Enabled); 
            set 
            {
                _Enabled = value;
                Invalidate();
            }
        }

        public Control Parent { get; private set; }

        Vector2 _Position;

        /// <summary>
        /// The point relative with the parent position
        /// </summary>
        public Vector2 Position
        {
            get => _Position;
            set 
            {
                _Position = value;
                GLObject.Position = AbsolutePosition;
                Invalidate();

                OnControlMoved?.Invoke(this, new EventArgs());
            }
        }


        /// <summary>
        /// The Point in the screen to draw the element
        /// </summary>
        public Vector2 AbsolutePosition
        {
            get
            {
                if (Parent != null)
                    return Parent.Position + Position;
                return Position;
            }
        }

        Vector2 _Size;
        public Vector2 Size
        {
            get => _Size; 
            set 
            {
                _Size = value;
                Invalidate();
            }
        }

        public Rectangle AbsoluteRectangle => new Vector4(AbsolutePosition, Size.X, Size.Y);

        protected readonly Blank2D GLObject = new Blank2D();

        public IEnumerable<Control> Childs => Children;

        List<Control> Children = new List<Control>();

        protected bool Invalidated { get; set; } = true;

        public event EventHandler OnControlMoved;
    }
}
