using OrbisGL.Controls.Events;
using OrbisGL.GL;
using System;
using System.ComponentModel;
using System.Diagnostics;
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

#if DEBUG
        public System.Diagnostics.Stopwatch Timecritical = new System.Diagnostics.Stopwatch();
#endif

        public virtual void Draw(long Tick)
        {
            if (!Visible)
                return;

            if (Invalidated)
            {
#if DEBUG
                Timecritical.Restart();
                Refresh();
                Timecritical.Stop();

                if (Timecritical.ElapsedMilliseconds > 1)
                {
                    Debugger.Log(0, "WARN", "Refresh too slow: " + GetType().Name + "\n");
                }
#else
                Refresh();
#endif

                Invalidated = false;
            }

#if DEBUG
            Timecritical.Restart();
            GLObject.Draw(Tick);
            Timecritical.Stop();

            if (Timecritical.ElapsedMilliseconds > 1)
            {
                Debugger.Log(0, "WARN", "Draw too slow: " + GetType().Name + "\n");
            }
#else
            GLObject.Draw(Tick);
#endif

            foreach (var Child in Children)
                Child.Draw(Tick);

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
            if (Area.IsEmpty())
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

        /// <summary>
        /// Invalidate this controller
        /// </summary>
        public void Invalidate() => Invalidate(false);

        /// <summary>
        /// Invalidate this controller
        /// </summary>
        /// <param name="Recursive">If true, all parent tree will be invalidated</param>
        public void Invalidate(bool Recursive)
        {
            Invalidated = true;

            if (Recursive)
                Parent?.Invalidate(Recursive);

            OnControlInvalidated?.Invoke(this, EventArgs.Empty);
        }

        public abstract void Refresh();

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
                SetAsSelected();
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
