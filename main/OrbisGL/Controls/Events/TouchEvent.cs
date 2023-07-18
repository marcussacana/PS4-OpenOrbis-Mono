using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Controls.Events
{
    public delegate void TouchEventHandler(object Sender, TouchEventArgs Args);
    public class TouchEventArgs : PropagableEventArgs
    {
        /// <summary>
        /// The finger that triggered this event
        /// </summary>
        public Finger Finger { get; private set; }

        /// <summary>
        /// The finger position vector from -1 to 1
        /// </summary>
        public Vector2 Position { get; private set; }

        public TouchEventArgs(Vector2 Position, Finger Finger)
        {
            this.Position = Position;
            this.Finger = Finger;
        }
    }
}
