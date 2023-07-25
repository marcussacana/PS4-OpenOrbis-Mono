using OrbisGL.FreeTypeLib;
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
            OnControlParentChanged += Label_OnControlParentChanged;
            GLObject.AddChild(Text2D);
        }

        private void Label_OnControlParentChanged(object sender, System.EventArgs e)
        {
            if (Parent == null)
                return;

            Parent.OnControlInvalidated += Parent_OnControlInvalidated;
        }

        private void Parent_OnControlInvalidated(object sender, System.EventArgs e)
        {
            Invalidate();
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

                if (RichText)
                    Text2D.SetRichText(value);
                else
                    Text2D.SetRichText(value.Replace("<", "<<"));

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
                ClearVisibleArea();
                Invalidate(true);
            }

            Size = NewSize;
        }
    }
}
