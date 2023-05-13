using System.Runtime.CompilerServices;

namespace OrbisGL.GL2D
{
    public static class Coordinates2D
    {
        internal static int Width;
        internal static int Height;

        public static float XUnity { get; internal set; }
        public static float YUnity { get; internal set; }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float XToPoint(float X)
        {
            return ((X / Width) * 2) - 1f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float YToPoint(float Y)
        {
            return -(((Y / Height) * 2) - 1f);
        }
    }
}