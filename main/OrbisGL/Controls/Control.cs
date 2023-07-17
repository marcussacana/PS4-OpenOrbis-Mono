using OrbisGL.Controls.Events;
using OrbisGL.GL;
using System;
using System.Linq;

namespace OrbisGL.Controls
{
    public abstract partial class Control : IRenderable
    {
        public virtual void Dispose()
        {
            foreach (var Child in Children.ToArray())
                Child.Dispose();

            GLObject.Dispose();

            RemoveChildren();
            Parent?.RemoveChild(this);
        }

        public virtual void Draw(long Tick)
        {
            if (!Visible)
                return;

            FlushMouseEvents(Tick);

            GLObject.Draw(Tick);

            foreach (var Child in Children)
                Child.Draw(Tick);

            if (Parent == null)
                Cursor.Draw(Tick);

            LastDrawTick = Tick;
        }

        /// <summary>
        /// Set the visible area rectangle relative with his screen position
        /// </summary>
        public void SetAbsoluteVisibleArea(Rectangle Rectangle)
        {
            SetVisibleArea(Rectangle.GetChildBounds(Rectangle, AbsoluteRectangle));
        }

        /// <summary>
        /// Set the visible area rectangle relative to the controller begin
        /// </summary>
        /// <param name="Area"></param>
        public void SetVisibleArea(Rectangle Area)
        {
            if (Area.Width == 0 || Area.Height == 0)
            {
                _RectInvisible = true;
                return;
            }

            if (!VisibleRectangle.HasValue || VisibleRectangle.Value != Area)
                Invalidate();

            VisibleRectangle = Area;
            _RectInvisible = false;

            var AbsArea = new Rectangle(Area.X + AbsolutePosition.X, Area.Y + AbsolutePosition.Y, Area.Width, Area.Height);
            foreach (var Child in Childs)
            {
                Child.SetVisibleArea(Rectangle.GetChildBounds(AbsArea, Child.AbsoluteRectangle));
            }

            GLObject.SetVisibleRectangle(Area);
        }

        public void ClearVisibleArea()
        {
            VisibleRectangle = null;
            GLObject.ClearVisibleRectangle();
        }

        public void Focus()
        {
            if (_Focused || !Focusable)
                return;

            OnFocus(this, new EventArgs());
        }

        public void Invalidate()
        {
            Invalidated = true;
            Parent?.Invalidate();
        }

        protected virtual void OnButtonDown(object Sender, ButtonEventArgs Args)
        {
            if (!Focused)
                return;

            if (!_Focused)
            {
                Children.Single(x => x.Focused).OnButtonDown(Sender, Args);
                return;
            }

        }

        protected virtual void OnButtonPressed(object Sender, ButtonEventArgs Args)
        {
            if (!Focused)
                return;

            if (!_Focused)
            {
                Children.Single(x => x.Focused).OnButtonPressed(Sender, Args);
                return;
            }

        }

        protected virtual void OnButtonUp(object Sender, ButtonEventArgs Args)
        {
            if (!Focused)
                return;

            if (!_Focused)
            {
                Children.Single(x => x.Focused).OnButtonUp(Sender, Args);
                return;
            }
        }

        protected virtual void OnFocus(object Sender, EventArgs Args)
        {
            if (Focused)
                return;

            if (!Visible || !Enabled)
                return;

            RootControl.FocusedControl?.OnLostFocus(this, Args);

            if (Focusable)
            {
                _Focused = true;
                Invalidate();
            } 
        }

        protected virtual void OnLostFocus(object Sender, EventArgs Args)
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
        public virtual void AddChild(Control Child)
        {
            if (Child.IsAncestorOf(this))
            {
                throw new InvalidOperationException("Unable to add an ancestor as child");
            }

            Child.Parent = this;

            if (!Children.Contains(Child))
                Children.Add(Child);
        }

        public virtual void RemoveChild(Control Child)
        {
            if (Child == null || !Children.Contains(Child))
                return;

            Child.Parent = null;
            Children.Remove(Child);
        }

        public virtual void RemoveChildren()
        {
            foreach (var Child in Children) { 
                Child.Parent = null;
            }

            Children.Clear();
        }
    }
}
