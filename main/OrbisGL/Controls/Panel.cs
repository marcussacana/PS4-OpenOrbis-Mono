using OrbisGL.GL;
using OrbisGL.GL2D;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Controls
{
    public class Panel : Control
    {
        public Panel()
        {
            Background = new Rectangle2D((int)Size.X, (int)Size.Y, true);
            Background.Position = new Vector2(0, 0);

            GLObject.AddChild(Background);
        }

        public byte BackgroundTransparency { get => Background.Transparency; set => Background.Transparency = value; }

        public override bool Focusable => false;

        public override string Name { get; }

        public override string Text { get; set; }

        Rectangle2D Background;

        int _ScrollX = 0;
        int _ScrollY = 0;

        public int ScrollX { get => _ScrollX; set { _ScrollX = value; Invalidate(); } }
        public int ScrollY { get => _ScrollY; set { _ScrollY = value; Invalidate(); } }

        public int MaxScrollX { 
            get 
            {
                var MaxX = PositionMap.Max(x => x.Value.X + x.Key.Size.X) - Size.X;
                MaxX = Math.Max(MaxX, 0);

                return (int)MaxX;
            }
        }

        public int MaxScrollY
        {
            get
            {
                var MaxY = PositionMap.Max(x => x.Value.Y + x.Key.Size.Y) - Size.Y;
                MaxY = Math.Max(MaxY, 0);

                return (int)MaxY;
            }
        }

        void Refresh()
        {

            Background.Width = (int)Size.X;
            Background.Height = (int)Size.Y;
            Background.Color = BackgroundColor;

            ScrollX = Math.Max(ScrollX, 0);
            ScrollY = Math.Max(ScrollY, 0);

            ScrollX = Math.Min(ScrollX, MaxScrollX);
            ScrollY = Math.Min(ScrollY, MaxScrollY);

            try
            {
                Moving = true;
                foreach (var Child in Childs)
                {
                    var ChildPos = PositionMap[Child];
                    Child.Position = ChildPos - new Vector2(ScrollX, ScrollY);

                    Child.ClearVisibleArea();
                    Child.SetVisibleArea(Rectangle);
                }
            }
            finally 
            {
                Moving = false;
            }

            GLObject.RefreshVertex();

            Invalidated = false;
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

        public override void Draw(long Tick)
        {
            if (Invalidated)
                Refresh();

            base.Draw(Tick);
        }
    }
}
