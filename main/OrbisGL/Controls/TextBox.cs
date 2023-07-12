using OrbisGL.Controls.Events;
using OrbisGL.FreeTypeLib;
using OrbisGL.GL;
using OrbisGL.GL2D;
using OrbisGL.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

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

            FocusIndicator.SetLines(new Line[] {
                new Line()
                {
                    Begin = new Vector2(0, 0),
                    End = new Vector2(Size.X-10, 0)
                }
            });
            FocusIndicator.Position = new Vector2(5, Size.Y - 1);

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

            OnMouseButtonDown += MouseButtonDown;
            OnMouseButtonUp += MouseButtonUp;
            OnMouseMove += MouseMoved;
        }


        bool ButtonDown = false;
        Vector2 ClickBeginCursorPosition;
        Vector2 CurrentCursorPosition;
        int ClickBeginIndex = -1;
        int CurrentClickIndex = -1;

        private void MouseButtonDown(object Sender, ClickEventArgs EventArgs)
        {
            if (EventArgs.Type != MouseButtons.Left)
                return;

            if (!AbsoluteRectangle.IsInBounds(EventArgs.Position))
                return;

            var RelativeClick = ToRelativeCoordinates(EventArgs.Position);

            ButtonDown = true;
            CurrentCursorPosition = ClickBeginCursorPosition = RelativeClick;
            ClickBeginIndex = CurrentClickIndex = GetIndexByPosition(RelativeClick);

            EventArgs.Handled = true;
        }
        private void MouseMoved(object Sender, MouseEventArgs EventArgs)
        {
            if (!ButtonDown || TypeWriter == null)
                return;

            var RelativeCursor = ToRelativeCoordinates(EventArgs.Position);
            bool Moved = IsCursorMoved(RelativeCursor);

            if (Moved)
            {
                var CurrentIndex = GetIndexByPosition(RelativeCursor);
                if (ClickBeginIndex != CurrentIndex && CurrentIndex > -1)
                {
                    int SelectionStart = Math.Min(ClickBeginIndex, CurrentIndex);
                    int SelectionEnd = Math.Max(ClickBeginIndex, CurrentIndex);

                    int SelectionLength = SelectionEnd - SelectionStart;

                    CurrentClickIndex = CurrentIndex;

                    //Selection Length has no event therefore must be set first
                    //The selection will be refreshed in the CaretPosition Changed Event
                    TypeWriter.SelectionLength = SelectionLength;
                    TypeWriter.CaretPosition = SelectionStart;
                }
            }

            EventArgs.Handled = true;
        }

        private void MouseButtonUp(object Sender, ClickEventArgs EventArgs)
        {
            if (EventArgs.Type != MouseButtons.Left || TypeWriter == null)
                return;

            var RelativeCursor = ToRelativeCoordinates(EventArgs.Position);
            bool Moved = IsCursorMoved(RelativeCursor);

            ButtonDown = false;

            if (!Moved)
            {
                var CurrentIndex = GetIndexByPosition(RelativeCursor);

                if (CurrentIndex > -1)
                {
                    CurrentClickIndex = CurrentIndex;

                    //Selection Length has no event therefore must be set first
                    //The selection will be refreshed in the CaretPosition Changed Event
                    TypeWriter.SelectionLength = 0;
                    TypeWriter.CaretPosition = CurrentIndex;
                }
            }

            EventArgs.Handled = true;
        }

        private bool IsCursorMoved(Vector2 RelativeCursor)
        {
            var DistanceVector = RelativeCursor - CurrentCursorPosition;

            DistanceVector.X = Math.Abs(DistanceVector.X);
            DistanceVector.Y = Math.Abs(DistanceVector.Y);

            float Distance = DistanceVector.X + DistanceVector.Y;

            bool Moved = Distance > 3;
            return Moved;
        }

        public int GetIndexByPosition(Vector2 Position)
        {
            if (Foreground == null)
                return -1;

            var TextRelativePos = Position - Foreground.Position;

            //Check if the click is over an glyph
            var TargetGlyph = Foreground.GlyphsSpace
                .Select((x,i) => new KeyValuePair<int, GlyphInfo>(i, x))
                .Where(x => x.Value.Area.IsInBounds(TextRelativePos));

            //If not get the nearest glyph of the given click offset
            if (!TargetGlyph.Any())
                TargetGlyph = Foreground.GlyphsSpace
                .Select((x, i) => new KeyValuePair<int, GlyphInfo>(i, x))
                .OrderBy(x => Math.Abs(GetGlyphCenterX(x.Value) - TextRelativePos.X));

            if (!TargetGlyph.Any())
                return -1;

            //Check if the click is near of the left or right side of the glyph
            var Glyph = TargetGlyph.First();
            var HalfX = GetGlyphCenterX(Glyph.Value);
            if (Position.X <= HalfX)
            {
                return Glyph.Key;
            }

            return Glyph.Key + 1;
        }
        float GetGlyphCenterX(GlyphInfo Glyph)
        {
            return (Glyph.Area.X + (Glyph.Area.Width / 2)) + TextMargin - CurrentTextXOffset;
        }

        private void TextBox_OnKeyDown(object Sender, KeyboardEventArgs Args)
        {
            ButtonDown = false;
            if (TypeWriter is KeyboardTypewriter Keyboard)
            {
                Keyboard.OnKeyDown(Args);
            }
        }

        private void TextBox_OnKeyUp(object Sender, KeyboardEventArgs Args)
        {
            ButtonDown = false;
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
                //Apply the style if the current text is inside the selection range
                bool InSelection = i >= SelectionStart && i < SelectionStart + SelectionLength;

                if (InSelection && !SelectionStarted && SelectionLength > 0)
                {
                    RichText += $"<backcolor={PrimaryBackgroundColor.Highlight(200).AsHex()}><color={PrimaryForegroundColor.AsHex()}>";
                    SelectionStarted = true;
                }

                if (!InSelection && SelectionStarted)
                {
                    RichText += "</color></backcolor>";
                    SelectionStarted = false;
                }

                //The current text is in accumulator
                if (i == SelectionStart && TypeWriter.CurrentAccumulator != string.Empty)
                {
                    RichText += $"<color={ForegroundColor.Highlight(200).AsHex()}>{TypeWriter.CurrentAccumulator.Replace("<", "<<")}</color>";
                }


                if (i >= TypeWriter.CurrentText.Length)
                    continue;

                //Get the next character
                char CurrentChar = TypeWriter.CurrentText[i];

                //Escape the '<' character
                if (CurrentChar == '<')
                    RichText += '<';

                //Append the character to the rich text
                RichText += CurrentChar;
            }


            //Close selection style if not closed
            if (SelectionStarted)
            {
                RichText += "</color></backcolor>";
            }

            Foreground.SetRichText(RichText);
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

            FocusIndicator.Transparency = Focused ? (byte)255 : (byte)160;
            FocusIndicator.Color = Focused ? PrimaryBackgroundColor : ForegroundColor;

            //Update Text Visible Range
            SetDisplayOffset(CurrentTextXOffset);

            //Update TextBox Caret
            Caret.ClearVisibleRectangle();
            Caret.Color = ForegroundColor;

            Vector2 CaretPos = new Vector2(TextMargin, TextMargin);

            //Update the Caret and Display position
            CaretPos = RefreshCaretAndEnsureVisible(CaretPos);
            Caret.Position = CaretPos;

            Invalidated = false;
        }

        private Vector2 RefreshCaretAndEnsureVisible(Vector2 CaretPos)
        {
            var Glyphs = Foreground.GlyphsSpace;
            int GlyphInCaret = ButtonDown ? CurrentClickIndex : SelectionStart;
            bool CaretOnEnd = false;

            GlyphInfo CurrentGlyph;

            if (GlyphInCaret >= 0 && GlyphInCaret < Glyphs.Count) //Caret in the begin or middle of the text
            {
                CurrentGlyph = Glyphs[GlyphInCaret];
            }
            else if (GlyphInCaret == Glyphs.Count && Glyphs.Any()) //Caret in the end of the text
            {
                CurrentGlyph = Glyphs.Last();
                CaretOnEnd = true;
            }
            else //Caret out of bound
            {
                return CaretPos;
            }

            var GlyphX = CurrentGlyph.Area.X;
            var GlyphMaxX = GlyphX + CurrentGlyph.Area.Width;

            EnsureGlyphVisible(GlyphX, GlyphMaxX);

            //Update Caret Position
            CaretPos.X = Foreground.Position.X + (CaretOnEnd ? GlyphMaxX : GlyphX);

            var BGRect = Background.VisibleRectangle ?? Background.Rectangle;
            var CaretRect = new Rectangle(TextMargin, TextMargin, 1, Caret.Height);

            var BoundsRect = Rectangle.GetChildBounds(BGRect, CaretRect);

            Caret.SetVisibleRectangle(BoundsRect);

            return CaretPos;
        }

        private void EnsureGlyphVisible(float GlyphX, float GlyphMaxX)
        {
            if (GlyphX < CurrentTextXOffset)
                SetDisplayOffset(GlyphX);

            if (GlyphMaxX >= CurrentTextXOffset + ForegroundWidth)
            {
                float DisplayX = GlyphMaxX - ForegroundWidth;
                SetDisplayOffset(DisplayX);
            }
        }

        int ForegroundWidth => (int)Size.X - (TextMargin * 2);
        int ForegroundHeight => (int)Size.Y - (TextMargin * 2);

        private void SetDisplayOffset(float X)
        {
            X = Math.Min(Foreground.Width - ForegroundWidth, X);
            X = Math.Max(0, X);

            //If the control is half visible, the foreground visible area must
            //keep the visible range already set, we do that here by copying
            //the textbox background visible rectangle and computing the text bounds
            var BGRect = Background.VisibleRectangle ?? Background.Rectangle;
            var FGRect = new Rectangle(X, TextMargin, ForegroundWidth, ForegroundHeight);
            BGRect.X += X;

            var BoundsRect = Rectangle.GetChildBounds(BGRect, FGRect);

            BoundsRect.X += X;

            Foreground.SetVisibleRectangle(BoundsRect);
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
