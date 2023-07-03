using OrbisGL.GL;

namespace OrbisGL.Controls
{
    public class Container : Control
    {
        public override bool Focusable => false;

        public override string Name { get; }
        public override string Text { get; set; }

        public override void Draw(long Tick)
        {
            if (Invalidated)
                SetVisible();

            base.Draw(Tick);
        }

        protected void SetVisible()
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

                Child.SetVisibleArea(new Rectangle(VisibleX, VisibleY, VisibleWidth, VisibleHeight));
            }

            Invalidated = false;
        }
    }
}
