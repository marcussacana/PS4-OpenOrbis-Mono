using OrbisGL.FreeTypeLib;
using OrbisGL.GL2D;
using System.Numerics;
using System.Runtime.Remoting.Messaging;

namespace OrbisGL.Controls
{
    public class Checkbox : Control
    {
        Text2D Label;
        RoundedRectangle2D Background;
        RoundedRectangle2D BGContour;
        Line2D CheckIcon;
        public override bool Focusable => true;

        public override string Name => "Checkbox";

        private const int TextMargin = 5;

        bool _Checked;
        public bool Checked { get => _Checked; 
            set 
            {
                _Checked = value;
                Invalidate();
            }
        }

        public Checkbox(int FontSize) : this(FontSize, Text2D.GetFont(null, FontSize, out _)) { }
        
        public Checkbox(int FontSize, FontFaceHandler Font) {
            FreeType.SetFontSize(Font, FontSize);

            FreeType.MeasureText("A", Font, out int _, out int Height, out _);

            int Size = Height + (TextMargin * 2);

            Background = new RoundedRectangle2D(Size, Size, true);
            Background.Color = BackgroundColor;
            Background.ContourWidth = 1.8f;
            Background.Margin = new Vector2(2, 2);

            BGContour = new RoundedRectangle2D(Size, Size, false);
            BGContour.Color = ForegroundColor.Highlight(160);
            BGContour.ContourWidth = 1.8f;


                int SymbolLeft = (int)(Size * 0.3);
                int SymbolRight = Size - (int)(Size * 0.3);
                int SymbolWidth = SymbolRight - SymbolLeft;
                int SymbolLeftY = (int)(Size * 0.48);
                int SymbolRightY = (int)(Size * 0.4);
                int SymbolBottom = (int)(Size * 0.435);
                int SymbolCenterX = (int)(Size * 0.267);


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


            CheckIcon.Color = BackgroundColor;
            CheckIcon.LineWidth = 2f;
            CheckIcon.Position = new Vector2((int)(Size*0.25), (int)(Size * 0.25));
            CheckIcon.Visible = false;

            Label = new Text2D(Font, FontSize);
            Label.Color = ForegroundColor;
            Label.Position = new Vector2(Size + TextMargin, TextMargin);

            this.Size = new Vector2(Size);

            GLObject.AddChild(Background);
            GLObject.AddChild(BGContour);
            GLObject.AddChild(CheckIcon);
            GLObject.AddChild(Label);

            OnMouseClick += Checkbox_OnMouseClick;
        }

        private void Checkbox_OnMouseClick(object Sender, Events.ClickEventArgs EventArgs)
        {
            Checked = !Checked;
        }

        public override void Draw(long Tick)
        {
            if (Invalidated)
                Refresh();

            base.Draw(Tick);
        }

        private void Refresh()
        {
            Label.SetText(Text);


            Size = new Vector2(Label.Position.X + Label.Width, Size.Y);

            CheckIcon.Visible = Checked;
            Background.Color = Checked ? ForegroundColor.Highlight(160) : BackgroundColor;

            Invalidated = false;
        }
    }
}
