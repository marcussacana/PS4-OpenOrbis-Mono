using OrbisGL.GL;
using OrbisGL.GL2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace OrbisGL.Controls
{
    public class Panel : Control
    {
        VerticalScrollBar ScrollBar;

        public Panel(int Width, int Height) : this(new Vector2(Width, Height)) { }
        public Panel(Vector2 Size)
        {
            this.Size = Size;

            Background = new Rectangle2D((int)Size.X, (int)Size.Y, true);
            Background.Position = new Vector2(0, 0);

            OnControlResized += (s, e) =>
            {
                Background.Width = (int)this.Size.X;
                Background.Height = (int)this.Size.Y;
            };

            GLObject.AddChild(Background);
        }

        public bool AllowScroll { get; set; }
        public byte ScrollBarWidth { get; set; } = 15;

        public byte BackgroundTransparency { get => Background.Opacity; set => Background.Opacity = value; }

        public override bool Focusable => false;

        public override string Name { get; }

        public override string Text { get; set; }

        Rectangle2D Background;

        int _ScrollX = 0;
        int _ScrollY = 0;

        public int ScrollX { get => _ScrollX; set { if (value == _ScrollX) return;  _ScrollX = value; Invalidate(); } }
        public int ScrollY { get => _ScrollY; set { if (value == _ScrollY) return;  _ScrollY = value; Invalidate(); } }

        public override IEnumerable<Control> Childs => base.Childs.Where(x => x != ScrollBar);

        public int MaxScrollX { 
            get
            {
                if (!PositionMap.Any())
                    return 0;

                var MaxX = PositionMap
                    .Where(x => x.Key != ScrollBar)
                    .Max(x => x.Value.X + x.Key.Size.X) - Size.X;
                
                MaxX = Math.Max(MaxX, 0);

                return (int)MaxX;
            }
        }

        public int MaxScrollY
        {
            get
            {
                if (!PositionMap.Any())
                    return 0;

                var MaxY = PositionMap
                    .Where(x => x.Key != ScrollBar)
                    .Max(x => x.Value.Y + x.Key.Size.Y) - Size.Y;
                MaxY = Math.Max(MaxY, 0);

                return (int)MaxY;
            }
        }

        private Rectangle _CurrentVisibleArea;
        public Rectangle CurrentVisibleArea { get => _CurrentVisibleArea; private set => _CurrentVisibleArea = value; }

        public override void Refresh()
        {
            if (ScrollBar == null && AllowScroll)
            {
                if (Childs.Any(x => x is VerticalScrollBar))
                {
                    foreach (var OldScrollBar in Childs.Where(x => x is VerticalScrollBar).ToArray())
                    {
                        OldScrollBar.Dispose();
                    }
                }

                ScrollBar = new VerticalScrollBar((int)Size.Y, MaxScrollY + (int)Size.Y, ScrollBarWidth);
                ScrollBar.CurrentScroll = ScrollY;
                ScrollBar.Refresh();
                ScrollBar.ScrollChanged += (s, e) => {
                    ScrollY = (int)((VerticalScrollBar)s).CurrentScroll;
                };

                AddChild(ScrollBar);
            }

            Background.Width = (int)Size.X;
            Background.Height = (int)Size.Y;
            Background.Color = BackgroundColor;

            ScrollX = Math.Max(ScrollX, 0);
            ScrollY = Math.Max(ScrollY, 0);

            ScrollX = Math.Min(ScrollX, MaxScrollX);
            ScrollY = Math.Min(ScrollY, MaxScrollY);

            if (ScrollBar != null)
                ScrollBar.CurrentScroll = ScrollY;

            var AreaRect = AbsoluteRectangle;

            if (VisibleRectangle.HasValue)
            {
                AreaRect = VisibleRectangle.Value;
                AreaRect.Position += AbsolutePosition;

                if (Parent != null && Parent.VisibleRectangle.HasValue)
                {
                    var ParentVisibleArea = Parent.VisibleRectangle.Value;
                    ParentVisibleArea.Position += Parent.AbsolutePosition;

                    AreaRect = AreaRect.Intersect(ParentVisibleArea);
                }
            }

            _CurrentVisibleArea = AreaRect;
            _CurrentVisibleArea.Position += new Vector2(ScrollX, ScrollY);

            try
            {
                Moving = true;
                foreach (var Child in Childs)
                {
                    var ChildPos = PositionMap[Child];
                    Child.Position = ChildPos - new Vector2(ScrollX, ScrollY);

                    Child.SetAbsoluteVisibleArea(AreaRect);
                }

                if (ScrollBar != null)
                {
                    ScrollBar.Position = new Vector2(Size.X - ScrollBarWidth - (int)(ScrollBarWidth * 0.3), 0);
                    ScrollBar.SetAbsoluteVisibleArea(AreaRect);
                }
            }
            finally 
            {
                Moving = false;
            }

            Background.ClearVisibleRectangle();

            var BGVisibleRect = Rectangle.GetChildBounds(AreaRect, AbsoluteRectangle);
            Background.Position = BGVisibleRect.Position;
            Background.Width = (int)BGVisibleRect.Width;
            Background.Height = (int)BGVisibleRect.Height;

            GLObject.RefreshVertex();
        }

        Dictionary<Control, Vector2> PositionMap = new Dictionary<Control, Vector2>();
        public override void AddChild(Control Child)
        {
            PositionMap[Child] = Child.Position;
            Child.OnControlMoved += Child_OnControlMoved;
            base.AddChild(Child);
        }

        bool Moving;
        private void Child_OnControlMoved(object sender, EventArgs e)
        {
            if (sender == null || Moving)
                return;                          

            var Child = (Control)sender;
            PositionMap[Child] = Child.Position;
        }

        public override void RemoveChild(Control Child)
        {
            if (!PositionMap.ContainsKey(Child))
                return;

            Child.OnControlMoved -= Child_OnControlMoved;
            PositionMap.Remove(Child);
            base.RemoveChild(Child);
        }

        public override void RemoveChildren()
        {
            foreach (var Child in Childs)
            {
                Child.OnControlMoved -= Child_OnControlMoved;
            }

            PositionMap.Clear();
            base.RemoveChildren();
        }

        public Vector2 GetChildPosition(Control Child)
        {
            if (PositionMap.TryGetValue(Child, out Vector2 Result))
                return Result;
            
            return Vector2.Zero;
        }

    }
}
