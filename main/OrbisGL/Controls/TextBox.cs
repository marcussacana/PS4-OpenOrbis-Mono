using OrbisGL.Controls.Events;
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
            BackgroundContour.ContourWidth = 0.8f;
            BackgroundContour.Margin = new Vector2(-0.2f, -0.2f);

            Foreground = new RichText2D(Face, FontSize, ForegroundColor);

            FocusIndicator = new Line2D(false);
            FocusIndicator.Color = ForegroundColor;
            FocusIndicator.LineWidth = 0.2f;
            FocusIndicator.Transparency = 150;

            GLObject.AddChild(Background);
            GLObject.AddChild(BackgroundContour);
            GLObject.AddChild(FocusIndicator);
            GLObject.AddChild(Foreground);

            OnMouseEnter += TextBox_OnMouseEnter;
            OnMouseLeave += TextBox_OnMouseLeave;
        }

        bool Desaturate = false;
        private void TextBox_OnMouseEnter(object Sender, MouseEventArgs EventArgs)
        {
            if (Focused)
                return;

            Desaturate = true;
            Invalidate();
        }

        private void TextBox_OnMouseLeave(object Sender, MouseEventArgs EventArgs)
        {
            Desaturate = false;
            Invalidate();
        }

        RoundedRectangle2D Background;
        RoundedRectangle2D BackgroundContour;
        RichText2D Foreground;

        Line2D FocusIndicator; //[WIP] must implement SetVisibleRectangle of Line2D


        public void Refresh()
        {
            Background.Color = Desaturate && !Focused ? BackgroundColor.Desaturate(240) : BackgroundColor;
            BackgroundContour.Color = ForegroundColor;

            FocusIndicator.Transparency = Focused ? (byte)255 : (byte)60;
            FocusIndicator.Color = Focused ? PrimaryBackgroundColor : ForegroundColor;

            FocusIndicator.SetLines(new Line[] { 
                new Line()
                {
                    Begin = new Vector2(5, Size.Y - 1),
                    End = new Vector2(Size.X - 5, Size.Y - 1)
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
    }
}
