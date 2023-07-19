using OrbisGL.FreeTypeLib;
using OrbisGL.GL2D;
using System;
using System.Numerics;

namespace OrbisGL.Controls
{
    public class Radiobutton : Control
    {
        public event EventHandler OnCheckedChanged;

        bool _Checked;
        public bool Checked { get => _Checked; 
            set { 
                if (_Checked == value) 
                    return; 
                _Checked = value;
                Invalidate();

                OnCheckedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public override bool Focusable => true;

        public override string Name => "Radiobutton";

        Text2D Label;
        Elipse2D CircleIcon;
        GLObject2D BGContour;
        GLObject2D Background;
        private const int TextMargin = 5;

        public Radiobutton(int FontSize) : this(FontSize, Text2D.GetFont(null, FontSize, out _)) { }

        public Radiobutton(int FontSize, FontFaceHandler Font)
        {
            FreeType.SetFontSize(Font, FontSize);

            FreeType.MeasureText("A", Font, out int _, out int Height, out _);

            int Size = Height + (TextMargin * 2);

            Background = new Elipse2D(Size, Size, true)
            {
                Color = BackgroundColor,
                AntiAliasing = 2
            };

            BGContour = new Elipse2D(Size, Size, false)
            {
                Color = ForegroundColor,
                ContourWidth = 1f,
                AntiAliasing = 2
            };

            CircleIcon = new Elipse2D((int)(Background.Width * 0.55), (int)(Background.Height * 0.55), true);
            CircleIcon.Color = BackgroundColor;
            CircleIcon.AntiAliasing = 2;
            CircleIcon.Position = (Background.Rectangle.Size / 2) - (CircleIcon.Rectangle.Size / 2);
            CircleIcon.Visible = false;

            Label = new Text2D(Font, FontSize);
            Label.Position = new Vector2(Size + TextMargin, TextMargin);

            this.Size = new Vector2(Size);



            GLObject.AddChild(Background);
            GLObject.AddChild(BGContour);
            GLObject.AddChild(CircleIcon);
            GLObject.AddChild(Label);


            OnMouseEnter += (s, e) => Invalidate();
            OnMouseLeave += (s, e) => Invalidate();

            OnMouseClick += Radiobutton_OnMouseClick;
            OnCheckedChanged += CheckHandler;
        }


        private void Radiobutton_OnMouseClick(object Sender, Events.ClickEventArgs EventArgs)
        {
            Checked = true;
        }

        private void CheckHandler(object sender, EventArgs e)
        {
            if (!Checked || Parent == null)
                return;

            foreach (var Sibling in Siblings)
            {
                if (Sibling is Radiobutton Button && Sibling != this) {
                    Button.Checked = false;
                }
            }
        }

        public override void Refresh()
        {
            Label.Color = ForegroundColor;
            CircleIcon.Color = BackgroundColor;

            Label.SetText(Text);

            Size = new Vector2(Label.Position.X + Label.Width, Size.Y);

            Background.Color = BackgroundColor;
            BGContour.Color = ForegroundColor.Highlight(160);

            Background.Color = IsMouseHover ? BackgroundColor.Highlight(220) : BackgroundColor;

            if (Checked)
                Background.Color = BackgroundColor.Highlight(160);

            CircleIcon.Visible = Checked;
        }

        public override void Draw(long Tick)
        {
            if (Invalidated)
                Refresh();

            base.Draw(Tick);
        }
    }
}
