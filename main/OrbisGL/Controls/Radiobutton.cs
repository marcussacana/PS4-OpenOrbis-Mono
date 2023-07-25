using OrbisGL.FreeTypeLib;
using OrbisGL.GL2D;
using System;
using System.Numerics;
using OrbisGL.Controls.Events;
using System.Xml;

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

        //Text2D Label;
        Label lblText;
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

            lblText = new Label();
            lblText.Font = Font;
            lblText.FontSize = FontSize;
            lblText.Position = new Vector2(Size + TextMargin, TextMargin);

            this.Size = new Vector2(Size);

            GLObject.AddChild(Background);
            GLObject.AddChild(BGContour);
            GLObject.AddChild(CircleIcon);

            AddChild(lblText);

            OnMouseEnter += (s, e) => Invalidate();
            OnMouseLeave += (s, e) => Invalidate();

            OnButtonPressed += Radiobutton_OnButtonPressed;
            OnMouseClick += Radiobutton_OnMouseClick;
            OnCheckedChanged += CheckHandler;
        }

        private void Radiobutton_OnButtonPressed(object sender, ButtonEventArgs Args)
        {
            if (Args.Button != OrbisPadButton.Cross)
                return;

            Checked = true;
            Args.Handled = true;
        }


        private void Radiobutton_OnMouseClick(object Sender, ClickEventArgs Args)
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
            lblText.Text = Text;
            lblText.ForegroundColor = ForegroundColor;
            lblText.Refresh();

            CircleIcon.Color = BackgroundColor;

            Size = new Vector2(lblText.Position.X + lblText.Size.X, Size.Y);

            Background.Color = BackgroundColor;
            BGContour.Color = ForegroundColor.Highlight(160);

            Background.Color = IsMouseHover ? BackgroundColor.Highlight(220) : BackgroundColor;

            if (Checked)
                Background.Color = BackgroundColor.Highlight(160);

            CircleIcon.Visible = Checked;
        }
    }
}
