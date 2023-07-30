using OrbisGL.FreeTypeLib;
using OrbisGL.GL;
using OrbisGL.GL2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace OrbisGL.Controls
{
    public class DropDownButton : Control
    {
        public override bool Focusable => true;

        public override string Name => "DropDownButton";

        public override string Text { get => base.Text; 
            set {
                bool Changed = base.Text != value;
                base.Text = value;

                if (Changed)
                    Invalidate();
            } 
        }

        const int Margin = 5;
        const int ExpandMaxHeight = 400;

        RoundedRectangle2D Background;
        RoundedRectangle2D BackgroundContour;
        Text2D Foreground;

        ControlState CurrentState;

        public DropDownButton(int Width) : this(Width, Text2D.GetFont(null, 20, out _)) { }
        public DropDownButton(int Width, FontFaceHandler Font)
        {
            FreeType.MeasureText("A", Font, out _, out int Height, out _);

            Height += Margin * 2;

            Background = new RoundedRectangle2D(Width, Height, true);
            Background.Color = BackgroundColor;
            Background.RoundLevel = 1.8f;

            BackgroundContour = new RoundedRectangle2D(Width, Height, false);
            BackgroundContour.Color = ForegroundColor;
            BackgroundContour.Opacity = 100;
            BackgroundContour.RoundLevel = 1.2f;
            BackgroundContour.ContourWidth = 1.5f;
            BackgroundContour.Margin = new Vector2(-0.8f);

            Foreground = new Text2D(Font);
            Foreground.Color = ForegroundColor;
            Foreground.Position = new Vector2(Margin, Margin);

            //[WIP] Add "v" symbol in the dropdownbutton

            GLObject.AddChild(Background);
            GLObject.AddChild(BackgroundContour);
            GLObject.AddChild(Foreground);

            Size = new Vector2(Width, Height);

            OnMouseEnter += (sender, e) => {
                if (!Enabled)
                    return;

                e.Handled = true;
                CurrentState = ControlState.Hover;
                Invalidate();
            };

            OnMouseButtonDown += (sender, e) => {
                if (!Enabled || !IsMouseHover)
                    return;

                e.Handled = true;
                CurrentState = ControlState.Pressed;
                Invalidate();
            };

            OnMouseButtonUp += (sender, e) => {
                if (!Enabled)
                    return;

                e.Handled = CurrentState == ControlState.Pressed;
                CurrentState = IsMouseHover ? ControlState.Hover : ControlState.Normal;
                Invalidate();
            };

            OnMouseLeave += (sender, e) => {
                if (!Enabled)
                    return;

                e.Handled = true;
                CurrentState = ControlState.Normal;
                Invalidate();
            };

            OnButtonDown += (sender, args) =>
            {
                if (!Focused || args.Button != OrbisPadButton.Cross)
                    return;

                args.Handled = true;
                CurrentState = ControlState.Pressed;
                Invalidate();
            };

            OnButtonUp += (sender, args) =>
            {
                if (CurrentState != ControlState.Pressed || args.Button != OrbisPadButton.Cross)
                    return;

                args.Handled = true;
                CurrentState = ControlState.Normal;
                Invalidate();
            };

            OnButtonPressed += (sender, args) =>
            {
                if (args.Button != OrbisPadButton.Cross)
                    return;

                Expand();
            };

            OnMouseClick += (sender, args) =>
            {
                if (!IsMouseHover)
                    return;

                Expand();
            };

        }

        RowView ListView;

        public IList<string> Items { get; set; } = new List<string>();

        public event EventHandler OnBeforeItemsExpand;
        public event EventHandler OnItemsExpanded;
        public event EventHandler OnItemsColapsed;
        public event EventHandler OnItemSelected;

        private void Expand()
        {
            OnBeforeItemsExpand?.Invoke(this, EventArgs.Empty);

            int ItemCount = Items.Count();

            if (ListView == null)
            {
                int Height = Math.Min(((int)Size.Y * ItemCount) - (Margin*2), ExpandMaxHeight);
                ListView = new RowView((int)Size.X + (Margin * 4), Height);

                Blank2D BG = new Blank2D();

                var BGRound = new RoundedRectangle2D((int)ListView.Size.X, (int)ListView.Size.Y + Margin, false);
                BGRound.RoundLevel = 1.2f;
                BGRound.Color = BackgroundColor;

                var BGRoundContour = new RoundedRectangle2D(BGRound.Width, BGRound.Height, false);
                BGRoundContour.Color = ForegroundColor;
                BGRoundContour.Opacity = 100;
                BGRoundContour.RoundLevel = 1.05f;
                BGRoundContour.ContourWidth = 1.5f;
                BGRoundContour.Margin = new Vector2(-0.8f);

                BG.AddChild(BGRound);
                BG.AddChild(BGRoundContour);

                ListView.SetBackground(BG);
            }

            var LastItems = ListView.Childs.Cast<Label>().ToArray();

            int i = 0;
            for (; i < LastItems.Length; i++)
            {
                if (i >= ItemCount)
                {
                    LastItems[i].Dispose();
                    continue;
                }

                LastItems[i].Text = Items[i];
            }

            for (; i < ItemCount; i++)
            {
                var Item = new Label(Items[i], Foreground.Font) { Selectable = true };

                Item.OnMouseClick += (sender, args) =>
                {
                    Text = ((Label)sender).Text;
                    OnItemSelected?.Invoke(this, EventArgs.Empty);
                    Colapse();
                };

                Item.OnButtonPressed += (sender, args) => {
                    Text = ((Label)sender).Text;
                    OnItemSelected?.Invoke(this, EventArgs.Empty);
                    Colapse();
                };

                Item.Position = new Vector2(Margin * 4, 0);

                ListView.AddChild(Item);
            }

            var PositionX = ((Size.X / 2) + AbsolutePosition.X) - (ListView.Size.X / 2);
            var PositionY = AbsoluteRectangle.Bottom;

            ListView.Position = new Vector2(PositionX, PositionY);

            var Bottom = ListView.AbsoluteRectangle.Bottom;
            var FreeHeight = Application.Height - Bottom;

            if (FreeHeight <= 0)
            {
                ListView.Position = new Vector2(ListView.Position.X, Application.Height - ExpandMaxHeight);
                Bottom = ListView.AbsoluteRectangle.Bottom;
                FreeHeight = Application.Height - Bottom;
            }

            if (Bottom > Application.Height)
            {
                ListView.Size = new Vector2(ListView.Size.X, FreeHeight);
            }

            if (!Application.Objects.Contains(ListView))
            {
                Application.AddObject(ListView);

                OnItemsExpanded?.Invoke(this, EventArgs.Empty);

                ListView.Focus();
            }
        }

        private void Colapse()
        {
            if (!Application.Objects.Contains(ListView))
                return;

            Application.RemoveObject(ListView);
            OnItemsColapsed?.Invoke(this, EventArgs.Empty);
            SetAsSelected();
        }

        public override void Refresh()
        {
            if (Size.X != Background.Width || Size.Y != Background.Height)
            {
                Background.Width = (int)Size.X;
                Background.Height = (int)Size.Y;
                BackgroundContour.Width = (int)Size.X;
                BackgroundContour.Height = (int)Size.Y;
            }

            Foreground.SetText(Text);

            var FullRect = GLObject.VisibleRectangle;
            int MaxForegroundWidth = Math.Min(Background.Width - (Margin * 2), (int?)FullRect?.Width ?? int.MaxValue);
            if (Foreground.Width > MaxForegroundWidth)
            {
                Foreground.SetVisibleRectangle(0, 0, MaxForegroundWidth, Foreground.Height);
            }
            else
            {
                if (FullRect == null)
                {
                    Foreground.ClearVisibleRectangle();
                }
                else
                {
                    var ForeRect = Foreground.Rectangle;
                    ForeRect.Position += FullRect.Value.Position;

                    var ForeVisible = Rectangle.GetChildBounds(FullRect.Value, ForeRect);
                    Foreground.SetVisibleRectangle(ForeVisible);
                }
            }
        }
    }
}
