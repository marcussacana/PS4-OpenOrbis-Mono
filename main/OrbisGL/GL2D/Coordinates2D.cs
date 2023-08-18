using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace OrbisGL.GL2D
{
    public static class Coordinates2D
    {
        internal static void SetSize(int Width, int Height)
        {
            Coordinates2D.Width = Width;
            Coordinates2D.Height = Height;


            var Offset = MeasurePixelOffset(new Vector2(Width, Height));

            XOffset = Offset.X;
            YOffset = Offset.Y;
        }

        internal static int Width { get; private set; }
        internal static int Height { get; private set; }

        /// <summary>
        /// The float point that represents a single pixel X distance in the rendering space.
        /// </summary>
        public static float XOffset { get; private set; }

        /// <summary>
        /// The float point that represents a single pixel Y distance in the rendering space.
        /// </summary>
        public static float YOffset { get; private set; }

        /// <summary>
        /// Nomarlize the Vertex X Coordinate
        /// </summary>
        /// <param name="X">The X coordinate in pixels</param>
        /// <returns>The Vertex X Coordinate</returns>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float XToPoint(float X)
        {
            return ((X / Width) * 2) - 1f;
        }

        /// <summary>
        /// Nomarlize the Vertex Y Coordinate
        /// </summary>
        /// <param name="X">The Y coordinate in pixels</param>
        /// <returns>The Vertex Y Coordinate</returns>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float YToPoint(float Y)
        {
            return -(((Y / Height) * 2) - 1f);
        }

        /// <summary>
        /// Nomarlize the Vertex XY Coordinate
        /// </summary>
        /// <param name="XY">The XY coordinate in pixels</param>
        /// <returns>The Vertex XY Coordinate</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ToPoint(this Vector2 XY)
        {
            return new Vector2(XToPoint(XY.X), YToPoint(XY.Y));
        }

        /// <summary>
        /// Nomarlize the Vertex XY Coordinate
        /// </summary>
        /// <param name="XY">The XY coordinate in pixels</param>
        /// <returns>The Vertex XY Coordinate</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ToPoint(this Vector2 XY, Vector2 MaxSize)
        {
            return new Vector2(XToPoint(XY.X, (int)MaxSize.X), YToPoint(XY.Y, (int)MaxSize.Y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float XToPoint(float X, int MaxWidth)
        {
            return ((X / MaxWidth) * 2) - 1f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float YToPoint(float Y, int MaxHeight)
        {
            return -(((Y / MaxHeight) * 2) - 1f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float PointToX(float X, int MaxWidth)
        {
            return ((X + 1f) / 2) * MaxWidth;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float PointToY(float Y, int MaxHeight)
        {
            return (((-Y) + 1f) / 2) * MaxHeight;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetU(float X, int MaxWidth)
        {
            return (X / MaxWidth);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetV(float Y, int MaxHeight)
        {
            return (Y / MaxHeight);
        }

        public static Vector2 GetMiddle(this Vector2 Size, int Width, int Height) => GetMiddle(Size, new Vector2(Width, Height));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetMiddle(this Vector2 Size, Vector2 Child)
        {
            float CenterX = Child.X / 2;
            float CenterY = Child.Y / 2;

            float ThisCenterX = Size.X / 2;
            float ThisCenterY = Size.Y / 2;

            return new Vector2(ThisCenterX - CenterX, ThisCenterY - CenterY);
        }

        public static Vector2 RotatePoint(Vector2 Point, Vector2 Center, float AngleDegrees)
        {
            double Radians = AngleDegrees * (Math.PI / 180.0);

            float cosTheta = (float)Math.Cos(Radians);
            float sinTheta = (float)Math.Sin(Radians);

            float translatedX = Point.X - Center.X;
            float translatedY = Point.Y - Center.Y;

            float rotatedX = translatedX * cosTheta - translatedY * sinTheta;
            float rotatedY = translatedX * sinTheta + translatedY * cosTheta;

            float finalX = rotatedX + Center.X;
            float finalY = rotatedY + Center.Y;

            return new Vector2(finalX, finalY);
        }

        public static Vector2 MeasurePixelOffset(Vector2 ScreenSize)
        {
            var X = XToPoint(1, (int)ScreenSize.X) + 1;
            var Y = YToPoint(1, (int)ScreenSize.Y) - 1;

            return new Vector2(X, Y);
        }
    }
}