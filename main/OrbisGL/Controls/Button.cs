using OrbisGL.FreeType;
using OrbisGL.GL2D;
using System.Numerics;

namespace OrbisGL.Controls
{
    public class Button : Control
    {

        public bool AutoSize { get; set; } = true;
        public override bool Focusable => true;

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
            Foreground = new Text2D(Render.DefaultFace, FontSize);
            GLObject.AddChild(Foreground);
        }

        public unsafe Button(int Width, int Height, FT_Face* Font) : this(Width, Height)
        {
            Foreground = new Text2D(Font);
            GLObject.AddChild(Foreground);
        }

        Button(int Width, int Height) {
            Size = new Vector2(Width, Height);

            Background = new RoundedRectangle2D(Width, Height, true);
            Background.Color = BackgroundColor;
            Background.RoundLevel = 1.8f;

            BackgroundCountor = new RoundedRectangle2D(Width, Height, false);
            BackgroundCountor.Color = BackgroundColor;
            //BackgroundCountor.Transparency = 150;
            BackgroundCountor.RoundLevel = 1.2f;
            BackgroundCountor.ContourWidth = 0.5f;
            BackgroundCountor.Margin = new Vector2(-0.4f, -0.25f);

            GLObject.AddChild(Background);
            GLObject.AddChild(BackgroundCountor);
        }

        public void Refresh()
        {
            if (Foreground == null || Background == null)
                return;

            Foreground.SetText(Name);

            if (AutoSize && Size.X < Foreground.Width || Size.Y < Foreground.Height)
                Size = new Vector2(Foreground.Width + 10, Foreground.Height + 10);

            GLObject.Position = new Vector3(Position, 1);
            GLObject.Width = (int)Size.X;
            GLObject.Height = (int)Size.Y;

            Background.Width = (int)Size.X;
            Background.Height = (int)Size.Y;
            Background.Color = BackgroundColor;
            Background.RefreshVertex();

            BackgroundCountor.Width = (int)Size.X;
            BackgroundCountor.Height = (int)Size.Y;
            BackgroundCountor.Color = ForegroundColor;
            BackgroundCountor.RefreshVertex();

            Foreground.Color = ForegroundColor;
            Foreground.Position = new Vector3(Size.GetMiddle(Foreground.Width, Foreground.Height), 1);

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
    }
}
