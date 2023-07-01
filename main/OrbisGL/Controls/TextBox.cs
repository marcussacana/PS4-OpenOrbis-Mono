using OrbisGL.Controls.Events;
using OrbisGL.FreeTypeLib;
using OrbisGL.GL;
using OrbisGL.GL2D;
using OrbisGL.Input;
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
        ITypewriter TypeWriter { get; set; }
        public int SelectionStart { get; set; } = 0;
        public int SelectionLength { get; set; }
        public int TextMargin { get; set; } = 10;

        public override bool Focusable => true;

        public override string Name { get => "TextBox"; }

        string _Text = null;
        public override string Text { 
            get => _Text; 
            set 
            {
                _Text = value;
                Foreground.SetRichText(value.Replace("<", "<<"));
                Invalidate();
            } 
        }

        bool Typing;
        float CurrentTextXOffset = 0;

        public TextBox(int Width, int FontSize)
        {
            var Face = Text2D.GetFont(FreeType.DefaultFace, FontSize, out bool OK);

            if (!OK)
                throw new Exception("Failed to Load the Default Font Face");

            FreeType.MeasureText("A", Face, out _, out int Height, out _);

            int TextHeight = Height;

            Height += TextMargin * 2;

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

            Caret = new Line2D(false);
            Caret.LineWidth = 0.5f;
            Caret.SetLines(new Line[] { 
                new Line()
                {
                    Begin = new Vector2(0, 0),
                    End = new Vector2(0, TextHeight)
                }
            });

            Caret.Position = new Vector2(TextMargin, TextMargin);

            GLObject.AddChild(Background);
            GLObject.AddChild(BackgroundContour);
            GLObject.AddChild(FocusIndicator);
            GLObject.AddChild(Foreground);
            GLObject.AddChild(Caret);

            OnMouseEnter += TextBox_OnMouseEnter;
            OnMouseLeave += TextBox_OnMouseLeave;

            OnKeyDown += TextBox_OnKeyDown;
            OnKeyUp += TextBox_OnKeyUp;
        }

        private void TextBox_OnKeyDown(object Sender, KeyboardEventArgs Args)
        {
            if (TypeWriter is KeyboardTypewriter Keyboard)
            {
                Keyboard.OnKeyDown(Args);
            }
        }

        private void TextBox_OnKeyUp(object Sender, KeyboardEventArgs Args)
        {
            if (TypeWriter is KeyboardTypewriter Keyboard)
            {
                Keyboard.OnKeyUp(Args);
            }
        }

        protected override void OnFocus(object Sender, EventArgs Args)
        {
            if (Application.PhysicalKeyboardAvailable && !(TypeWriter is KeyboardTypewriter))
            {
                TypeWriter = new KeyboardTypewriter();
                TypeWriter.OnTextChanged += TypeWriter_OnTextChanged;
                TypeWriter.OnCaretMove += TypeWriter_OnCaretMove;
                TypeWriter.OnComplete += TypeWriter_OnComplete;
                TypeWriter.CurrentText = Text;
                TypeWriter.CaretPosition = SelectionStart;
            }

            if (TypeWriter != null)
            {
                TypeWriter.Enter(new Rectangle(Position.X, Position.Y, Size.X, Size.Y));
                Typing = true;
            }

            base.OnFocus(Sender, Args);
        }

        private void TypeWriter_OnComplete(object sender, EventArgs e)
        {
            Typing = false;
        }

        private void TypeWriter_OnCaretMove(object sender, EventArgs e)
        {
            bool MustRefreshRichText = SelectionLength != TypeWriter.SelectionLength;

            SelectionStart = TypeWriter.CaretPosition;
            SelectionLength = TypeWriter.SelectionLength;

            if (MustRefreshRichText)
                RefreshRichText();

            Invalidate();
        }

        private void TypeWriter_OnTextChanged(object sender, EventArgs e)
        {
            RefreshRichText();
            Invalidate();
        }

        private void RefreshRichText()
        {
            var RichText = string.Empty;
            bool SelectionStarted = false;
            for (int i = 0; i <= TypeWriter.CurrentText.Length; i++)
            {
                bool InSelection = i >= SelectionStart && i < SelectionStart + SelectionLength;

                if (InSelection && !SelectionStarted && SelectionLength > 0)
                {
                    RichText += $"<backcolor={Highlight(PrimaryBackgroundColor, 200).AsHex()}><color={PrimaryForegroundColor.AsHex()}>";
                    SelectionStarted = true;
                }

                if (!InSelection && SelectionStarted)
                {
                    RichText += "</color></backcolor>";
                    SelectionStarted = false;
                }

                if (i == SelectionStart && TypeWriter.CurrentAccumulator != string.Empty)
                {
                    RichText += $"<color={Highlight(ForegroundColor, 200).AsHex()}>{TypeWriter.CurrentAccumulator.Replace("<", "<<")}</color>";
                }

                if (i >= TypeWriter.CurrentText.Length)
                    continue;

                char CurrentChar = TypeWriter.CurrentText[i];

                if (CurrentChar == '<')
                    RichText += '<';

                RichText += CurrentChar;
            }

            if (SelectionStarted)
            {
                RichText += "</color></backcolor>";
            }

            Foreground.SetRichText(RichText);
        }

        public RGBColor Highlight(RGBColor Color, byte Alpha)
        {
            if (Color.R + Color.G + Color.B / 3 > 255 / 2)
            {
                return Color.Desaturate(Alpha);
            }

            return Color.Saturate(Alpha);
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

        Line2D FocusIndicator;
        Line2D Caret;

        public void Refresh()
        {
            //Update TextBox Style
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

            //Update Text Visible Range
            SetDisplayOffset(CurrentTextXOffset);

            //Update TextBox Caret
            Caret.Color = ForegroundColor;

            Vector2 CaretPos = new Vector2(TextMargin, TextMargin);


            //Update the Caret and Display position
            int GlyphInCaret = SelectionStart;// + (TypeWriter?.CurrentAccumulator?.Length??0);
            if (GlyphInCaret >= 0 && GlyphInCaret < Foreground.GlyphsSpace.Count)
            {
                var CurrentGlyph = Foreground.GlyphsSpace[GlyphInCaret];
                var GlyphX = CurrentGlyph.Area.X;
                var GlyphMaxX = GlyphX + CurrentGlyph.Area.Width;

                if (GlyphX < CurrentTextXOffset)
                    SetDisplayOffset(GlyphX);

                if (GlyphMaxX >= CurrentTextXOffset + ForegroundWidth)
                {
                    float DisplayX = GlyphMaxX - ForegroundWidth;
                    SetDisplayOffset(DisplayX);
                }

                CaretPos.X = Foreground.Position.X + GlyphX;
            }
            else if (GlyphInCaret == Foreground.GlyphsSpace.Count && Foreground.GlyphsSpace.Any())
            {
                var LastGlyph = Foreground.GlyphsSpace.Last();
                var GlyphX = LastGlyph.Area.X;
                var GlyphMaxX = GlyphX + LastGlyph.Area.Width;

                if (GlyphX < CurrentTextXOffset)
                    SetDisplayOffset(GlyphX);

                if (GlyphMaxX >= CurrentTextXOffset + ForegroundWidth)
                {
                    float DisplayX = GlyphMaxX - ForegroundWidth;
                    SetDisplayOffset(DisplayX);
                }
                CaretPos.X = Foreground.Position.X + GlyphMaxX;
            }

            Caret.Position = CaretPos;



            Invalidated = false;
        }

        int ForegroundWidth => (int)Size.X - (TextMargin * 2);
        int ForegroundHeight => (int)Size.Y - (TextMargin * 2);

        private void SetDisplayOffset(float X)
        {
            X = Math.Min(Foreground.Width - ForegroundWidth, X);
            X = Math.Max(0, X);


            Foreground.SetVisibleRectangle(X, 0, ForegroundWidth, ForegroundHeight);
            Foreground.Position = new Vector2(-X + TextMargin, TextMargin);

            CurrentTextXOffset = X;
        }

        const int Second = Constants.SCE_SECOND;
        const int HalfSecond = Constants.SCE_SECOND / 2;
        public override void Draw(long Tick)
        {
            if (Invalidated)
                Refresh();

            Caret.Visible = Focused && (Tick % Second > HalfSecond);

            base.Draw(Tick);
        }
    }
}
