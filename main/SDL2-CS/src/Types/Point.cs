using System;
using System.Runtime.CompilerServices;

namespace SDL2.Types
{
    public class Point
    {
        /// <summary>
        /// OnChanged(XDelta, YDelta)
        /// </summary>
        internal Action<int, int> OnChanged;

        public int X { get; private set; }
        public int Y { get; private set; }

        public Point(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(int NewX, int NewY)
        {
            if (NewX == X && NewY == Y)
                return;

            var XDelta = NewX - X;
            var YDelta = NewY - Y;

            X = NewX;
            Y = NewY;

            OnChanged?.Invoke(XDelta, YDelta);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Sum(int XDiff, int YDiff)
        {
            if (XDiff == 0 && YDiff == 0)
                return;

            X += XDiff;
            Y += YDiff;

            OnChanged?.Invoke(XDiff, YDiff);
        }

        public static Point Zero => new Point(0, 0);
    }
}