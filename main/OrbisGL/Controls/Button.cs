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

        bool Invalidated = true;

        string _Name = "";
        public override string Name { 
            get => _Name; 
            set 
            { 
                _Name = value;
                Invalidate();
            }
        }

        RoundedRectangle2D Background;
        RoundedRectangle2D BackgroundCountor;
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

            BackgroundCountor = new RoundedRectangle2D(Width, Height, false);
            BackgroundCountor.Color = BackgroundColor;
            BackgroundCountor.Transparency = 100;
            BackgroundCountor.RoundLevel = 1.2f;
            BackgroundCountor.ContourWidth = 0.5f;
            BackgroundCountor.Margin = new Vector2(-0.4f, -0.25f);

            GLObject.AddChild(Background);
            GLObject.AddChild(BackgroundCountor);

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

            BackgroundCountor.Width = (int)Size.X;
            BackgroundCountor.Height = (int)Size.Y;
            BackgroundCountor.Color = ForegroundColor;
            BackgroundCountor.RefreshVertex();

            Foreground.Color = Primary ? PrimaryForegroundColor : ForegroundColor;
            Foreground.Position = Size.GetMiddle(Foreground.Width, Foreground.Height);

            switch (CurrentState)
            {
                case ButtonState.Pressed:
                    Background.Color = DesaturateColor(Background.Color, 180);
                    break;
                case ButtonState.Hover:
                    Background.Color = DesaturateColor(Background.Color, 215);
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

        public override void Invalidate()
        {
            Invalidated = true;
            base.Invalidate();
        }

        private RGBColor DesaturateColor(RGBColor color, byte Alpha)
        {
            return color.Blend(RGBColor.Black, Alpha);
        }
    }
}
