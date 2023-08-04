using OrbisGL.GL;
using System;
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

        int _StepDelay;

        /// <summary>
        /// Sets an delay in miliseconds for advance the sprite in the next frame
        /// automatically, where 0 means disabled
        /// </summary>
        public int StepDelay { get => _StepDelay; 
            set 
            {
                _StepDelay = value;
                StepDelayTicks = value * Constants.SCE_MILISECOND;
            }
        }

        /// <summary>
        /// The max frame loop step
        /// </summary>
        public int MaxStep { get; set; }

        public int? _StepsPerLine;

        /// <summary>
        /// The max sprite frames per line,
        /// when 0 the amount will be automatically set by the target size
        /// </summary>
        public int StepsPerLine
        {
            get => _StepsPerLine ?? Target.Width / Width;
            set
            {
                _StepsPerLine = value;

                if (value == 0)
                    _StepsPerLine = null;
            }
        }

        int StepDelayTicks;
        int CurrentStep = 0;

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
            CurrentStep = 0;

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

        public Rectangle? FirstRectangle { get; set; } = null;

        /// <summary>
        /// Get the next rectangle for the next <see cref="StepFoward"/> call
        /// </summary>
        public Rectangle NextRectangle {
            get 
            {
                if (FirstRectangle == null)
                    FirstRectangle = new Rectangle(0, 0, Width, Height);

                var Rect = FirstRectangle ?? throw new ArgumentNullException(nameof(FirstRectangle));

                if (CurrentStep > 0)
                {
                    Rect.X += Rect.Width * (CurrentStep % StepsPerLine);
                    Rect.Y += Rect.Height * (CurrentStep / StepsPerLine);

                    if (Rect.Right > Target.Width)
                    {
                        Rect.X = 0;
                        Rect.Y += Rect.Height;
                    }

                    if (Rect.Bottom > Target.Height)
                        Rect.Y = 0;
                }

                return Rect;
            }
        }


        /// <summary>
        /// Move the visiblity by one rectangle ahead
        /// </summary>
        public void StepFoward()
        {
            if (Width == 0)
                throw new ArgumentOutOfRangeException(nameof(Width));

            if (Height == 0)
                throw new ArgumentOutOfRangeException(nameof(Height));

            if (MaxStep != 0 && CurrentStep >= MaxStep)
                CurrentStep = 0;            

            int NewStep = CurrentStep + 1;
            SetVisibleRectangle(NextRectangle);
            CurrentStep = NewStep;
        }

        public void SetStep(int Step)
        {
            CurrentStep = Step;

            StepFoward();
        }

        long LastStepTick = -1;
        public override void Draw(long Tick)
        {
            if (Width != 0 && Height != 0)
            {
                if (LastStepTick == -1)
                {
                    LastStepTick = Tick;
                }
                else if ((Tick - LastStepTick) > StepDelayTicks)
                {
                    LastStepTick = Tick;
                    StepFoward();
                }
            }

            base.Draw(Tick);
        }
    }
}
