using System;
using System.Numerics;

namespace OrbisGL.Controls.Events
{
    public delegate void MoveEventHandler(object sender, MoveEventArgs Args);
    public class MoveEventArgs : PropagableEventArgs
    {
        public Vector2 CurrentOffset { get; set; }
        public MoveEventArgs(Vector2 Offset)
        {
            this.CurrentOffset = Offset;
        }
    }
}
