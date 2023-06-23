using OrbisGL.FreeTypeLib;
using OrbisGL.GL;
using OrbisGL.GL2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Controls
{
    public class TextBox : Control
    {
        public override bool Focusable => true;

        public override string Name { get => "TextBox"; }

        string _Text = null;
        public override string Text { 
            get => _Text; 
            set 
            {
                _Text = value;
                Invalidate();
            } 
        }

        public TextBox(int Width, int FontSize)
        {
            var Face = Text2D.GetFont(FreeType.DefaultFace, FontSize, out bool OK);

            if (!OK)
                throw new Exception("Failed to Load the Default Font Face");

            FreeType.MeasureText("A", Face, out _, out int Height, out _);

            Height += 20;

            Size = new Vector2(Width, Height);

            Background = new RoundedRectangle2D(Width, Height, true);
            Background.Color = BackgroundColor;
            Background.RoundLevel = 1.8f;

            BackgroundContour = new RoundedRectangle2D(Width, Height, false);
            BackgroundContour.Color = ForegroundColor;
            BackgroundContour.Transparency = 60;
            BackgroundContour.RoundLevel = 1.2f;
            BackgroundContour.ContourWidth = 0.75f;
            BackgroundContour.Margin = new Vector2(-0.4f, -0.25f);

            Foreground = new RichText2D(Face, FontSize, ForegroundColor);

            FocusIndicator = new Line2D(false);
            FocusIndicator.Color = ForegroundColor;
            FocusIndicator.LineWidth = 2;
            FocusIndicator.Transparency = 100;

            GLObject.AddChild(Background);
            GLObject.AddChild(BackgroundContour);
            GLObject.AddChild(FocusIndicator);
            GLObject.AddChild(Foreground);
        }


        RoundedRectangle2D Background;
        RoundedRectangle2D BackgroundContour;
        RichText2D Foreground;

        Line2D FocusIndicator; //must implement SetVisibleRectangle of Line2D


        public void Refresh()
        {
            Background.Color = BackgroundColor;
            BackgroundContour.Color = ForegroundColor;
            FocusIndicator.Color = Focused ? PrimaryBackgroundColor : ForegroundColor;

            FocusIndicator.SetLines(new Line[] { 
                new Line()
                {
                    Begin = new Vector2(10, Size.Y - 1),
                    End = new Vector2(Size.X - 10, Size.Y - 1)
                }
            });

            Foreground.Position = new Vector2(10, 10);

            Foreground.SetRichText("DEBUG TEST");
            Foreground.SetVisibleRectangle(0, 0, (int)Size.X - 20, (int)Size.Y - 20);

            Invalidated = false;
        }

        public override void Draw(long Tick)
        {
            if (Invalidated)
                Refresh();

            base.Draw(Tick);
        }

        private RGBColor DesaturateColor(RGBColor color, byte Alpha)
        {
            return color.Blend(RGBColor.Black, Alpha);
        }
    }
}
