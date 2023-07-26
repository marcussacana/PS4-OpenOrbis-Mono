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
                if (PrimaryBackColor == value)
                    return;

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
                if (PrimaryForeColor == value)
                    return;

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
                if (BackColor == value)
                    return;

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
                if (ForeColor == value)
                    return;

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
                if (Visible == value)
                    return;

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
                if (_Enabled == value)
                    return;

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
                if (_Position == value)
                {
                    OnControlMoved?.Invoke(this, new EventArgs());
                    return;
                }

                _Position = value;
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
                    return Parent.AbsolutePosition + Position;
                return Position;
            }
        }

        Vector2 _Size;
        public Vector2 Size
        {
            get => _Size; 
            set 
            {
                if (value == _Size)
                {
                    OnControlResized?.Invoke(this, new EventArgs());
                    return;
                }

                GLObject.Width = (int)Size.X;
                GLObject.Height = (int)Size.Y;

                _Size = value;
                Invalidate();

                OnControlResized?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// An rectangle relative with this control position
        /// that represents the visible area of this control
        /// </summary>
        public Rectangle? VisibleRectangle { get; private set; }

        /// <summary>
        /// An rectangle relative with his screen position
        /// that represents this control rendering space
        /// </summary>
        public Rectangle AbsoluteRectangle => new Vector4(AbsolutePosition, Size.X, Size.Y);

        /// <summary>
        /// An container for 2D objects that give form for the control
        /// </summary>
        protected readonly Blank2D GLObject = new Blank2D();

        /// <summary>
        /// Get the childs of this control
        /// </summary>
        public virtual IEnumerable<Control> Childs => Children;

        /// <summary>
        /// Represents a generic container for data or information defined by the user.
        /// The contents of this object can vary based on the application's needs.
        /// </summary>
        public object Model { get; set; }

        /// <summary>
        /// Get the controls that shares the same parent of this one
        /// </summary>
        public IEnumerable<Control> Siblings => Parent?.Children;

        internal Application _Application;

        /// <summary>
        /// Returns the linked <see cref="Application"/> instance with this controller
        /// </summary>
        public Application Application => _Application ?? Parent?.Application;

        List<Control> Children = new List<Control>();

        protected bool Invalidated { get; private set; } = true;

        public event EventHandler OnControlMoved;
        public event EventHandler OnControlResized;
        public event EventHandler OnControlInvalidated;
        public event EventHandler OnControlParentChanged;
    }
}
