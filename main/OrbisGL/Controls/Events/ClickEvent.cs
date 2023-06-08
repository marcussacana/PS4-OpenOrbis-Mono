using System.Numerics;

namespace OrbisGL.Controls.Events
{
    public class ClickEventArgs : PropagableEventArgs
    {
        public ClickType Type { get; }
        public Vector2 Position { get; }

        public ClickEventArgs(Vector2 Position, ClickType Type)
        {
            this.Type = Type;
            this.Position = Position;
        }
    }
}
