using OrbisGL.GL;
using OrbisGL.GL2D;
using System.Numerics;
using OrbisGL.FreeTypeLib;

namespace OrbisGL.Controls
{
    public class Button : Control
    {
        public bool AutoSize { get; set; } = true;
        public override bool Focusable => true;

        bool _Primary = false;
        public bool Primary { 
            get => _Primary; 
            set 
            {
                _Primary = value;
                Invalidate();
            } 
        }

        string _Text = "";
        public override string Text { 
            get => _Text; 
            set 
            {
                _Text = value;
                Invalidate();
            }
        }

        public override string Name => "Button";

        RoundedRectangle2D Background;
        RoundedRectangle2D BackgroundContour;
        Text2D Foreground;

        public Button(int Width, int Height, int FontSize) : this (Width, Height)
        {
            Foreground = new Text2D(FreeType.DefaultFace, FontSize);
            GLObject.AddChild(Foreground);
        }

        public unsafe Button(int Width, int Height, FontFaceHandler Font, int FontSize) : this(Width, Height)
        {
            Foreground = new Text2D(Font, FontSize);
            GLObject.AddChild(Foreground);
        }

        enum ButtonState
        {
            Normal,
            Disabled,
            Hover,
            Pressed
        }

        ButtonState CurrentState = ButtonState.Normal;

        Button(int Width, int Height) {
            Size = new Vector2(Width, Height);

            Background = new RoundedRectangle2D(Width, Height, true);
            Background.Color = BackgroundColor;
            Background.RoundLevel = 1.8f;

            BackgroundContour = new RoundedRectangle2D(Width, Height, false);
            BackgroundContour.Color = BackgroundColor;
            BackgroundContour.Transparency = 100;
            BackgroundContour.RoundLevel = 1.8f;
            BackgroundContour.ContourWidth = 0.5f;
            BackgroundContour.Margin = new Vector2(-0.25f, -0.25f);

            GLObject.AddChild(Background);
            GLObject.AddChild(BackgroundContour);

            OnMouseEnter += (sender, e) => {
                if (!Enabled)
                    return;

                Hover = true;
                e.Handled = true;
                CurrentState = ButtonState.Hover;
                Invalidate();
            };

            OnMouseButtonDown += (sender, e) => {
                if (!Enabled)
                    return;

                e.Handled = true;
                CurrentState = ButtonState.Pressed;
                Invalidate();
            };

            OnMouseButtonUp += (sender, e) => {
                if (!Enabled)
                    return;

                e.Handled = true;
                CurrentState = Hover ? ButtonState.Hover : ButtonState.Normal;
                Invalidate();
            };

            OnMouseLeave += (sender, e) => {
                if (!Enabled)
                    return;

                Hover = false;
                e.Handled = true;
                CurrentState = ButtonState.Normal;
                Invalidate();
            };
        }

        bool Hover = false;

        public void Refresh()
        {
            if (Foreground == null || Background == null)
                return;

            if (!Enabled)
                CurrentState = ButtonState.Disabled;

            Foreground.SetText(Name);

            if (AutoSize && Size.X < Foreground.Width || Size.Y < Foreground.Height)
                Size = new Vector2(Foreground.Width + 10, Foreground.Height + 10);

            GLObject.Position = Position;
            GLObject.Width = (int)Size.X;
            GLObject.Height = (int)Size.Y;

            Background.Width = (int)Size.X;
            Background.Height = (int)Size.Y;
            Background.Color = Primary ? PrimaryBackgroundColor : BackgroundColor;
            Background.RefreshVertex();

            BackgroundContour.Width = (int)Size.X;
            BackgroundContour.Height = (int)Size.Y;
            BackgroundContour.Color = ForegroundColor;
            BackgroundContour.RefreshVertex();

            Foreground.Color = Primary ? PrimaryForegroundColor : ForegroundColor;
            Foreground.Position = Size.GetMiddle(Foreground.Width, Foreground.Height);

            switch (CurrentState)
            {
                case ButtonState.Pressed:
                    Background.Color = Background.Color.Desaturate(180);
                    break;
                case ButtonState.Hover:
                    Background.Color = Background.Color.Desaturate(215);
                    break;
                case ButtonState.Disabled:
                    Background.Color = BackgroundColor.Grayscale();
                    Foreground.Color = ForegroundColor.Grayscale();
                    break;
            }

            Invalidated = false;
        }

        public override void Draw(long Tick)
        {
            if (Invalidated)
                Refresh();

            base.Draw(Tick);
        }
    }
}
