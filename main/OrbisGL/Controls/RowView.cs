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
            Child.Position = CurrentPosition;
            CurrentPosition += new Vector2(0, Child.Size.Y + Margin);
            base.AddChild(Child);
        }
    }
}
