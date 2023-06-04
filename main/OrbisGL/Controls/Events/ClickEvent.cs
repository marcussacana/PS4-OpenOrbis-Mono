using System.Numerics;

namespace OrbisGL.Controls.Events
{
    public class ClickEvent : PropagableEvent
    {
        public ClickType Type { get; }
        public Vector2 Position { get; }

        public ClickEvent(Vector2 Position, ClickType Type)
        {
            this.Type = Type;
            this.Position = Position;
        }
    }
}
