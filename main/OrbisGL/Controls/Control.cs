using OrbisGL.Controls.Events;
using OrbisGL.GL;
using OrbisGL.GL2D;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace OrbisGL.Controls
{
    public abstract class Control : IRenderable
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

        public long LastDrawTick { get; private set; }

        public abstract bool Focusable { get; }


        public abstract string Name { get; set; }

        bool _Focused;
        public bool Focused { get => _Focused | Children.Any(x => x.Focused); }

        bool _Visible = true;
        public bool Visible
        {
            get => _Visible; 
            set
            {
                _Visible = value;
                Invalidate();
            }
        }

        bool _Enabled = true;
        public bool Enabled
        {
            get => _Enabled; 
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
                Invalidate();
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

        public Vector4 Rectangle => new Vector4(AbsolutePosition, Size.X, Size.Y);

        protected readonly Blank2D GLObject = new Blank2D();

        public IEnumerable<Control> Childs => Children;

        List<Control> Children = new List<Control>();

        public void AddChild(Control Child)
        {
            Child.Parent = this;

            if (!Children.Contains(Child))
                Children.Add(Child);
        }

        public virtual void Dispose()
        {
            foreach (var Child in Children)
                Child.Dispose();
        }

        public virtual void Draw(long Tick)
        {
            if (!Visible)
                return;

            if (ClickBegin > 0 && (Tick - ClickBegin) > Constants.SCE_MILISECOND * 500)
            {
                if (RightClickCount > 0)
                {
                    bool DoubleClick = RightClickCount > 1;
                    var Event = new ClickEventArgs(RightClickPos, MouseButtons.Right, DoubleClick);

                    if (LastCursorControl != null)
                    {
                        LastCursorControl.PropagateUp((x, y) => x?.OnMouseClick?.Invoke(x, (ClickEventArgs)y), Event);
                    }

                    RightClickCount = 0;
                }

                if (LeftClickCount > 0)
                {
                    bool DoubleClick = LeftClickCount > 1;
                    var Event = new ClickEventArgs(LeftClickPos, MouseButtons.Left, DoubleClick);

                    if (LastCursorControl != null)
                    {
                        LastCursorControl.PropagateUp((x, y) => x?.OnMouseClick?.Invoke(x, (ClickEventArgs)y), Event);
                    }

                    LeftClickCount = 0;
                }

                if (RightClickCount == 0 && LeftClickCount == 0)
                    ClickBegin = 0;
            }

            GLObject.Draw(Tick);

            foreach (var Child in Children)
                Child.Draw(Tick);

            if (Parent == null)
                Cursor.Draw(Tick);

            LastDrawTick = Tick;
        }

        public void SetVisibleArea(float X, float Y, int Width, int Height)
        {
            GLObject.SetVisibleRectangle(X, Y, Width, Height);
        }

        public void ClearVisibleArea()
        {
            GLObject.ClearVisibleRectangle();
        }

        public void Focus()
        {
            OnFocus(this, new EventArgs());
        }

        public virtual void Invalidate()
        {
            Parent?.Invalidate();
        }


        public virtual void OnButtonDown(object Sender, ButtonEventArgs Args)
        {
            if (!Focused)
                return;

            if (!_Focused)
            {
                Children.Single(x => x.Focused).OnButtonDown(Sender, Args);
                return;
            }

        }

        public virtual void OnButtonPressed(object Sender, ButtonEventArgs Args)
        {
            if (!Focused)
                return;

            if (!_Focused)
            {
                Children.Single(x => x.Focused).OnButtonPressed(Sender, Args);
                return;
            }

        }

        public virtual void OnButtonUp(object Sender, ButtonEventArgs Args)
        {
            if (!Focused)
                return;

            if (!_Focused)
            {
                Children.Single(x => x.Focused).OnButtonUp(Sender, Args);
                return;
            }
        }

        #region Mouse
        static Control LastCursorControl = null;
        static Vector2 CurrentPosition = Vector2.Zero;
        static Elipse2D Cursor = new Elipse2D(5, 5, true) { Color = RGBColor.Black };

        internal void ProcessMouseMove(Vector2 XY)
        {
            Cursor.Position = XY;

            bool Handled = false;
            foreach (var Child in Children)
            {
                if (Child.Visible && Child.Rectangle.IsInBounds(XY))
                {
                    Child.ProcessMouseMove(XY);
                    Handled = true;
                    break;
                }
            }

            if (!Visible || !Rectangle.IsInBounds(XY) || Handled)
                return;

            var Coordinates = new MouseEventArgs(XY);

            if (LastCursorControl != this)
            {
                LastCursorControl?.PropagateUp((x, y) => x?.OnMouseLeave?.Invoke(x, (MouseEventArgs)y), Coordinates);
                Coordinates.Handled = false;

                LastCursorControl = this;
                LastCursorControl?.PropagateUp((x, y) => x?.OnMouseEnter?.Invoke(x, (MouseEventArgs)y), Coordinates);
                Coordinates.Handled = false;

                ClickBegin = -1;
            }

            CurrentPosition = XY;
            LastCursorControl?.PropagateUp((x, y) => x?.OnMouseMove?.Invoke(x, (MouseEventArgs)y), Coordinates);
        }
        internal void ProcessMouseButtons(MouseButtons PressedBefore, MouseButtons PressedNow)
        {
            if (LastCursorControl == null)
                return;

            var Changes = PressedBefore ^ PressedNow;
            var NewPressed = Changes & PressedNow;
            var NewReleased = Changes & (~PressedNow);

            if (NewPressed != 0)
            {
                var PressedEvent = new ClickEventArgs(CurrentPosition, NewPressed, false);
                LastCursorControl.PropagateUp((x, y) => x?.OnMouseButtonDown?.Invoke(x, (ClickEventArgs)y), PressedEvent);
            }

            if (NewReleased != 0)
            {
                var ReleasedEvent = new ClickEventArgs(CurrentPosition, NewReleased, false);
                LastCursorControl.PropagateUp((x, y) => x?.OnMouseButtonUp?.Invoke(x, (ClickEventArgs)y), ReleasedEvent);
            }

            if (NewReleased.HasFlag(MouseButtons.Left))
            {
                if (ClickBegin <= 0)
                {
                    ClickBegin = LastDrawTick;
                    LeftClickCount = 1;
                }
                else
                    LeftClickCount++;

                LeftClickPos = CurrentPosition;
            }

            if (NewReleased.HasFlag(MouseButtons.Right))
            {
                if (ClickBegin <= 0)
                {
                    ClickBegin = LastDrawTick;
                    RightClickCount = 1;
                }
                else
                    RightClickCount++;

                RightClickPos = CurrentPosition;
            }
        }

        long ClickBegin;
        long RightClickCount;
        long LeftClickCount;
        Vector2 LeftClickPos;
        Vector2 RightClickPos;

        public event MouseEvent OnMouseMove;
        public event MouseEvent OnMouseEnter;
        public event MouseEvent OnMouseLeave;

        public event ClickEvent OnMouseButtonDown;
        public event ClickEvent OnMouseButtonUp;
        public event ClickEvent OnMouseClick;

        #endregion

        public virtual void OnFocus(object Sender, EventArgs Args)
        {
            if (Focused)
                return;

            if (!Visible || !Enabled)
                return;

            RootControl.FocusedControl.OnLostFocus(this, Args);


            if (Focusable)
            {
                _Focused = true;
                Invalidate();
            } 
        }

        public virtual void OnLostFocus(object Sender, EventArgs Args)
        {
            if (Focused)
            {
                if (!_Focused)
                {
                    Children.Single(x => x.Focused).OnLostFocus(Sender, Args);
                    return;
                }
                else
                {
                    _Focused = false;
                    Invalidate();
                }
            }
        }

        public void RemoveChild(Control Child)
        {
            if (Child == null || !Children.Contains(Child))
                return;

            Child.Parent = null;
            Children.Remove(Child);
        }

        public void RemoveChildren()
        {
            foreach (var Child in Children) { 
                Child.Parent = null;
            }

            Children.Clear();
        }

        public Vector2 ToRelativeCoordinates(Vector2 AbsoluteCoordinates)
        {
            if (Parent != null)
                return AbsoluteCoordinates - Parent.AbsolutePosition;

            return AbsoluteCoordinates;
        }

        public Vector2 ToAbsoluteCoordinates(Vector2 RelativeCoordinates)
        {
            if (Parent != null)
                return Parent.AbsolutePosition + RelativeCoordinates;

            return RelativeCoordinates;
        }

        /// <summary>
        /// Check if this controller is a child of the controller defined at <paramref name="Parent"/>
        /// </summary>
        /// <param name="Parent">The Possible Parent Controller to Verify</param>
        /// <returns></returns>
        public bool IsDescendantOf(Control Parent)
        {
            Control current = this.Parent;

            while (current != null)
            {
                if (current == Parent)
                    return true;

                current = current.Parent;
            }

            return false;
        }

        /// <summary>
        /// Check if this controller is a parent of the controller defined at <paramref name="Child"/>
        /// </summary>
        /// <param name="Child">The Possible Parent Controller to Verify</param>
        /// <returns></returns>
        public bool IsAncestorOf(Control Child)
        {
            Control current = Child;

            while (current != null)
            {
                if (current == this)
                    return true;

                current = current.Parent;
            }

            return false;
        }

        /// <summary>
        /// Propagate a action to all parent controls and itself
        /// </summary>
        /// <param name="Action">The action to execute in each node</param>
        /// <param name="Args">A Shared Argument to be passed</param>
        public void PropagateUp(Action<Control, PropagableEventArgs> Action, PropagableEventArgs Args)
        {
            var Current = this;
            do
            {
                Action(Current, Args);

                if (Args != null && Args.Handled)
                    break;

                Current = Current.Parent;
            } while (Current != null);
        }
    }
}
