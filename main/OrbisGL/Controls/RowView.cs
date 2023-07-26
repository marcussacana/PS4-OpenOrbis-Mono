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

        public override void AddChild(Control Child)
        {
            var Positions = PositionMap.Where(x => Childs.Contains(x.Key));
            if (!(Child is VerticalScrollBar))
            {
                Control LastChild = Childs.Any() ? Childs.Last() : null;

                if (LastChild != null)
                    LastChild.Links.Down = Child;

                Child.Links.Up = LastChild;

                var CurrentBottom = Positions.Any() ? Positions.Max(x => x.Value.Y + x.Key.Size.Y) : 0;

                Child.Position = new Vector2(0, CurrentBottom + Margin);

                Child.Refresh();
            }
            
            base.AddChild(Child);
        }
    }
}
