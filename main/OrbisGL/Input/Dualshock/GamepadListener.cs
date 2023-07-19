using OrbisGL.Controls.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public byte hFingerA = 255;
        public byte hFingerB = 255;
        public Vector2? FingerA { get; set; } = null;
        public Vector2? FingerB { get; set; } = null;

        public GamepadListener(int UserID)
        {
#if ORBIS
            Dualshock = new Gamepad();
            Dualshock.Open(UserID);
#endif
        }

        public void RefreshData()
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

            if (PressedNow.HasFlag(OrbisPadButton.Invalid))
                return;
            
            if (NewPressed != 0)
                OnButtonDown?.Invoke(this, new ButtonEventArgs(NewPressed));

            if (NewReleased != 0)
                OnButtonUp?.Invoke(this, new ButtonEventArgs(NewReleased));
            
            PressedBefore = PressedNow;
        }

        private void ProcessSticks()
        {
            Vector2 LStick = (Vector2)Dualshock.CurrentData.LeftStick;
            Vector2 RStick = (Vector2)Dualshock.CurrentData.RightStick;

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

        private Dictionary<int, Finger> FingerMap = new Dictionary<int, Finger>(); 
        private void ProcessTouch()
        {
            Finger ActiveFingers = 0;
            
            int TotalFingers = Dualshock.CurrentData.Touch.Fingers;
            
            FingerMap.Clear();
            for (int i = 0; i < TotalFingers; i++)
            {
                var CurrentFinger = Dualshock.CurrentData.Touch.Touch[i];
                if (CurrentFinger.Finger == hFingerA)
                    ActiveFingers |= FingerMap[i] = Finger.A;
                else if (CurrentFinger.Finger == hFingerB)
                    ActiveFingers |= FingerMap[i] = Finger.B;
            }

            Finger FreeFinger = (~ActiveFingers) & (Finger.Both);
                
            for (int i = 0; i < TotalFingers; i++)
            {
                var CurrentFinger = Dualshock.CurrentData.Touch.Touch[i];
                var CurrentPosition = (Vector2)CurrentFinger;
                
                Finger Current = FingerMap.TryGetValue(i, out var value) ? value : FreeFinger;

                if (Current == Finger.Both)
                    Current = Finger.A;
                
                ActiveFingers |= Current;
                
                ProcessFinger(Current, CurrentPosition, CurrentFinger.Finger);
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
        
        private void ProcessFinger(Finger Current, Vector2 CurrentPosition, byte hFinger)
        {
            switch (Current)
            {
                case Finger.A:
                    if (FingerA != null && FingerA != CurrentPosition)
                    {
                        OnTouchMove?.Invoke(this, new TouchEventArgs(CurrentPosition, Finger.A));
                    }

                    if (FingerA == null)
                    {
                        OnTouchStart?.Invoke(this, new TouchEventArgs(CurrentPosition, Finger.A));
                    }

                    FingerA = CurrentPosition;
                    hFingerA = hFinger;
                    break;
                case Finger.B:
                    if (FingerB != null && FingerB != CurrentPosition)
                    {
                        OnTouchMove?.Invoke(this, new TouchEventArgs(CurrentPosition, Finger.B));
                    }

                    if (FingerB == null)
                    {
                        OnTouchStart?.Invoke(this, new TouchEventArgs(CurrentPosition, Finger.B));
                    }

                    FingerB = CurrentPosition;
                    hFingerB = hFinger;
                    break;
            }
        }

        public void Dispose()
        {
            Dualshock?.Dispose();
        }
    }
}
