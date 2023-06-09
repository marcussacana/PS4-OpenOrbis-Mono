using System.Numerics;

namespace OrbisGL.Controls.Events
{

    public delegate void MouseEvent(object Sender, MouseEventArgs EventArgs);

    public class MouseEventArgs : PropagableEventArgs
    {
        public Vector2 CursorPosition { get; }
        public MouseEventArgs(Vector2 Cursor) {
            CursorPosition = Cursor;
        }
    }
}
