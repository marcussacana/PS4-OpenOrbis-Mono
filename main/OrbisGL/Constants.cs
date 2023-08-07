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
        public const int ORBIS_PAD_MAX_TOUCH_NUM = 2;
        public const int ORBIS_PAD_MAX_DATA_NUM = 0x40;

        public const int SCE_AUDIO_OUT_PARAM_FORMAT_S16_MONO = 0;
        public const int SCE_AUDIO_OUT_PARAM_FORMAT_S16_STEREO = 1;
        public const int SCE_AUDIO_OUT_PARAM_FORMAT_S16_8CH = 2;
        public const int SCE_AUDIO_OUT_PARAM_FORMAT_FLOAT_MONO = 3;
        public const int SCE_AUDIO_OUT_PARAM_FORMAT_FLOAT_STEREO = 4;
        public const int SCE_AUDIO_OUT_PARAM_FORMAT_FLOAT_8CH = 5;

        public const int SCE_AUDIO_OUT_PARAM_FORMAT_S16_8CH_STD = 6;
        public const int SCE_AUDIO_OUT_PARAM_FORMAT_FLOAT_8CH_STD = 7;


        public const int SCE_IME_MAX_TEXT_LENGTH = 2048;
        public const int SCE_IME_KEYBOARD_MAX_NUMBER = 5;

        public const int SCE_USER_SERVICE_USER_ID_ALL_USERS = 0x000000FE;
        public const int SCE_SYSTEM_SERVICE_PARAM_ID_LANG = 0x00000001;

        public const int MaxTouchNum = 2;
        public const int MaxTouchX = 1919;
        public const int MaxTouchY = 941;

        public const int MaxDataNum = 0x40;
    }
}