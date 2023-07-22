using System;
using System.Linq;
using System.Numerics;

namespace OrbisGL.Controls
{
    public class RowView : Panel
    {
        public override string Name => "RowView";

        Vector2 CurrentPosition = Vector2.Zero;

        public int Margin { get; set; } = 5;

        public RowView(Vector2 Size) : base(Size)
        {
            AllowScroll = true;
        }

        public RowView(int Width, int Height) : this(new Vector2(Width, Height))
        {

        }

        protected override void OnFocus(object Sender, EventArgs Args)
        {
            EnsureVisible((Control)Sender);
            base.OnFocus(Sender, Args);
        }

        private void EnsureVisible(Control Target)
        {
            var tAbsRect = Target.AbsoluteRectangle;
            var Absrect = AbsoluteRectangle;
            if (tAbsRect.Bottom >= Absrect.Bottom)
            {
                ScrollY = (int)GetChildPosition(Target).Y;
            }

            if (tAbsRect.Top <= Absrect.Top)
            {
                ScrollY = (int)(GetChildPosition(Target).Y - Size.Y + Target.Size.Y);
            }
        }

        public override void AddChild(Control Child)
        {
            Control LastChild = Childs.Any() ? Childs.Last() : null;
            
            if (LastChild != null)
                LastChild.Links.Down = Child;
            
            Child.Links.Up = LastChild;
            
            Child.Position = CurrentPosition;
            CurrentPosition += new Vector2(0, Child.Size.Y + Margin);
            
            base.AddChild(Child);
        }
    }
}
