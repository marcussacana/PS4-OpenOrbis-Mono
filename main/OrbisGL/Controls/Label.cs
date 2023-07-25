using OrbisGL.FreeTypeLib;
using OrbisGL.GL;
using OrbisGL.GL2D;
using System.Numerics;

namespace OrbisGL.Controls
{
    internal class Label : Control
    {
        RichText2D Text2D;

        public Label()
        {
            BackgroundColor = null;
            Text2D = new RichText2D(22, ForegroundColor, null);
            GLObject.AddChild(Text2D);
        }

        public Label(string Text) : this()
        {
            Text2D.SetRichText(Text.Replace("<", "<<"));
        }

        public bool RichText { get; set; } = false;
        public bool Selectable { get; set; }
        public override bool Focusable { get => Selectable; }

        public override string Name => "Label";

        public override string Text { 
            get => Text2D.Text;
            set {
                if (Text2D.Text == value)
                    return;

                var NewText = value;

                if (!RichText)
                    NewText = NewText.Replace("<", "<<");

                Text2D.SetRichText(NewText);
                Invalidate();
            }
        }

        public FontFaceHandler Font {
            get => Text2D.Font;
            set {
                Text2D.Font = value;
                Invalidate();
            }
        }
        public int FontSize { 
            get => Text2D.FontSize;
            set 
            {
                Text2D.FontSize = value;
                Invalidate();
            }
        }

        public override void Refresh()
        {
            Text2D.FontColor = ForegroundColor;
            Text2D.BackColor = BackgroundColor;

            var NewSize = new Vector2(Text2D.Width, Text2D.Height);

            var Resized = Size != NewSize;

            if (Resized)
            {
                Parent?.Invalidate();
                Size = NewSize;
            }
        }

        public override void SetVisibleArea(Rectangle Area)
        {
            if (Size == Vector2.Zero)
                return;

            base.SetVisibleArea(Area);
        }
    }
}
