using System.Runtime.CompilerServices;

namespace OrbisGL.GL2D
{
    public static class Coordinates2D
    {
        internal static int Width;
        internal static int Height;

        /// <summary>
        /// The float point that represents a single pixel X distance in the rendering space.
        /// </summary>
        public static float XOffset { get; internal set; }

        /// <summary>
        /// The float point that represents a single pixel Y distance in the rendering space.
        /// </summary>
        public static float YOffset { get; internal set; }

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
    }
}