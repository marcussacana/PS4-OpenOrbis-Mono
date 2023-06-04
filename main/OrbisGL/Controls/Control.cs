using OrbisGL.Controls.Events;
using OrbisGL.GL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace OrbisGL.Controls
{
    public abstract class Control : IRenderable
    {

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
        /// The point relative with the parent render (Ex: Panel)
        /// </summary>
        public virtual Vector2 RenderPosition
        {
            get
            {
                if (Parent != null)
                    return Parent.RenderPosition + Position;
                return Position;
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

        public IEnumerable<Control> Childs => Children.Select(x => x);

        List<Control> Children = new List<Control>();

        public void AddChild(Control Child)
        {
            Child.Parent = this;

            if (!Children.Contains(Child))
                Children.Add(Child);
        }

        public abstract void Dispose();

        public virtual void Draw(long Tick)
        {
            foreach (var Child in Children)
                Child.Draw(Tick);
        }

        public void Focus()
        {
            OnFocus(this, new EventArgs());
        }

        public virtual void Invalidate()
        {
            Parent?.Invalidate();
        }


        public virtual void OnButtonDown(object Sender, ButtonEvent Args)
        {
            if (!Focused)
                return;

            if (!_Focused)
            {
                Children.Single(x => x.Focused).OnButtonDown(Sender, Args);
                return;
            }

        }

        public virtual void OnButtonPressed(object Sender, ButtonEvent Args)
        {
            if (!Focused)
                return;

            if (!_Focused)
            {
                Children.Single(x => x.Focused).OnButtonPressed(Sender, Args);
                return;
            }

        }

        public virtual void OnButtonUp(object Sender, ButtonEvent Args)
        {
            if (!Focused)
                return;

            if (!_Focused)
            {
                Children.Single(x => x.Focused).OnButtonUp(Sender, Args);
                return;
            }
        }

        public virtual void OnClick(object Sender, ClickEvent Args)
        {
            if (!Focused)
                return;

            if (!_Focused)
            {
                Children.Single(x => x.Focused).OnClick(Sender, Args);
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
    }
}
