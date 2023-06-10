using OrbisGL.Controls.Events;
using OrbisGL.GL;
using System;
using System.Linq;

namespace OrbisGL.Controls
{
    public abstract partial class Control : IRenderable
    {
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

            FlushMouseEvents(Tick);

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
    }
}
