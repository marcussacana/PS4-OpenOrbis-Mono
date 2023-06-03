using System.Numerics;

namespace OrbisGL.Controls.Events
{
    public class ClickEvent : PropagableEvent
    {
        public ClickType ClickType { get; }
        public Vector2 ClickPosition { get; }

        public ClickEvent(Vector2 Position, ClickType Type)
        {
            this.ClickType = Type;
            this.ClickPosition = Position;
        }
    }
}
