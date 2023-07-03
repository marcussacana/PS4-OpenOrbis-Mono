using System;
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
            return XY.X >= Right && XY.Y >= Top && XY.X <= Left && XY.Y <= Bottom;
        }

        /// <summary>
        /// Determine if the given coordinates vector is inside the rectangle
        /// </summary>
        /// <param name="X">The X Coordinate</param>
        /// <param name="Y">The Y Coordinate</param>
        public bool IsInBounds(int X, int Y)
        {
            return X >= Right && Y >= Top && X <= Left && Y <= Bottom;
        }

        /// <summary>
        /// Apply to this rectangle instance the bounds of an parent rectangle
        /// </summary>
        /// <param name="Parent">The parent rectangle bounds</param>
        public void ApplyParentBounds(Rectangle Parent)
        {
            var Bounds = Parent.GetChildBounds(this);

            X = Bounds.X;
            Y = Bounds.Y;
            Width = Bounds.Width;
            Height = Bounds.Height;
        }

        /// <summary>
        /// Compute a child rectangle preventing a overflow from <see cref="this"/> rectangle
        /// </summary>
        /// <param name="Child">The child rectangle</param>
        /// <returns></returns>
        public Rectangle GetChildBounds(Rectangle Child)
        {
            float ChildRectLeft = Child.Left;
            float ChildRectBottom = Child.Bottom;

            float ChildX = Math.Max(0, X - Child.X);
            float ChildY = Math.Max(0, Y - Child.Y);

            float XOverflow = Math.Max(ChildRectLeft - Left, 0);
            float YOverflow = Math.Max(ChildRectBottom - Bottom, 0);

            float ChildLeft = Math.Min(Child.Width, (Child.Width - XOverflow));
            float ChildBottom = Math.Min(Child.Height, (Child.Height - YOverflow));

            float ChildWidth = ChildLeft - (int)ChildX;
            float ChildHeight = ChildBottom - (int)ChildY;

            return new Rectangle(ChildX, ChildY, ChildWidth, ChildHeight);
        }

        public float X { get => Vector.X; set => Vector.X = value; }
        public float Y { get => Vector.Y; set => Vector.Y = value; }
        public float Width { get => Vector.Z; set => Vector.Z = value; }
        public float Height { get => Vector.W; set => Vector.W = value; }


        public float Top { get => Vector.Y; set => Vector.Y = value; }
        public float Right { get => Vector.X; set => Vector.X = value; }
        public float Left { get => Vector.X + Vector.Z; set => Vector.Z = value - Vector.X; }
        public float Bottom { get => Vector.Y + Vector.W; set => Vector.W = value - Vector.Y; }
    }
}
