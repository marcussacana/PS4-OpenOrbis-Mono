using System;
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
                if (_Primary == value)
                    return;

                _Primary = value;
                Invalidate();
            } 
        }

        string _Text = "";
        public override string Text { 
            get => _Text; 
            set 
            {
                if (_Text == value)
                    return;

                _Text = value;
                Invalidate();
            }
        }

        public override string Name => "Button";

        RoundedRectangle2D Background;
        RoundedRectangle2D BackgroundContour;
        Text2D Foreground;

        public event EventHandler OnClicked;

        public Button(int Width, int Height, int FontSize) : this (Width, Height)
        {
            Foreground = new Text2D(FreeType.DefaultFace, FontSize);
            GLObject.AddChild(Foreground);
        }

        public Button(int Width, int Height, FontFaceHandler Font, int FontSize) : this(Width, Height)
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
            BackgroundContour.Opacity = 100;
            BackgroundContour.RoundLevel = 1.8f;
            BackgroundContour.ContourWidth = 0.5f;
            BackgroundContour.Margin = new Vector2(-0.3f, -0.3f);

            GLObject.AddChild(Background);
            GLObject.AddChild(BackgroundContour);

            OnMouseEnter += (sender, e) => {
                if (!Enabled)
                    return;

                e.Handled = true;
                CurrentState = ButtonState.Hover;
                Invalidate();
            };

            OnMouseButtonDown += (sender, e) => {
                if (!Enabled || !IsMouseHover)
                    return;

                e.Handled = true;
                CurrentState = ButtonState.Pressed;
                Invalidate();
            };

            OnMouseButtonUp += (sender, e) => {
                if (!Enabled)
                    return;

                e.Handled = CurrentState == ButtonState.Pressed;
                CurrentState = IsMouseHover ? ButtonState.Hover : ButtonState.Normal;
                Invalidate();
            };

            OnMouseLeave += (sender, e) => {
                if (!Enabled)
                    return;

                e.Handled = true;
                CurrentState = ButtonState.Normal;
                Invalidate();
            };

            OnButtonDown += (sender, args) =>
            {
                if (!Focused || args.Button != OrbisPadButton.Cross)
                    return;

                args.Handled = true;
                CurrentState = ButtonState.Pressed;
                Invalidate();
            };

            OnButtonUp += (sender, args) =>
            {
                if (CurrentState != ButtonState.Pressed || args.Button != OrbisPadButton.Cross)
                    return;
                
                args.Handled = true;
                CurrentState = ButtonState.Normal;
                Invalidate();
            };

            OnButtonPressed += (sender, args) =>
            {
                if (args.Button != OrbisPadButton.Cross)
                    return;

                OnClicked?.Invoke(this, args);
            };

            OnMouseClick += (sender, args) =>
            {
                if (!IsMouseHover)
                    return;

                OnClicked?.Invoke(this, args);
            };
        }

        public override void Refresh()
        {
            if (Foreground == null || Background == null)
                return;

            if (!Enabled)
                CurrentState = ButtonState.Disabled;

            Foreground.SetText(Text);

            if (AutoSize && Size.X < Foreground.Width || Size.Y < Foreground.Height)
                Size = new Vector2(Foreground.Width + 10, Foreground.Height + 10);

            Background.Width = (int)Size.X;
            Background.Height = (int)Size.Y;
            Background.Color = Primary ? PrimaryBackgroundColor : BackgroundColor;

            BackgroundContour.Width = (int)Size.X;
            BackgroundContour.Height = (int)Size.Y;
            BackgroundContour.Color = ForegroundColor;

            Foreground.Color = Primary ? PrimaryForegroundColor : ForegroundColor;
            Foreground.Position = Size.GetMiddle(Foreground.Width, Foreground.Height);

            switch (CurrentState)
            {
                case ButtonState.Pressed:
                    Background.Color = Background.Color.Desaturate(180);
                    break;
                case ButtonState.Hover:
                    Background.Color = Background.Color.Desaturate(210);
                    break;
                case ButtonState.Disabled:
                    Background.Color = BackgroundColor.Grayscale();
                    Foreground.Color = ForegroundColor.Grayscale();
                    break;
            }

            GLObject.Width = (int)Size.X;
            GLObject.Height = (int)Size.Y;
            GLObject.RefreshVertex();
        }
    }
}
