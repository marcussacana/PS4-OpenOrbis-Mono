using System;
using System.Runtime.CompilerServices;

namespace SDL2.Types
{
    public class Point
    {
        internal Action<Point> OnChanged;

        public int X { get; private set; }
        public int Y { get; private set; }

        public Point(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(int X, int Y)
        {
            if (X == this.X && Y == this.Y)
                return;

            this.X = X;
            this.Y = Y;

            OnChanged?.Invoke(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Sum(int X, int Y)
        {
            if (X == 0 && Y == 0)
                return;

            this.X += X;
            this.Y += Y;

            OnChanged?.Invoke(this);
        }

        public static Point Zero => new Point(0, 0);
    }
}