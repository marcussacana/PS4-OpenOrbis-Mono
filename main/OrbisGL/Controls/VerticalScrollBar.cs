using OrbisGL.Controls.Events;
using OrbisGL.GL;
using OrbisGL.GL2D;
using System;
using System.Numerics;

namespace OrbisGL.Controls
{
    public class VerticalScrollBar : Control
    {
        public event EventHandler ScrollChanged;

        public override bool Focusable => false;

        public override string Name => "VerticalScrollBar";

        public int TotalHeight { get; set; }

        public float CurrentScroll { get; set; }

        private float MaxScroll
        {
            get
            {
                return TotalHeight - Size.Y;
            }
        }

        private float BarMaxY {
            get
            {
                return Size.Y - SlimBar.Height - (BarMargin * 2);
            }
        }


        RoundedRectangle2D SlimBar;
        RoundedRectangle2D FatBarForeground;
        RoundedRectangle2D FatBarBackground;

        Triangle2D UpButton;
        Triangle2D DownButton;

        int BarMargin;
        public VerticalScrollBar(int VisibleHeight, int TotalHeight, int Width)
        {
            Size = new Vector2(Width, VisibleHeight);
            this.TotalHeight = TotalHeight;

            var InnerDistance = Width * 0.6f;
            var TriangleMargin = Width * 0.2f;
            var TriangleDistance = InnerDistance + TriangleMargin;
            var InnerBarMargin = (Width - InnerDistance) / 2;

            SlimBar = new RoundedRectangle2D(Width / 3, 1, true);
            SlimBar.Color = ForegroundColor;
            SlimBar.RoundLevel = 1.8f;
            SlimBar.Opacity = 150;
            SlimBar.Position = new Vector2(InnerBarMargin, 0);

            FatBarBackground = new RoundedRectangle2D(Width, VisibleHeight - (int)(InnerDistance * 2), true);
            FatBarBackground.Color = BackgroundColor.Highlight(200);
            FatBarBackground.RoundLevel = 1.8f;
            FatBarBackground.Opacity = 200;
            FatBarBackground.Position = new Vector2(0, InnerDistance);

            FatBarForeground = new RoundedRectangle2D((int)InnerDistance, 1, true);
            FatBarForeground.Color = ForegroundColor;
            FatBarForeground.RoundLevel = 1.8f;
            FatBarForeground.Position = new Vector2(InnerBarMargin, 1);

            UpButton = new Triangle2D((int)InnerDistance, (int)InnerDistance);
            UpButton.Color = ForegroundColor;
            UpButton.RoundLevel = 1.8f;
            UpButton.Rotation = Triangle2D.Degrees.Degree180;
            UpButton.Position = new Vector2(InnerBarMargin, TriangleDistance);

            DownButton = new Triangle2D((int)InnerDistance, (int)InnerDistance);
            DownButton.Color = ForegroundColor;
            DownButton.RoundLevel = 1.8f;
            DownButton.Position = new Vector2(InnerBarMargin, VisibleHeight - (TriangleDistance * 2));

            SetFatBarVisible(false);

            BarMargin = (int)(InnerDistance * 2f + TriangleMargin) + UpButton.Height;//Button Margin + Bar Margin

            OnMouseButtonDown += ScrollBar_OnMouseButtonDown;
            OnMouseButtonUp += ScrollBar_OnMouseButtonUp;
            OnMouseMove += ScrollBar_OnMouseMove;
            OnMouseLeave += ScrollBar_OnMouseLeave;
            OnMouseEnter += ScrollBar_OnMouseEnter;

            GLObject.AddChild(SlimBar);
            GLObject.AddChild(FatBarForeground);
            GLObject.AddChild(UpButton);
            GLObject.AddChild(DownButton);
            GLObject.AddChild(FatBarBackground);
        }


        private void ScrollBar_OnMouseLeave(object Sender, MouseEventArgs EventArgs)
        {
            SetFatBarVisible(false);
            EventArgs.Handled = true;
        }
        private void ScrollBar_OnMouseEnter(object Sender, MouseEventArgs EventArgs)
        {
            SetFatBarVisible(true);
            EventArgs.Handled = true;
        }

        bool ButtonDown;
        float ButtonDownBarY;
        float ButtonDownClickY;
        private void ScrollBar_OnMouseButtonDown(object Sender, ClickEventArgs EventArgs)
        {
            if (!IsMouseHover)
                return;

            ButtonDown = true;
            ButtonDownClickY = EventArgs.Position.Y;
            ButtonDownBarY = SlimBar.Position.Y;
            EventArgs.Handled = true;
        }
        private void ScrollBar_OnMouseButtonUp(object Sender, ClickEventArgs EventArgs)
        {
            if (!ButtonDown)
                return;

            ButtonDown = false;
            EventArgs.Handled = true;
        }

        private void ScrollBar_OnMouseMove(object Sender, MouseEventArgs EventArgs)
        {
            if (!ButtonDown)
                return;

            var DeltaY = EventArgs.Position.Y - ButtonDownClickY;

            SetScrollByBarY(ButtonDownBarY + DeltaY);

            Invalidate();
            EventArgs.Handled = true;
        }

        public override void Refresh()
        {
            Visible = TotalHeight > Size.Y;

            if (!Visible)
                return;

            float VisibleProportion = Size.Y / TotalHeight;
            float BarSize = Math.Max(Size.Y * VisibleProportion, 10) - (BarMargin*2);

            SlimBar.Height = FatBarForeground.Height = (int)BarSize;
            SlimBar.RefreshVertex();
            FatBarForeground.RefreshVertex();

            SetScrollByScrollValue(CurrentScroll);

            //[WIP] Copy set visible from parent (Fix scroll bar visible in recursive scroll)
        }

        private void SetScrollByScrollValue(float Value)
        {
            Value = Math.Min(MaxScroll, Value);
            Value = Math.Max(0, Value);

            CurrentScroll = Value;

            var BarOffset = Value / MaxScroll;
            var BarY = (BarOffset * BarMaxY) + BarMargin;

            SlimBar.Position = new Vector2(SlimBar.Position.X, BarY);
            FatBarForeground.Position = new Vector2(FatBarForeground.Position.X, BarY);
        }

        private void SetScrollByBarY(float Value)
        {
            Value = Math.Min(Value, BarMaxY);
            Value = Math.Max(Value, 0);

            SlimBar.Position = new Vector2(SlimBar.Position.X, Value);
            FatBarForeground.Position = new Vector2(FatBarForeground.Position.X, Value);

            var BarOffset = Value / BarMaxY;
            var NewScroll = BarOffset * MaxScroll;

            bool ScrollChanged = NewScroll != CurrentScroll;

            CurrentScroll = NewScroll;

            if (ScrollChanged)
            {
                this.ScrollChanged?.Invoke(this, new EventArgs());
            }
        }

        private void SetFatBarVisible(bool Visible)
        {
            SlimBar.Visible = !Visible;
            FatBarBackground.Visible = Visible;
            FatBarForeground.Visible = Visible;
            UpButton.Visible = Visible;
            DownButton.Visible = Visible;
        }
    }
}
