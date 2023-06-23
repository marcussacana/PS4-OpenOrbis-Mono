using System;
using System.Numerics;

namespace OrbisGL.Controls
{
    public class ListContainer : Container
    {
        public float Margin { get; set; } = 5;
        public bool Horizontal { get; set; } = false;

        public override bool Focusable => false;

        public override string Name { get; } = "ListContainer";

        public override void Draw(long Tick)
        {
            if (Invalidated)
                SetPositions();
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
            Invalidated = false;
        }
    }
}
