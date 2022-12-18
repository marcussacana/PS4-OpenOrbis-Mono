using System;

namespace SDL2.Types
{
    public class Rectangle
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public Size Size
        {
            get => new Size(Width, Height);
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        public Point Point
        {
            get => new Point(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Rectangle(int X, int Y, int Width, int Height)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
        }
        
        public Rectangle(Point Location, Size Size) : this(Location.X, Location.Y, Size?.Width ?? 0, Size?.Height ?? 0) {}

        private NativeStruct<SDL.SDL_Rect> NativeRect = null;
        public static implicit operator NativeStruct<SDL.SDL_Rect>(Rectangle Rect)
        {
            if (Rect.NativeRect == null)
            {
                Rect.NativeRect = new NativeStruct<SDL.SDL_Rect>(new SDL.SDL_Rect()
                {
                    x = Rect.X,
                    y = Rect.Y,
                    w = Rect.Width,
                    h = Rect.Height
                });
            }

            Rect.NativeRect.Inner.x = Rect.X;
            Rect.NativeRect.Inner.y = Rect.Y;
            Rect.NativeRect.Inner.w = Rect.Width;
            Rect.NativeRect.Inner.h = Rect.Height;
            
            return Rect.NativeRect;
        }
    }
}