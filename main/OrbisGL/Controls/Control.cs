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
        ~Control()
        {
            Dispose();
        }
        
        public virtual void Dispose()
        {
            if (Disposed)
                return;
            
            Disposed = true;
            
            foreach (var Child in Children.ToArray())
                Child.Dispose();

            GLObject.Dispose();

            _RemoveChildren(true);
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

            GLObject.Position = AbsolutePosition;

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
        public virtual void SetVisibleArea(Rectangle Area)
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

            var AbsArea = Area;
            AbsArea.Position += AbsolutePosition;
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

        public bool Focus()
        {
            if (FocusedControl == this)
                return true;

            if (TryingChildFocus)
                return false;

            if (!Focusable)
            {
                if (LastChildFocus != null && LastChildFocus.Visible)
                {
                    return LastChildFocus.Focus();
                }

                try
                {
                    TryingChildFocus = true;
                    foreach (var Child in Childs)
                    {
                        if (Child.Focus())
                            return true;
                    }
                }
                finally
                {
                    TryingChildFocus = false;
                }

                return Parent?.Focus() ?? false;
            }

            OnFocus(this, EventArgs.Empty);
            return true;
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
            var LastFocus = RootControl.FocusedControl;

            if (LastFocus == this)
                return;

            if (!Focused && Visible && Enabled)
            {
                LastFocus?.OnLostFocus(this, Args);

                if (Focusable)
                {
                    CurrentFocus = this;

                    Parent?.SetLastChildFocus(this);

                    SetAsSelected();
                    Invalidate();
                }
            }

            Parent?.OnFocus(this, Args);
        }
        private void SetLastChildFocus(Control Child)
        {
            if (Child == this || !Children.Contains(Child))
                return;
            
            LastChildFocus = Child;
            Parent?.SetLastChildFocus(this);
        }

        protected virtual void OnLostFocus(object Sender, EventArgs Args)
        {
            if (Focused)
            {
                if (CurrentFocus != this)
                {
                    Children.Single(x => x.Focused).OnLostFocus(Sender, Args);
                    return;
                }

                Invalidate();
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

            Child.OnControlParentChanged?.Invoke(Child, EventArgs.Empty);
        }

        public virtual void RemoveChild(Control Child)
        {
            if (Child == null || !Children.Contains(Child))
                return;

            Child.Parent = null;
            Children.Remove(Child);

            Child.OnControlParentChanged?.Invoke(Child, EventArgs.Empty);
        }

        public virtual void RemoveChildren(bool Dispose) => _RemoveChildren(Dispose);

        void _RemoveChildren(bool Dispose)
        {
            foreach (var Child in Children.ToArray()) {
                if (Dispose)
                {
                    Child.Dispose();
                }
                else
                {
                    Child.Parent = null;
                    Child.OnControlParentChanged?.Invoke(Child, EventArgs.Empty);
                }
            }

            Children.Clear();
        }
    }
}
