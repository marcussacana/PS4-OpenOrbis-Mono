using System.ComponentModel;

namespace OrbisGL
{
    public static class Constants
    {
        public const int SCE_SECOND =  1000000;
        public const int SCE_MILISECOND = 1000;

        public const uint SCE_MOUSE_BUTTON_PRIMARY = 0x00000001;
        public const uint SCE_MOUSE_BUTTON_SECONDARY = 0x00000002;
        public const uint SCE_MOUSE_BUTTON_OPTIONAL = 0x00000004;
        public const uint SCE_MOUSE_BUTTON_INTERCEPTED = 0x80000000;
        public const byte SCE_MOUSE_OPEN_PARAM_MERGED = 0x01;
        public const byte SCE_MOUSE_PORT_TYPE_STANDARD = 0;
        public const int  SCE_OK = 0;
        


        public const int SCE_IME_KEYBOARD_MAX_NUMBER = 5;

        public const int SCE_USER_SERVICE_USER_ID_ALL_USERS = 0x000000FE;
        public const int SCE_SYSTEM_SERVICE_PARAM_ID_LANG = 0x00000001;

        public const int MaxTouchNum = 2;
        public const int MaxDataNum = 0x40;
    }
}