using OrbisGL.Controls.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Input.Dualshock
{
    public class GamepadListener : IDisposable
    {
        //analogic (sticks, Touch, etc) data is converted in
        //a space from -1 to 1 for the gamepad implementation
        //be easy ported for different controllers

        public event ButtonEventHandler OnButtonDown;
        public event ButtonEventHandler OnButtonUp;

        public event MoveEventHandler OnLeftStickMove;
        public event MoveEventHandler OnRightStickMove;

        public event TouchEventHandler OnTouchStart;
        public event TouchEventHandler OnTouchEnd;
        public event TouchEventHandler OnTouchMove;

        public readonly Gamepad Dualshock;

        OrbisPadButton PressedBefore;

        public Vector2 LeftStick { get; private set; } = Vector2.Zero;
        public Vector2 RightStick { get; private set; } = Vector2.Zero;

        public Vector2? FingerA { get; set; } = null;
        public Vector2? FingerB { get; set; } = null;

        public GamepadListener(int UserID)
        {
#if ORBIS
            Dualshock = new Gamepad();
            Dualshock.Open(UserID);
#endif
        }

        public void Refresh()
        {
            Dualshock.Refresh();
            ProcessButtons();
            ProcessSticks();
            ProcessTouch();
        }

        private void ProcessButtons()
        {
            var PressedNow = Dualshock.CurrentData.Buttons;

            var Changes = PressedBefore ^ PressedNow;
            var NewPressed = Changes & PressedNow;
            var NewReleased = Changes & (~PressedNow);

            if (NewPressed != 0)
                OnButtonDown?.Invoke(this, new ButtonEventArgs(NewPressed));

            if (NewReleased != 0)
                OnButtonUp?.Invoke(this, new ButtonEventArgs(NewReleased));

        }

        private void ProcessSticks()
        {
            Vector2 LStick = Dualshock.CurrentData.LeftStick;
            Vector2 RStick = Dualshock.CurrentData.RightStick;

            if (LStick != LeftStick)
            {
                OnLeftStickMove?.Invoke(this, new MoveEventArgs(LStick));

                LeftStick = LStick;
            }

            if (RStick != RightStick)
            {
                OnRightStickMove?.Invoke(this, new MoveEventArgs(RStick));

                RightStick = RStick;
            }
        }

        private void ProcessTouch()
        {
            Finger ActiveFingers = 0;
            for (int i = 0; i < Dualshock.CurrentData.Touch.Fingers; i++)
            {
                var CurrentFinger = Dualshock.CurrentData.Touch.Touch[i];
                switch (CurrentFinger.Finger)
                {
                    case 0:
                        ActiveFingers |= Finger.A;
                        if (FingerA != null && FingerA != CurrentFinger)
                        {
                            OnTouchMove?.Invoke(this, new TouchEventArgs(CurrentFinger, Finger.A));
                        }
                        if (FingerA == null)
                        {
                            OnTouchStart?.Invoke(this, new TouchEventArgs(CurrentFinger, Finger.A));
                        }
                        FingerA = CurrentFinger;
                        break;
                    case 1:
                        ActiveFingers |= Finger.B;
                        if (FingerB != null && FingerB != CurrentFinger)
                        {
                            OnTouchMove?.Invoke(this, new TouchEventArgs(CurrentFinger, Finger.B));
                        }
                        if (FingerB == null)
                        {
                            OnTouchStart?.Invoke(this, new TouchEventArgs(CurrentFinger, Finger.B));
                        }
                        FingerB = CurrentFinger;
                        break;
                }
            }

            if (FingerA != null && !ActiveFingers.HasFlag(Finger.A))
            {
                OnTouchEnd?.Invoke(this, new TouchEventArgs(FingerA.Value, Finger.A));
                FingerA = null;
            }

            if (FingerB != null && !ActiveFingers.HasFlag(Finger.B))
            {
                OnTouchEnd?.Invoke(this, new TouchEventArgs(FingerB.Value, Finger.B));
                FingerB = null;
            }
        }

        public void Dispose()
        {
            Dualshock?.Dispose();
        }
    }
}
