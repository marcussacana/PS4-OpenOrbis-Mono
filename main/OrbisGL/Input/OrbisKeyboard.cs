﻿using OrbisGL.Controls.Events;
using OrbisGL.Input.Layouts;
using System;
using System.Runtime.InteropServices;

namespace OrbisGL.Input
{
    internal unsafe class OrbisKeyboard : IKeyboard
    {
        public readonly static ILayout[] Layouts = new ILayout[]
        {
            new ASCII(), new ABNT2()
        };

        public ILayout KeyboardLayout { get; set; }
        public event KeyboardEventDelegate OnKeyDown;
        public event KeyboardEventDelegate OnKeyUp;

        void KeyboardEvent(IntPtr Arg, OrbisKeyboardEvent Event)
        {
            
            switch (Event.id)
            {
                case IME_KeyboardEvent.OPEN: break;
                case IME_KeyboardEvent.KEYCODE_DOWN:
                    var KeyCode = (OrbisKeyboardKeycode*)Event.EventData;
                    var EventArgs = new KeyboardEventArgs(KeyCode->keycode, KeyCode->status, GetKeyChar(KeyCode->keycode, KeyCode->status));
                    OnKeyDown?.Invoke(this, EventArgs);
                    break;
            }
        }

        char? GetKeyChar(IME_KeyCode Code, IME_KeycodeState State)
        {
            bool Numlock = State.HasFlag(IME_KeycodeState.LED_NUM_LOCK);
            bool Shift = State.HasFlag(IME_KeycodeState.MODIFIER_L_SHIFT) || State.HasFlag(IME_KeycodeState.MODIFIER_R_SHIFT) || State.HasFlag(IME_KeycodeState.LED_CAPS_LOCK);
            bool AltGr = State.HasFlag(IME_KeycodeState.MODIFIER_R_ALT);

            var KeyInfo = new IMEKeyModifier(Code, Shift, AltGr, Numlock);


            return KeyboardLayout.GetKeyChar(KeyInfo);
        }

        bool Initialized = false;
        public bool Initialize(int UserID = -1)
        {
            if (UserID < 0 || Initialized)
                return false;

            OrbisKeyboardParam Param = new OrbisKeyboardParam();
            Param.handler = KeyboardEvent;

            if (sceImeKeyboardOpen(UserID, Param) != Constants.SCE_OK)
                return false;

            if (KeyboardLayout == null && sceSystemServiceParamGetInt(Constants.SCE_SYSTEM_SERVICE_PARAM_ID_LANG, out int LangID) == Constants.SCE_OK)
            {
                foreach (var Layout in Layouts)
                {
                    if (Layout.LanguageID == LangID)
                    {
                        KeyboardLayout = Layout;
                        break;
                    }
                }
            }


            return Initialized = true;
        }


        [DllImport("libSceSystemService.sprx")]
        static extern int sceSystemServiceParamGetInt(int paramId, out int Value);

        [DllImport("libSceIme.sprx")]
        static extern int InitializeImeModule();
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

        /*
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
        */

        public struct OrbisKeyboardEvent
        {
            public IME_KeyboardEvent id;
            public fixed byte EventData[84];
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void OrbisKeyboardEventHandler(IntPtr Arg, OrbisKeyboardEvent Event);

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
