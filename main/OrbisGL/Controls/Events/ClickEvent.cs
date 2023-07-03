using System.Numerics;

namespace OrbisGL.Controls.Events
{
    public delegate void ClickEvent(object Sender, ClickEventArgs EventArgs);
    public class ClickEventArgs : PropagableEventArgs
    {
        public MouseButtons Type { get; }

        /// <summary>
        /// Absolute Cursor Position when the Event Occurs
        /// </summary>
        public Vector2 Position { get; }

        public bool DoubleClick { get; }

        public ClickEventArgs(Vector2 Position, MouseButtons Type, bool DoubleClick)
        {
            this.Type = Type;
            this.Position = Position;
            this.DoubleClick = DoubleClick;
        }
    }
}
