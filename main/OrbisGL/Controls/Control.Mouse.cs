using OrbisGL.GL;
using OrbisGL.GL2D;
using System.Numerics;
using OrbisGL.Controls.Events;
using System.Collections.Generic;

namespace OrbisGL.Controls
{
    public abstract partial class Control : IRenderable
    {
        private void FlushMouseEvents(long Tick)
        {
            if (ClickBegin > 0 && (Tick - ClickBegin) > Constants.SCE_MILISECOND * 500)
            {
                if (RightClickCount > 1)
                {
                    bool DoubleClick = RightClickCount > 1;
                    var Event = new ClickEventArgs(RightClickPos, MouseButtons.Right, DoubleClick);

                    if (LastCursorControl != null)
                    {
                        LastCursorControl.PropagateUp((x, y) => { x.Focus(); x?.OnMouseDoubleClick?.Invoke(x, (ClickEventArgs)y); }, Event);
                    }
                }

                if (LeftClickCount > 1)
                {
                    bool DoubleClick = LeftClickCount > 1;
                    var Event = new ClickEventArgs(LeftClickPos, MouseButtons.Left, DoubleClick);

                    if (LastCursorControl != null)
                    {
                        LastCursorControl.PropagateUp((x, y) => x?.OnMouseDoubleClick?.Invoke(x, (ClickEventArgs)y), Event);
                    }
                }

                RightClickCount = 0;
                LeftClickCount = 0;
                ClickBegin = 0;
            }
        }

        static Control LastCursorControl = null;
        static Vector2 CurrentPosition = Vector2.Zero;
        static Elipse2D Cursor = new Elipse2D(5, 5, true) { Color = RGBColor.Black };

        internal void ProcessMouseMove(Vector2 XY)
        {
            Cursor.Position = XY;

            var Coordinates = new MouseEventArgs(XY);

            var CursorControl = GetControlAt(XY);
 
            if (LastCursorControl != CursorControl)
            {
                LastCursorControl?.PropagateUp((x, y) =>
                {
                    if (x == null)
                        return;

                    x.OnMouseLeave?.Invoke(x, (MouseEventArgs)y);
                    x.IsMouseHover = false;
                }, Coordinates);

                Coordinates.Handled = false;

                LastCursorControl = CursorControl;
                LastCursorControl?.PropagateUp((x, y) =>
                {
                    if (x == null)
                        return;
                    
                    x?.OnMouseEnter?.Invoke(x, (MouseEventArgs)y);
                    x.IsMouseHover = true;
                }, Coordinates);

                Coordinates.Handled = false;

                ClickBegin = -1;
            }

            CurrentPosition = XY;
            PropagateAll((x, y) => x?.OnMouseMove?.Invoke(x, (MouseEventArgs)y), Coordinates);
        }
        internal void ProcessMouseButtons(MouseButtons PressedBefore, MouseButtons PressedNow)
        {
            if (LastCursorControl == null)
                return;

            var Changes = PressedBefore ^ PressedNow;
            var NewPressed = Changes & PressedNow;
            var NewReleased = Changes & (~PressedNow);

            if (NewPressed != 0)
            {
                LastCursorControl.Focus();

                var PressedEvent = new ClickEventArgs(CurrentPosition, NewPressed, false);
                PropagateAll((x, y) => x?.OnMouseButtonDown?.Invoke(x, (ClickEventArgs)y), PressedEvent);
            }

            if (NewReleased != 0)
            {
                var ReleasedEvent = new ClickEventArgs(CurrentPosition, NewReleased, false);
                PropagateAll((x, y) => x?.OnMouseButtonUp?.Invoke(x, (ClickEventArgs)y), ReleasedEvent);

                ReleasedEvent.Handled = false;
                LastCursorControl.PropagateUp((x, y) => x?.OnMouseClick?.Invoke(x, (ClickEventArgs)y), ReleasedEvent);

                if (NewReleased.HasFlag(MouseButtons.Left))
                {
                    if (ClickBegin <= 0)
                    {
                        ClickBegin = LastDrawTick;
                        LeftClickCount = 1;
                    }
                    else
                        LeftClickCount++;

                    LeftClickPos = CurrentPosition;
                }

                if (NewReleased.HasFlag(MouseButtons.Right))
                {
                    if (ClickBegin <= 0)
                    {
                        ClickBegin = LastDrawTick;
                        RightClickCount = 1;
                    }
                    else
                        RightClickCount++;

                    RightClickPos = CurrentPosition;
                }
            }
        }

        long ClickBegin;
        long RightClickCount;
        long LeftClickCount;
        Vector2 LeftClickPos;
        Vector2 RightClickPos;


        /// <summary>
        /// An event propagated to all controls when the mouse move
        /// </summary>
        public event MouseEvent OnMouseMove;

        /// <summary>
        /// An event propagated to a control when the mouse enter
        /// </summary>
        public event MouseEvent OnMouseEnter;

        /// <summary>
        /// An event propagated to a control when the mouse leave
        /// </summary>
        public event MouseEvent OnMouseLeave;

        /// <summary>
        /// An event propagated to all controls when a mouse button is pressed
        /// </summary>
        public event ClickEvent OnMouseButtonDown;
        /// <summary>
        /// An event propagated to all controls when a mouse button is released
        /// </summary>
        public event ClickEvent OnMouseButtonUp;

        /// <summary>
        /// An event propagated to a control when the mouse is clicked
        /// </summary>
        public event ClickEvent OnMouseClick;

        /// <summary>
        /// An late event propagated to a control when the mouse is double clicked,
        /// The click <see cref="OnMouseClick"/> is triggered before this event
        /// </summary>
        public event ClickEvent OnMouseDoubleClick;

    }
}
