using OrbisGL.GL;
using System;
using System.Linq;
using System.Numerics;

namespace OrbisGL.GL2D
{
    /// <summary>
    /// Makes any <see cref="GLObject2D"/> acts as an sprite
    /// through the <see cref="SetVisibleRectangle(Rectangle)"/> method,
    /// by ensuring the visible rectangle will allways start in the position
    /// of the <see cref="Sprite2D"/> instance
    /// </summary>
    public class Sprite2D : GLObject2D
    {
        GLObject2D Target;

        int _FrameDelay;

        /// <summary>
        /// Sets an delay in miliseconds for advance the sprite in the next frame
        /// automatically, where 0 means disabled
        /// </summary>
        public int FrameDelay { get => _FrameDelay; 
            set 
            {
                _FrameDelay = value;
                FrameDelayTicks = value * Constants.SCE_MILISECOND;
            }
        }


        int FrameDelayTicks;
        int CurrentFrame = 0;

        public Sprite2D(GLObject2D Content)
        {
            if (Content is null)
                throw new ArgumentNullException(nameof(Content));

            Target = Content;

            base.Width = Target.Width;
            base.Height = Target.Height;

            base.AddChild(Content);
        }

        public override void SetVisibleRectangle(Rectangle Parent)
        {
            Target.Position = -Parent.Position;
            Width = (int)Parent.Width;
            Height = (int)Parent.Height;

            Target.SetVisibleRectangle(Parent);
        }

        public override void ClearVisibleRectangle()
        {
            Target.Position = Vector2.Zero;
            Width = Target.Width;
            Height = Target.Height;
            base.ClearVisibleRectangle();
        }

        public override void AddChild(GLObject2D Child)
        {
            Target.AddChild(Child);
        }

        public override void RemoveChild(GLObject2D Child)
        {
            Target.RemoveChild(Child);
        }

        public override void RemoveChildren(bool Dispose)
        {
            Target.RemoveChildren(Dispose);
        }

        public Rectangle[] Frames { get; set; } = new Rectangle[0];

        /// <summary>
        /// Calculate all frames rectangle by the frame amount
        /// </summary>
        /// <param name="TotalFrames"></param>
        public void ComputeAllFrames(int TotalFrames)
        {
            var Frame = new Rectangle(0, 0, Width, Height);
            var RowCount = Target.Width / Width;

            Frames = GetAllFrames(Frame, RowCount, TotalFrames, Target.Width, Target.Height);
        }

        /// <summary>
        /// Calculate all frame rectangles by the given sprite params
        /// </summary>
        /// <param name="FirstFrame">The first frame rectangle</param>
        /// <param name="TotalFrames">The total frame count</param>
        /// <param name="FramesPerRow">The max frame count in each row</param>
        public void ComputeAllFrames(int TotalFrames, Rectangle? FirstFrame, int? FramesPerRow = null)
        {
            var Frame = FirstFrame ?? new Rectangle(0, 0, Target.Width, Target.Height);
            var RowCount = FramesPerRow ?? Target.Width / Width;

            Frames = GetAllFrames(Frame, RowCount, TotalFrames, Target.Width, Target.Height);
        }

        public static Rectangle[] GetAllFrames(Rectangle FirstFrame, int FramesPerLine, int TotalFrames, int MaxWidth, int MaxHeight)
        {
            var Rects = new Rectangle[TotalFrames];

            var Rect = FirstFrame;

            for (int i = 0; i < TotalFrames; i++)
            {
                Rect.X = (Rect.Width * (i % FramesPerLine)) + FirstFrame.X;
                Rect.Y = (Rect.Height * (i / FramesPerLine)) + FirstFrame.Y;

                if (Rect.Right > MaxWidth)
                {
                    Rect.X = FirstFrame.X;
                    Rect.Y += Rect.Height;
                }

                if (Rect.Bottom > MaxHeight)
                    Rect.Y = FirstFrame.Y;

                Rects[i] = Rect;
            }

            return Rects;
        }


        /// <summary>
        /// Set the next frame visible
        /// </summary>
        public void NextFrame()
        {
            if (Width == 0)
                throw new ArgumentOutOfRangeException(nameof(Width));

            if (Height == 0)
                throw new ArgumentOutOfRangeException(nameof(Height));

            if (Frames == null || !Frames.Any())
                throw new ArgumentException("Missing Frame Info");

            if (Frames.Length != 0 && CurrentFrame >= Frames.Length)
                CurrentFrame = 0;

            SetVisibleRectangle(Frames[CurrentFrame]);
            CurrentFrame++;
        }


        /// <summary>
        /// Set the given frame visible
        /// </summary>
        public void SetCurrentFrame(int Step)
        {
            CurrentFrame = Step;

            NextFrame();
        }

        long LastStepTick = -1;
        public override void Draw(long Tick)
        {
            if (Width != 0 && Height != 0 && FrameDelayTicks != 0)
            {
                if ((Tick - LastStepTick) > FrameDelayTicks)
                {
                    LastStepTick = Tick;
                    NextFrame();
                }
            }

            base.Draw(Tick);
        }
    }
}
