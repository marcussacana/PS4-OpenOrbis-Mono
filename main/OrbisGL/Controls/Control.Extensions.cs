using OrbisGL.Controls.Events;
using OrbisGL.GL;
using OrbisGL.GL2D;
using System;
using System.Linq;
using System.Numerics;

namespace OrbisGL.Controls
{
    public abstract partial class Control : IRenderable
    {
        public Vector2 ToRelativeCoordinates(Vector2 AbsoluteCoordinates)
        {
            if (Parent != null)
                return AbsoluteCoordinates - AbsolutePosition;

            return AbsoluteCoordinates;
        }

        public Vector2 ToAbsoluteCoordinates(Vector2 RelativeCoordinates)
        {
            if (Parent != null)
                return AbsolutePosition + RelativeCoordinates;

            return RelativeCoordinates;
        }

        /// <summary>
        /// Check if this controller is a child of the controller defined at <paramref name="Parent"/>
        /// </summary>
        /// <param name="Parent">The Possible Parent Controller to Verify</param>
        /// <returns></returns>
        public bool IsDescendantOf(Control Parent)
        {
            Control current = this.Parent;

            while (current != null)
            {
                if (current == Parent)
                    return true;

                current = current.Parent;
            }

            return false;
        }

        /// <summary>
        /// Check if this controller is a parent of the controller defined at <paramref name="Child"/>
        /// </summary>
        /// <param name="Child">The Possible Parent Controller to Verify</param>
        /// <returns></returns>
        public bool IsAncestorOf(Control Child)
        {
            Control current = Child;

            while (current != null)
            {
                if (current == this)
                    return true;

                current = current.Parent;
            }

            return false;
        }

        /// <summary>
        /// Propagate a action to all parent controls and itself
        /// </summary>
        /// <param name="Action">The action to execute in each node</param>
        /// <param name="Args">A Shared Argument to be passed</param>
        public void PropagateUp(Action<Control, PropagableEventArgs> Action, PropagableEventArgs Args)
        {
            var Current = this;
            do
            {
                Action(Current, Args);

                if (Args != null && Args.Handled)
                    break;

                Current = Current.Parent;
            } while (Current != null);
        }
    }
}
