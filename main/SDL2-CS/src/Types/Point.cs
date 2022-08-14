namespace SDL2.Types
{
    public class Point
    {
        public int X;
        public int Y;

        public Point(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static Point Zero => new Point(0, 0);
    }
}