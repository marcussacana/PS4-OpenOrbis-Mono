using OrbisGL.FreeTypeLib;
using OrbisGL.GL;
using OrbisGL.GL2D;
using System;
using System.Numerics;
using OrbisGL.Controls.Events;

namespace OrbisGL.Controls
{
    public class Checkbox : Control
    {
        public event EventHandler OnCheckedChanged;

        Text2D Label;

        GLObject2D BGContour;
        GLObject2D Background;
        Line2D CheckIcon;
        public override bool Focusable => true;

        public override string Name => "Checkbox";

        private const int TextMargin = 5;

        bool _Checked;
        public bool Checked { get => _Checked; 
            set 
            {
                if (_Checked == value)
                    return;

                _Checked = value;
                Invalidate();

                OnCheckedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public Checkbox(int FontSize) : this(FontSize, Text2D.GetFont(null, FontSize, out _)) { }
        
        public Checkbox(int FontSize, FontFaceHandler Font) {
            Font.SetFontSize(FontSize);

            FreeType.MeasureText("A", Font, out int _, out int Height, out _);

            int Size = Height + (TextMargin * 2);

            Background = new RoundedRectangle2D(Size, Size, true)
            {
                ContourWidth = 1.8f,
                Margin = new Vector2(2, 2)
            };

            BGContour = new RoundedRectangle2D(Size, Size, false)
            {
                ContourWidth = 1.8f,
                Opacity = 160,
                Margin = new Vector2(-1)
            };

            CheckIcon = new Line2D(new GL.Line[] {
                new GL.Line()
                {
                    Begin = new Vector2((int)(Size * 0.17), (int)(Size * 0.5)),
                    End = new Vector2((int)(Size * 0.32), (int)(Size * 0.75))
                },
                new GL.Line()
                {
                    Begin = new Vector2((int)(Size * 0.32), (int)(Size * 0.75)),
                    End = new Vector2((int)(Size * 0.75), (int)(Size * 0.29))
                }
            }, false);

            CheckIcon.LineWidth = 2f;
            CheckIcon.Position = new Vector2((int)(Size*0.25), (int)(Size * 0.25));
            CheckIcon.Visible = false;

            Label = new Text2D(Font, FontSize);
            Label.Position = new Vector2(Size + TextMargin, TextMargin);

            this.Size = new Vector2(Size);

            GLObject.AddChild(Background);
            GLObject.AddChild(BGContour);
            GLObject.AddChild(CheckIcon);
            GLObject.AddChild(Label);

            OnMouseEnter += (s, e) => Invalidate();
            OnMouseLeave += (s, e) => Invalidate();

            OnMouseClick += Checkbox_OnMouseClick;
            OnButtonPressed += Checkbox_OnButtonPressed;
        }

        private void Checkbox_OnButtonPressed(object sender, ButtonEventArgs Args)
        {
            if (Args.Button != OrbisPadButton.Cross)
                return;
            
            Checked = !Checked;
        }

        private void Checkbox_OnMouseClick(object Sender, ClickEventArgs EventArgs)
        {
            Checked = !Checked;
        }

        public override void Draw(long Tick)
        {
            if (Invalidated)
                Refresh();

            base.Draw(Tick);
        }

        public override void Refresh()
        {
            Label.Color = ForegroundColor;
            CheckIcon.Color = BackgroundColor;

            Label.SetText(Text);

            Size = new Vector2(Label.Position.X + Label.Width, Size.Y);

            Background.Color = BackgroundColor;
            BGContour.Color = ForegroundColor.Highlight(160);


            CheckIcon.Visible = Checked;

            Background.Color = IsMouseHover ? BackgroundColor.Highlight(220) : BackgroundColor;

            if (Checked)
                Background.Color = BackgroundColor.Highlight(160);
        }
    }
}
