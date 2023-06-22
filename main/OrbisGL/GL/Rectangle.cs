using System.Numerics;

namespace OrbisGL.GL
{
    public struct Rectangle
    {
        public Vector4 Vector;

        public Rectangle(float X, float Y, float Width, float Height)
        {
            Vector = new Vector4(X, Y, Width, Height);
        }

        public static implicit operator Vector4(Rectangle Rectangle)
        {
            return Rectangle.Vector;
        }

        public static implicit operator Rectangle(Vector4 Vector)
        {
            var Rect = new Rectangle();
            Rect.Vector = Vector;
            return Rect;
        }

        /// <summary>
        /// Determine if the given coordinates vector is inside the rectangle
        /// </summary>
        /// <param name="XY">The XY Coordinates Vector</param>
        public bool IsInBounds(Vector2 XY)
        {
            return XY.X >= X && XY.Y >= Y && XY.X <= X + Width && XY.Y <= Y + Height;
        }

        public bool IsInBounds(int X, int Y)
        {
            return X >= this.X && Y >= this.Y && X <= this.X + Width && Y <= this.Y + Height;
        }

        public float X { get => Vector.X; set => Vector.X = value; }
        public float Y { get => Vector.Y; set => Vector.Y = value; }
        public float Width { get => Vector.Z; set => Vector.Z = value; }
        public float Height { get => Vector.W; set => Vector.W = value; }
    }
}
