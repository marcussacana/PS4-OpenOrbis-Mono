using System.Numerics;

namespace OrbisGL.Controls.Events
{

    public delegate void MouseEvent(object Sender, MouseEventArgs EventArgs);

    public class MouseEventArgs : PropagableEventArgs
    {
        /// <summary>
        /// Absolute Cursor Position when the Event Occurs
        /// </summary>
        public Vector2 Position { get; }

        public MouseEventArgs(Vector2 Cursor) {
            Position = Cursor;
        }
    }
}
