using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OrbisGL.Input;
using System.Numerics;
using OrbisGL.Controls.Events;
using System.Collections.Generic;

using Application = OrbisGL.GL.Application;
using MButtons = OrbisGL.MouseButtons;
using System.Linq;

namespace GLTest
{

    public partial class GLControl : Control
    {
#if !ORBIS
        public Application GLApplication;
        public GLControl(int Width, int Height)
        {
            Size = new Size(Width, Height);

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        }

        const int WM_CREATE = 0x0001;
        const int WM_TIMER = 0x0113;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WM_CREATE:
                    SetTimer(Handle, 1, 20, IntPtr.Zero);
                    break;
                case WM_TIMER:
                    Invalidate();
                    break;
            }
        }

        [DllImport("user32.dll")]
        static extern UIntPtr SetTimer(IntPtr hWnd, int nIDEvent, uint uElapse, IntPtr lpTimerFunc);

        protected override void OnPaint(PaintEventArgs e)
        {
            GLApplication?.DrawOnce();
            GLApplication?.SwapBuffers();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            GLApplication = new Application(Width, Height, 30, Handle);

            GLApplication.MouseDriver = new GenericMouse(() =>
            {
                var Pos = Cursor.Position;
                var CPos = PointToClient(Pos);
                return new Vector2(CPos.X, CPos.Y);
            }, () =>
            {
                MButtons Buttons = 0;
                if (MouseButtons.HasFlag(MouseButtons.Left))
                    Buttons |= MButtons.Left;
                if (MouseButtons.HasFlag(MouseButtons.Right))
                    Buttons |= MButtons.Right;
                if (MouseButtons.HasFlag(MouseButtons.Middle))
                    Buttons |= MButtons.Middle;

                if (Buttons != 0 && DisplayRectangle.IntersectsWith(new Rectangle(Cursor.Position, new Size(1, 1))))
                    Focus();

                return Buttons;
            });

            GLApplication.KeyboardDriver = Keyboard = new DesktopKeyboard();
            GLApplication.EnableKeyboard();

            
        }

        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_CHAR = 0x102;
        const int WM_SYSKEYDOWN = 0x0104;
        const int WM_SYSKEYUP = 0x0105;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (msg.Msg)
            {
                case WM_SYSKEYDOWN:
                case WM_KEYDOWN:
                    if (Keyboard != null)
                    {
                        Keyboard.KeyDown(keyData);
                        Keyboard.KeyUp(keyData);
                        return true;
                    }
                    break;

                case WM_SYSKEYUP:
                case WM_KEYUP:
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        DesktopKeyboard Keyboard;

        protected override void OnHandleDestroyed(EventArgs e)
        {
            GLApplication.Dispose();

            base.OnHandleDestroyed(e);
        }

        class DesktopKeyboard : IKeyboard
        {
            Dictionary<Keys, OrbisGL.IME_KeyCode> KeyMap = new Dictionary<Keys, OrbisGL.IME_KeyCode>() 
            {
#region Mapping
                { Keys.Up, OrbisGL.IME_KeyCode.UPARROW },
                { Keys.Down, OrbisGL.IME_KeyCode.DOWNARROW },
                { Keys.Left, OrbisGL.IME_KeyCode.LEFTARROW },
                { Keys.Right, OrbisGL.IME_KeyCode.RIGHTARROW },
                { Keys.A, OrbisGL.IME_KeyCode.A },
                { Keys.B, OrbisGL.IME_KeyCode.B },
                { Keys.C, OrbisGL.IME_KeyCode.C },
                { Keys.D, OrbisGL.IME_KeyCode.D },
                { Keys.E, OrbisGL.IME_KeyCode.E },
                { Keys.F, OrbisGL.IME_KeyCode.F },
                { Keys.G, OrbisGL.IME_KeyCode.G },
                { Keys.H, OrbisGL.IME_KeyCode.H },
                { Keys.I, OrbisGL.IME_KeyCode.I },
                { Keys.J, OrbisGL.IME_KeyCode.J },
                { Keys.K, OrbisGL.IME_KeyCode.K },
                { Keys.L, OrbisGL.IME_KeyCode.L },
                { Keys.M, OrbisGL.IME_KeyCode.M },
                { Keys.N, OrbisGL.IME_KeyCode.N },
                { Keys.O, OrbisGL.IME_KeyCode.O },
                { Keys.P, OrbisGL.IME_KeyCode.P },
                { Keys.Q, OrbisGL.IME_KeyCode.Q },
                { Keys.R, OrbisGL.IME_KeyCode.R },
                { Keys.S, OrbisGL.IME_KeyCode.S },
                { Keys.T, OrbisGL.IME_KeyCode.T },
                { Keys.U, OrbisGL.IME_KeyCode.U },
                { Keys.V, OrbisGL.IME_KeyCode.V },
                { Keys.W, OrbisGL.IME_KeyCode.W },
                { Keys.X, OrbisGL.IME_KeyCode.X },
                { Keys.Y, OrbisGL.IME_KeyCode.Y },
                { Keys.Z, OrbisGL.IME_KeyCode.Z },
                { Keys.D1, OrbisGL.IME_KeyCode.N1 },
                { Keys.D2, OrbisGL.IME_KeyCode.N2 },
                { Keys.D3, OrbisGL.IME_KeyCode.N3 },
                { Keys.D4, OrbisGL.IME_KeyCode.N4 },
                { Keys.D5, OrbisGL.IME_KeyCode.N5 },
                { Keys.D6, OrbisGL.IME_KeyCode.N6 },
                { Keys.D7, OrbisGL.IME_KeyCode.N7 },
                { Keys.D8, OrbisGL.IME_KeyCode.N8 },
                { Keys.D9, OrbisGL.IME_KeyCode.N9 },
                { Keys.D0, OrbisGL.IME_KeyCode.N0 },
                { Keys.Return, OrbisGL.IME_KeyCode.RETURN },
                { Keys.Escape, OrbisGL.IME_KeyCode.ESCAPE },
                { Keys.Back, OrbisGL.IME_KeyCode.BACKSPACE },
                { Keys.Tab, OrbisGL.IME_KeyCode.TAB },
                { Keys.Space, OrbisGL.IME_KeyCode.SPACEBAR },
                { Keys.OemMinus, OrbisGL.IME_KeyCode.MINUS },
                { Keys.Oemplus, OrbisGL.IME_KeyCode.EQUAL },
                { Keys.OemOpenBrackets, OrbisGL.IME_KeyCode.LEFTBRACKET },
                { Keys.OemCloseBrackets, OrbisGL.IME_KeyCode.RIGHTBRACKET },
                { Keys.OemBackslash, OrbisGL.IME_KeyCode.BACKSLASH },
                { Keys.OemPipe, OrbisGL.IME_KeyCode.NONUS_POUND },
                { Keys.OemSemicolon, OrbisGL.IME_KeyCode.SEMICOLON },
                { Keys.OemQuotes, OrbisGL.IME_KeyCode.SINGLEQUOTE },
                { Keys.Oemtilde, OrbisGL.IME_KeyCode.BACKQUOTE },
                { Keys.Oemcomma, OrbisGL.IME_KeyCode.COMMA },
                { Keys.OemPeriod, OrbisGL.IME_KeyCode.PERIOD },
                { Keys.OemQuestion, OrbisGL.IME_KeyCode.SLASH },
                { Keys.CapsLock, OrbisGL.IME_KeyCode.CAPSLOCK },
                { Keys.F1, OrbisGL.IME_KeyCode.F1 },
                { Keys.F2, OrbisGL.IME_KeyCode.F2 },
                { Keys.F3, OrbisGL.IME_KeyCode.F3 },
                { Keys.F4, OrbisGL.IME_KeyCode.F4 },
                { Keys.F5, OrbisGL.IME_KeyCode.F5 },
                { Keys.F6, OrbisGL.IME_KeyCode.F6 },
                { Keys.F7, OrbisGL.IME_KeyCode.F7 },
                { Keys.F8, OrbisGL.IME_KeyCode.F8 },
                { Keys.F9, OrbisGL.IME_KeyCode.F9 },
                { Keys.F10, OrbisGL.IME_KeyCode.F10 },
                { Keys.F11, OrbisGL.IME_KeyCode.F11 },
                { Keys.F12, OrbisGL.IME_KeyCode.F12 },
                { Keys.PrintScreen, OrbisGL.IME_KeyCode.PRINTSCREEN },
                { Keys.Scroll, OrbisGL.IME_KeyCode.SCROLLLOCK },
                { Keys.Pause, OrbisGL.IME_KeyCode.PAUSE },
                { Keys.Insert, OrbisGL.IME_KeyCode.INSERT },
                { Keys.Home, OrbisGL.IME_KeyCode.HOME },
                { Keys.Delete, OrbisGL.IME_KeyCode.DELETE },
                { Keys.End, OrbisGL.IME_KeyCode.END },
                { Keys.PageUp, OrbisGL.IME_KeyCode.PAGEUP },
                { Keys.PageDown, OrbisGL.IME_KeyCode.PAGEDOWN },
                { Keys.NumLock, OrbisGL.IME_KeyCode.KEYPAD_NUMLOCK },
                { Keys.NumPad0, OrbisGL.IME_KeyCode.KEYPAD_0 },
                { Keys.NumPad1, OrbisGL.IME_KeyCode.KEYPAD_1 },
                { Keys.NumPad2, OrbisGL.IME_KeyCode.KEYPAD_2 },
                { Keys.NumPad3, OrbisGL.IME_KeyCode.KEYPAD_3 },
                { Keys.NumPad4, OrbisGL.IME_KeyCode.KEYPAD_4 },
                { Keys.NumPad5, OrbisGL.IME_KeyCode.KEYPAD_5 },
                { Keys.NumPad6, OrbisGL.IME_KeyCode.KEYPAD_6 },
                { Keys.NumPad7, OrbisGL.IME_KeyCode.KEYPAD_7 },
                { Keys.NumPad8, OrbisGL.IME_KeyCode.KEYPAD_8 },
                { Keys.NumPad9, OrbisGL.IME_KeyCode.KEYPAD_9 },
                { Keys.Multiply, OrbisGL.IME_KeyCode.KEYPAD_MEMORY_MULTIPLY },
                { Keys.Add, OrbisGL.IME_KeyCode.KEYPAD_MEMORY_ADD },
                { Keys.Subtract, OrbisGL.IME_KeyCode.KEYPAD_MEMORY_SUBTRACT },
                { Keys.Divide, OrbisGL.IME_KeyCode.KEYPAD_MEMORY_DIVIDE },
                { Keys.Decimal, OrbisGL.IME_KeyCode.KEYPAD_DECIMAL },
                { Keys.LShiftKey, OrbisGL.IME_KeyCode.LEFTSHIFT },
                { Keys.RShiftKey, OrbisGL.IME_KeyCode.RIGHTSHIFT },
                { Keys.LControlKey, OrbisGL.IME_KeyCode.LEFTCONTROL },
                { Keys.RControlKey, OrbisGL.IME_KeyCode.RIGHTCONTROL },
                { Keys.Alt, OrbisGL.IME_KeyCode.LEFTALT },
                { Keys.RMenu, OrbisGL.IME_KeyCode.RIGHTALT },
                { Keys.LWin, OrbisGL.IME_KeyCode.LEFTGUI },
                { Keys.RWin, OrbisGL.IME_KeyCode.RIGHTGUI },
                { Keys.Apps, OrbisGL.IME_KeyCode.APPLICATION },
                { Keys.VolumeMute, OrbisGL.IME_KeyCode.MUTE },
                { Keys.VolumeDown, OrbisGL.IME_KeyCode.VOLUMEDOWN },
                { Keys.VolumeUp, OrbisGL.IME_KeyCode.VOLUMEUP },
                { Keys.ControlKey | Keys.Control, OrbisGL.IME_KeyCode.LEFTCONTROL },
                { Keys.Shift | Keys.ShiftKey, OrbisGL.IME_KeyCode.LEFTSHIFT }
#endregion
            };

            public event KeyboardEventDelegate OnKeyDown;
            public event KeyboardEventDelegate OnKeyUp;

            public bool Initialize(int UserID = -1)
            {
                Application.PhysicalKeyboardAvailable = true;
                return true;
            }

            public void RefreshData()
            {
                
            }

            public OrbisGL.Input.Layouts.ILayout Layout = OrbisKeyboard.Layouts.First();


            const Keys ModifierMask = (Keys.Shift | Keys.Control);

            public void KeyDown(Keys Key)
            {
                Keys Modifiers = Key & ModifierMask;

                if (Key == 0)
                    Key = Modifiers;

                if (!KeyMap.ContainsKey(Key)) {
                    Key &= ~Modifiers;
                    if (!KeyMap.ContainsKey(Key))
                        return;
                }

                var Modifier = new OrbisGL.IMEKeyModifier() 
                {
                    Code = KeyMap[Key],
                    Alt = Modifiers.HasFlag(Keys.Alt) | Modifiers.HasFlag(Keys.RMenu),
                    Shift = Modifiers.HasFlag(Keys.Shift) | Modifiers.HasFlag(Keys.RShiftKey) | Modifiers.HasFlag(Keys.LShiftKey),
                    NumLock = Modifiers.HasFlag(Keys.NumLock)
                };

                var Char = Layout.GetKeyChar(Modifier);

                OnKeyDown.Invoke(this, new KeyboardEventArgs(KeyMap[Key], OrbisGL.IME_KeycodeState.VALID, Char));
            }
            public void KeyUp(Keys Key)
            {
                Keys Modifiers = Key & ModifierMask;

                if (Key == 0)
                    Key = Modifiers;

                if (!KeyMap.ContainsKey(Key))
                {
                    Key &= ~Modifiers;
                    if (!KeyMap.ContainsKey(Key))
                        return;
                }

                var Modifier = new OrbisGL.IMEKeyModifier()
                {
                    Code = KeyMap[Key],
                    Alt = Modifiers.HasFlag(Keys.Alt) | Modifiers.HasFlag(Keys.RMenu),
                    Shift = Modifiers.HasFlag(Keys.Shift) | Modifiers.HasFlag(Keys.RShiftKey) | Modifiers.HasFlag(Keys.LShiftKey),
                    NumLock = Modifiers.HasFlag(Keys.NumLock)
                };


                var Char = Layout.GetKeyChar(Modifier);

                OnKeyUp.Invoke(this, new KeyboardEventArgs(KeyMap[Key], OrbisGL.IME_KeycodeState.VALID, Char));
            }
        }
#endif
    }
}