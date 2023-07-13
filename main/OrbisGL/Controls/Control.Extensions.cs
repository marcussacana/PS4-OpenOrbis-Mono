using OrbisGL.Controls.Events;
using OrbisGL.GL;
using OrbisGL.GL2D;
using System;
using System.Collections.Generic;
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
        /// Get the visible child control in the given coordinates
        /// </summary>
        /// <param name="XY">The absolute coordinates to find it</param>
        /// <returns>If no control is found returns null</returns>
        public Control GetControlAt(Vector2 XY)
        {
            Control Result = null;
            var Controls = new Stack<Control>();
            Controls.Push(this);

            while (Controls.Count > 0)
            {
                var Current = Controls.Pop();

                if (Current.Visible && Current.AbsoluteRectangle.IsInBounds(XY))
                {
                    Result = Current;

                    foreach (var child in Current.Childs)
                    {
                        Controls.Push(child);
                    }
                }
            }

            return Result;
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

        /// <summary>
        /// Propagate a action to all children controls and itself
        /// </summary>
        /// <param name="Action">The action to execute in each node</param>
        /// <param name="Args">A Shared Argument to be passed</param>
        public void PropagateAll(Action<Control, PropagableEventArgs> Action, PropagableEventArgs Args)
        {
            var CtrlStack = new Stack<Control>();
            CtrlStack.Push(this);

            while (CtrlStack.Count > 0)
            {
                var Current = CtrlStack.Pop();
                Action(Current, Args);

                if (Args.Handled)
                    break;

                foreach (var Child in Current.Childs)
                {
                    CtrlStack.Push(Child);
                }
            }
        }
    }
}
