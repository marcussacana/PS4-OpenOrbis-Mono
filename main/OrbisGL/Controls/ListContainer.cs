using System;
using System.Numerics;

namespace OrbisGL.Controls
{
    public class ListContainer : Control
    {
        public float Margin { get; set; } = 5;
        public bool Horizontal { get; set; } = false;

        public override bool Focusable => false;

        public override string Name { get; set; }

        public override void Invalidate()
        {
            SetPositions();
            SetVisible();

            base.Invalidate();
        }

        public override void Draw(long Tick)
        {
            base.Draw(Tick);
        }

        float ScrollX;
        float ScrollY;

        void SetPositions()
        {
            float CurrentX = Math.Min(ScrollX, 0);
            float CurrentY = Math.Min(ScrollY, 0);
            foreach (var Child in Childs)
            {
                Child.Position = new Vector2(CurrentX, CurrentY);

                CurrentX += Child.Size.X + Margin;
            }
        }

        void SetVisible()
        {
            foreach (var Child in Childs)
            {
                Child.Visible = true;

                if (Child.Position.X < -Child.Size.X || Child.Position.Y < -Child.Size.Y)
                {
                    Child.Visible = false;
                    continue;
                }

                if (Child.Position.X > Size.X || Child.Position.Y > Size.Y)
                {
                    Child.Visible = false;
                    continue;
                }


                bool FullVisible = true;
                float VisibleX = 0;
                float VisibleY = 0;
                int VisibleWidth = (int)Child.Size.X;
                int VisibleHeight = (int)Child.Size.Y;

                if (Child.Position.X < 0)
                {
                    VisibleX = Child.Position.X * -1f;
                    FullVisible = false;
                }

                if (Child.Position.Y < 0)
                {
                    VisibleY = Child.Position.Y * -1f;
                    FullVisible = false;
                }

                if (Child.Position.X + Child.Size.X > Size.X)
                {
                    VisibleWidth = (int)(Size.X - (Child.Position.X + Child.Size.X));
                    FullVisible = false;
                }

                if (Child.Position.Y + Child.Size.Y > Size.Y)
                {
                    VisibleHeight = (int)(Size.Y - (Child.Position.Y + Child.Size.Y));
                    FullVisible = false;
                }

                if (FullVisible)
                {
                    Child.ClearVisibleArea();
                    continue;
                }

                Child.SetVisibleArea(VisibleX, VisibleY, VisibleWidth, VisibleHeight);
            }
        }
    }
}
