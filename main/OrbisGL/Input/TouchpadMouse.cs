using OrbisGL.Controls.Events;
using OrbisGL.Input.Dualshock;
using System.Numerics;
using static OrbisGL.GL2D.Coordinates2D;

namespace OrbisGL.Input
{
    public class TouchpadMouse : IMouse
    {
        public GamepadListener Gamepad;

        float _Sensitivity = 0.5f;
        public float Sensitivity
        {
            get => _Sensitivity;
            set
            {
                _Sensitivity = value;
                TouchMax = new Vector2(Constants.MaxTouchX * value, Constants.MaxTouchX * value);
                DeadDistance = TouchMax * 0.02f;
            }
        }

        int FingerCount;
        long FingerTouchStartTick;
        long FingerTouchEndTick;
        
        long ElapsedTouchStartMilesecond => (LastRefreshTick - FingerTouchStartTick) / Constants.SCE_MILISECOND;
        long ElapsedTouchEndMilesecond => (LastRefreshTick - FingerTouchEndTick) / Constants.SCE_MILISECOND;


        int LeftState = 0;//0 = not pressed, 1 = clicked, 2 = hold

        bool RightPressed;

        long LastRefreshTick;

        Vector2 FingerInitialPos;
        Vector2 InitialCursorPos;
        Vector2 DeadDistance;
        Vector2 TouchMax;


        private const int PressDelay = 300;
        
        public TouchpadMouse(GamepadListener Gamepad)
        {
            this.Gamepad = Gamepad;

            this.Gamepad.OnTouchStart += Gamepad_OnTouchStart;
            this.Gamepad.OnTouchEnd += Gamepad_OnTouchEnd;
            this.Gamepad.OnTouchMove += Gamepad_OnTouchMove;
            this.Gamepad.OnButtonDown += Gamepad_OnButtonDown;
            this.Gamepad.OnButtonUp += Gamepad_OnButtonUp;
        }

        private void Gamepad_OnButtonDown(object Sender, ButtonEventArgs Args)
        {
            if (Args.Button == OrbisPadButton.TouchPad)
            {
                Args.Handled = true;
                RightPressed = true;
                FingerCount = 0;
            }
        }
        private void Gamepad_OnButtonUp(object Sender, ButtonEventArgs Args)
        {
            if (Args.Button == OrbisPadButton.TouchPad)
            {
                Args.Handled = true;
                RightPressed = false;
                FingerCount = 0;
            }
        }

        private void Gamepad_OnTouchStart(object Sender, TouchEventArgs Args)
        {
            Args.Handled = true;

            if (FingerCount == 0)
            {
                FingerInitialPos = Args.Position;
                InitialCursorPos = CurrentPos;
                FingerTouchStartTick = LastRefreshTick;

                if (LeftState == 1 && ElapsedTouchEndMilesecond < PressDelay)
                {
                    LeftState++;
                }
            }

            FingerCount++;

            if (FingerCount > 2)
                FingerCount = 2;
        }
        private void Gamepad_OnTouchEnd(object Sender, TouchEventArgs Args)
        {
            Args.Handled = true;

            if (FingerCount == 1)
            {
                var InitialPos = GetXY(FingerInitialPos);
                var EndPos = GetXY(Args.Position);

                var DeltaPos = Vector2.Abs(EndPos - InitialPos);

                //Too little pixels moved, trigger an click
                if (DeltaPos.X < DeadDistance.X && DeltaPos.Y < DeadDistance.Y)
                {
                    LeftState = 1;
                    FingerTouchEndTick = LastRefreshTick;
                }

                //Finish Drag
                if (LeftState == 2)
                    LeftState = 0;
            }

            FingerCount--;

            if (FingerCount < 0)
                FingerCount = 0;
        }
        
        
        private void Gamepad_OnTouchMove(object Sender, TouchEventArgs Args)
        {
            Args.Handled = true;

            var FingerInitialPos = GetXY(this.FingerInitialPos);
            var FingerEndPos = GetXY(Args.Position);

            var FingerDeltaPos = FingerEndPos - FingerInitialPos;

            var FingerDiff = Vector2.Abs(FingerDeltaPos);
            
            
            //For help the click accuracy the begin of the move will be ignored
            //fow some few milliseconds and an certain distance.
            if (FingerDiff.X < DeadDistance.X && FingerDiff.Y < DeadDistance.Y && ElapsedTouchStartMilesecond < PressDelay)
                return;

            CurrentPos = InitialCursorPos + FingerDeltaPos;

            CurrentPos = Vector2.Max(CurrentPos, Vector2.Zero);
            CurrentPos = Vector2.Min(CurrentPos, ScreenSize);
            
            //[WIP] Consider the finger move speed and make it increase the distance somehow
        }

        private Vector2 GetXY(Vector2 Offset)
        {
            return new Vector2(PointToX(Offset.X, (int)TouchMax.X), PointToY(Offset.Y, (int)TouchMax.Y));
        }

        Vector2 ScreenSize = new Vector2(Width, Height);
        Vector2 CurrentPos = new Vector2(Width, Height) / 2;

        bool Disposed = false;
        public void Dispose()
        {
            if (Disposed)
                return;

            Disposed = true;
            Gamepad.OnButtonDown -= Gamepad_OnButtonDown;
            Gamepad.OnButtonUp -= Gamepad_OnButtonUp;
            Gamepad.OnTouchStart -= Gamepad_OnTouchStart;
            Gamepad.OnTouchEnd -= Gamepad_OnTouchEnd;
            Gamepad.OnTouchMove -= Gamepad_OnTouchMove;
        }

        public MouseButtons GetMouseButtons()
        {
            MouseButtons Buttons = 0;

            if (LeftState == 1)
            {
                long Elapsed = ElapsedTouchEndMilesecond;
                if (Elapsed > PressDelay)
                {
                    LeftState = 0;
                }
            }

            if (LeftState > 0)
            { 
                Buttons |= MouseButtons.Left;
            }

            if (RightPressed)
            {
                Buttons |= MouseButtons.Right;
            }

            return Buttons;
        }

        public Vector2 GetPosition()
        {
            return CurrentPos;
        }

        public bool Initialize(int UserID = -1)
        {
            Sensitivity = 0.5f;
            return true;
        }

        public void RefreshData(long Tick)
        {
            LastRefreshTick = Tick;   
        }
    }
}
