using OrbisGL.Controls.Events;
using OrbisGL.GL;
using OrbisGL.GL2D;
using System.Numerics;

namespace OrbisGL.Controls
{
    public abstract partial class Control : IRenderable
    {
        private void FlushMouseEvents(long Tick)
        {
            if (ClickBegin > 0 && (Tick - ClickBegin) > Constants.SCE_MILISECOND * 100)
            {
                if (RightClickCount > 0)
                {
                    bool DoubleClick = RightClickCount > 1;
                    var Event = new ClickEventArgs(RightClickPos, MouseButtons.Right, DoubleClick);

                    if (LastCursorControl != null)
                    {
                        LastCursorControl.PropagateUp((x, y) => x?.OnMouseClick?.Invoke(x, (ClickEventArgs)y), Event);
                    }

                    RightClickCount = 0;
                }

                if (LeftClickCount > 0)
                {
                    bool DoubleClick = LeftClickCount > 1;
                    var Event = new ClickEventArgs(LeftClickPos, MouseButtons.Left, DoubleClick);

                    if (LastCursorControl != null)
                    {
                        LastCursorControl.PropagateUp((x, y) => x?.OnMouseClick?.Invoke(x, (ClickEventArgs)y), Event);
                    }

                    LeftClickCount = 0;
                }

                if (RightClickCount == 0 && LeftClickCount == 0)
                    ClickBegin = 0;
            }
        }

        static Control LastCursorControl = null;
        static Vector2 CurrentPosition = Vector2.Zero;
        static Elipse2D Cursor = new Elipse2D(5, 5, true) { Color = RGBColor.Black };

        internal void ProcessMouseMove(Vector2 XY)
        {
            Cursor.Position = XY;

            bool Handled = false;
            foreach (var Child in Children)
            {
                if (Child.Visible && Child.Rectangle.IsInBounds(XY))
                {
                    Child.ProcessMouseMove(XY);
                    Handled = true;
                    break;
                }
            }

            if (!Visible || !Enabled || !Rectangle.IsInBounds(XY) || Handled)
                return;

            var Coordinates = new MouseEventArgs(XY);

            if (LastCursorControl != this)
            {
                LastCursorControl?.PropagateUp((x, y) => x?.OnMouseLeave?.Invoke(x, (MouseEventArgs)y), Coordinates);
                Coordinates.Handled = false;

                LastCursorControl = this;
                LastCursorControl?.PropagateUp((x, y) => x?.OnMouseEnter?.Invoke(x, (MouseEventArgs)y), Coordinates);
                Coordinates.Handled = false;

                ClickBegin = -1;
            }

            CurrentPosition = XY;
            LastCursorControl?.PropagateUp((x, y) => x?.OnMouseMove?.Invoke(x, (MouseEventArgs)y), Coordinates);
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
                var PressedEvent = new ClickEventArgs(CurrentPosition, NewPressed, false);
                LastCursorControl.PropagateUp((x, y) => x?.OnMouseButtonDown?.Invoke(x, (ClickEventArgs)y), PressedEvent);
            }

            if (NewReleased != 0)
            {
                var ReleasedEvent = new ClickEventArgs(CurrentPosition, NewReleased, false);
                LastCursorControl.PropagateUp((x, y) => x?.OnMouseButtonUp?.Invoke(x, (ClickEventArgs)y), ReleasedEvent);
            }

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

        long ClickBegin;
        long RightClickCount;
        long LeftClickCount;
        Vector2 LeftClickPos;
        Vector2 RightClickPos;

        public event MouseEvent OnMouseMove;
        public event MouseEvent OnMouseEnter;
        public event MouseEvent OnMouseLeave;

        public event ClickEvent OnMouseButtonDown;
        public event ClickEvent OnMouseButtonUp;
        public event ClickEvent OnMouseClick;

    }
}
