using OrbisGL.Controls.Events;
using OrbisGL.Input.Layouts;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OrbisGL.Input
{
    internal unsafe class OrbisKeyboard
    {
        public ILayout KeyboardLayout { get; set; } = new ASCII();
        public event KeyboardEventDelegate OnKeyDown;
        public event KeyboardEventDelegate OnKeyUp;

        void KeyboardEvent(IntPtr Arg, ref OrbisKeyboardEvent Event)
        {
            switch (Event.id)
            {
                case IME_KeyboardEvent.OPEN: break;
                case IME_KeyboardEvent.KEYCODE_DOWN:
                    var KeyCode = Event.param.keycode;
                    var EventArgs = new KeyboardEventArgs(KeyCode.keycode, KeyCode.status, GetKeyChar(KeyCode.keycode, KeyCode.status));
                    OnKeyDown?.Invoke(this, EventArgs);
                    break;
            }
        }

        #region CharMapping
        char? GetKeyChar(IME_KeyCode Code, IME_KeycodeState State)
        {
            bool Numlock = State.HasFlag(IME_KeycodeState.LED_NUM_LOCK);
            bool Shift = State.HasFlag(IME_KeycodeState.MODIFIER_L_SHIFT) || State.HasFlag(IME_KeycodeState.MODIFIER_R_SHIFT) || State.HasFlag(IME_KeycodeState.LED_CAPS_LOCK);
            bool AltGr = State.HasFlag(IME_KeycodeState.MODIFIER_R_ALT);
            
            char? Result = KeyboardLayout.GetKeyChar(new IMEKeyModifier(Code, Shift, AltGr, Numlock));

            if (Result != null)
                return Result;
            
            //Parse A-Z
            if (Code >= IME_KeyCode.A && Code <= IME_KeyCode.Z) {
                char Begin = Shift ? 'A' : 'a';
                Result = (char)(Begin + ((int)IME_KeyCode.A - (int)Code));

                return Result;
            }

            if (Code >= IME_KeyCode.N1 && Code <= IME_KeyCode.N9)
            {
                Result = (char)('1' + ((int)IME_KeyCode.N1 - (int)Code));
                return Result;
            }

            if (Numlock && Code >= IME_KeyCode.KEYPAD_1 && Code <= IME_KeyCode.KEYPAD_9)
            {
                Result = (char)('1' + ((int)IME_KeyCode.KEYPAD_1 - (int)Code));

                return Result;
            }

            if (Numlock && Code == IME_KeyCode.KEYPAD_0)
            {
                Result = '0';
                return Result;
            }

            switch (Code)
            {
                case IME_KeyCode.N0:
                    Result = '0';
                    break;
                case IME_KeyCode.MINUS:
                    Result = Shift ? '_' : '-';
                    break;
                case IME_KeyCode.EQUAL:
                    Result = Shift ? '+' : '=';
                    break;
                case IME_KeyCode.RETURN:
                    Result = '\n';
                    break;
                case IME_KeyCode.SPACEBAR:
                    Result = ' ';
                    break;
                case IME_KeyCode.LEFTBRACKET:
                    Result = Shift ? '{' : '[';
                    break;
                case IME_KeyCode.RIGHTBRACKET:
                    Result = Shift ? '}' : ']';
                    break;
                case IME_KeyCode.BACKSLASH:
                    Result = Shift ? '|' : '\\';
                    break;
                case IME_KeyCode.SEMICOLON:
                    Result = Shift ? ':' : ';';
                    break;
                case IME_KeyCode.SINGLEQUOTE:
                    Result = Shift ? '"' : '\'';
                    break;
                case IME_KeyCode.BACKQUOTE:
                    Result = Shift ? '~' : '`';
                    break;
                case IME_KeyCode.COMMA:
                    Result = Shift ? '<' : ',';
                    break;
                case IME_KeyCode.PERIOD:
                    Result = Shift ? '>' : '.';
                    break;
                case IME_KeyCode.SLASH:
                    Result = Shift ? '?' : '/';
                    break;
            }

            return Result;
        }
        #endregion

        bool Initialized = false;
        public bool Initialize(int UserID = -1)
        {
            if (UserID < 0 || Initialized)
                return false;

            OrbisKeyboardParam Param = new OrbisKeyboardParam();
            Param.handler = KeyboardEvent;

            if (sceImeKeyboardOpen(UserID, Param) != Constants.SCE_OK)
                return false;


            Initialized = true;
        }

        [DllImport("libSceIme.sprx")]
        static extern int sceImeKeyboardClose(int UserId);

        [DllImport("libSceIme.sprx")]
        static extern int sceImeUpdate(OrbisKeyboardEventHandler Handler);

        [DllImport("libSceIme.sprx")]
        static extern int sceImeKeyboardOpen(int UserId, OrbisKeyboardParam param);

        public struct OrbisKeyboardConfig
        {
            public int userId;
            public int status;
            public int caps_key;
            public int control_key_left;
            public int control_key_right;
            public int shift_key_left;
            public int shift_key_right;
            public int alt_key_left;
            public int alt_key_right;
            public int cmd_key_left;
            public int cmd_key_right;
            public byte key;
            public byte key_status;
            public int orbiskeyboard_initialized;
        }

        public struct OrbisKeyboardResourceIdArray
        {
            public int userId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public uint[] resourceId;
        }

        public struct OrbisKeyboardKeycode
        {
            public IME_KeyCode keycode;
            public ushort unknown;
            public IME_KeycodeState status;
            public uint unknown1;
            public int userId;
            public uint resourceId;
            public uint unknown2;
            public ulong timestamp;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct OrbisKeyboardEventParam
        {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 84)]
            public byte[] notused;

            [FieldOffset(0)]
            public OrbisKeyboardKeycode keycode;

            [FieldOffset(0)]
            public OrbisKeyboardResourceIdArray resourceIdArray;

            [FieldOffset(0)]
            public IntPtr candidateWord;

            [FieldOffset(0)]
            public int candidateIndex;

            [FieldOffset(0)]
            public int deviceType;

            [FieldOffset(0)]
            public uint inputMethodState;

            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] reserved;
        }


        public struct OrbisKeyboardEvent
        {
            public IME_KeyboardEvent id;
            public OrbisKeyboardEventParam param;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OrbisKeyboardEventHandler(IntPtr Arg, ref OrbisKeyboardEvent Event);

        public struct OrbisKeyboardParam
        {
            public uint option;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] unknown1;
            public IntPtr arg;
            public OrbisKeyboardEventHandler handler;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] unknown2;
        }
    }
}
