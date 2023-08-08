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

        public const int SCE_AUDIO_OUT_PORT_TYPE_MAIN = 0;
        public const int SCE_AUDIO_OUT_PORT_TYPE_BGM = 1;
        public const int SCE_AUDIO_OUT_PORT_TYPE_VOICE = 2;
        public const int SCE_AUDIO_OUT_PORT_TYPE_PERSONAL = 3;
        public const int SCE_AUDIO_OUT_PORT_TYPE_PADSPK = 4;
        public const int SCE_AUDIO_OUT_PORT_TYPE_AUX = 127;

        public const int ORBIS_AUDIO_VOLUME_SHIFT = 15;
        public const int ORBIS_AUDIO_OUT_VOLUME_SHIFT = ORBIS_AUDIO_VOLUME_SHIFT;
        public const int ORBIS_AUDIO_VOLUME_0DB = 1 << ORBIS_AUDIO_VOLUME_SHIFT;
        public const int ORBIS_AUDIO_OUT_VOLUME_0DB = ORBIS_AUDIO_VOLUME_0DB;

        public const int ORBIS_AUDIO_VOLUME_FLAG_L_CH = 1 << 0;
        public const int ORBIS_AUDIO_VOLUME_FLAG_FL_CH = ORBIS_AUDIO_VOLUME_FLAG_L_CH;
        public const int ORBIS_AUDIO_OUT_VOLUME_FLAG_L_CH = ORBIS_AUDIO_VOLUME_FLAG_L_CH;
        public const int ORBIS_AUDIO_OUT_VOLUME_FLAG_FL_CH = ORBIS_AUDIO_VOLUME_FLAG_L_CH;

        public const int ORBIS_AUDIO_VOLUME_FLAG_R_CH = 1 << 1;
        public const int ORBIS_AUDIO_VOLUME_FLAG_FR_CH = ORBIS_AUDIO_VOLUME_FLAG_R_CH;
        public const int ORBIS_AUDIO_OUT_VOLUME_FLAG_R_CH = ORBIS_AUDIO_VOLUME_FLAG_R_CH;
        public const int ORBIS_AUDIO_OUT_VOLUME_FLAG_FR_CH = ORBIS_AUDIO_VOLUME_FLAG_R_CH;

        public const int ORBIS_AUDIO_VOLUME_FLAG_C_CH = 1 << 2;
        public const int ORBIS_AUDIO_VOLUME_FLAG_FC_CH = ORBIS_AUDIO_VOLUME_FLAG_C_CH;
        public const int ORBIS_AUDIO_VOLUME_FLAG_CNT_CH = ORBIS_AUDIO_VOLUME_FLAG_C_CH;
        public const int ORBIS_AUDIO_OUT_VOLUME_FLAG_C_CH = ORBIS_AUDIO_VOLUME_FLAG_C_CH;
        public const int ORBIS_AUDIO_OUT_VOLUME_FLAG_FC_CH = ORBIS_AUDIO_VOLUME_FLAG_C_CH;
        public const int ORBIS_AUDIO_OUT_VOLUME_FLAG_CNT_CH = ORBIS_AUDIO_VOLUME_FLAG_C_CH;

        public const int ORBIS_AUDIO_VOLUME_FLAG_LFE_CH = 1 << 3;
        public const int ORBIS_AUDIO_OUT_VOLUME_FLAG_LFE_CH = ORBIS_AUDIO_VOLUME_FLAG_LFE_CH;

        public const int ORBIS_AUDIO_VOLUME_FLAG_LS_CH = 1 << 4;
        public const int ORBIS_AUDIO_VOLUME_FLAG_RL_CH = ORBIS_AUDIO_VOLUME_FLAG_LS_CH;
        public const int ORBIS_AUDIO_OUT_VOLUME_FLAG_LS_CH = ORBIS_AUDIO_VOLUME_FLAG_LS_CH;
        public const int ORBIS_AUDIO_OUT_VOLUME_FLAG_RL_CH = ORBIS_AUDIO_VOLUME_FLAG_LS_CH;

        public const int ORBIS_AUDIO_VOLUME_FLAG_RS_CH = 1 << 5;
        public const int ORBIS_AUDIO_VOLUME_FLAG_RR_CH = ORBIS_AUDIO_VOLUME_FLAG_RS_CH;
        public const int ORBIS_AUDIO_OUT_VOLUME_FLAG_RS_CH = ORBIS_AUDIO_VOLUME_FLAG_RS_CH;
        public const int ORBIS_AUDIO_OUT_VOLUME_FLAG_RR_CH = ORBIS_AUDIO_VOLUME_FLAG_RS_CH;

        public const int ORBIS_AUDIO_VOLUME_FLAG_LE_CH = 1 << 6;
        public const int ORBIS_AUDIO_VOLUME_FLAG_BL_CH = ORBIS_AUDIO_VOLUME_FLAG_LE_CH;
        public const int ORBIS_AUDIO_OUT_VOLUME_FLAG_LE_CH = ORBIS_AUDIO_VOLUME_FLAG_LE_CH;
        public const int ORBIS_AUDIO_OUT_VOLUME_FLAG_BL_CH = ORBIS_AUDIO_VOLUME_FLAG_LE_CH;

        public const int ORBIS_AUDIO_VOLUME_FLAG_RE_CH = 1 << 7;
        public const int ORBIS_AUDIO_VOLUME_FLAG_BR_CH = ORBIS_AUDIO_VOLUME_FLAG_RE_CH;
        public const int ORBIS_AUDIO_OUT_VOLUME_FLAG_RE_CH = ORBIS_AUDIO_VOLUME_FLAG_RE_CH;
        public const int ORBIS_AUDIO_OUT_VOLUME_FLAG_BR_CH = ORBIS_AUDIO_VOLUME_FLAG_RE_CH;

        public const int ORBIS_AUDIO_VOLUME_FLAG_ALL = ORBIS_AUDIO_VOLUME_FLAG_FL_CH | ORBIS_AUDIO_VOLUME_FLAG_FR_CH
                                  | ORBIS_AUDIO_VOLUME_FLAG_CNT_CH | ORBIS_AUDIO_VOLUME_FLAG_LFE_CH
                                  | ORBIS_AUDIO_VOLUME_FLAG_RL_CH | ORBIS_AUDIO_VOLUME_FLAG_RR_CH
                                  | ORBIS_AUDIO_VOLUME_FLAG_BL_CH | ORBIS_AUDIO_VOLUME_FLAG_BR_CH;


        public const int SCE_IME_MAX_TEXT_LENGTH = 2048;
        public const int SCE_IME_KEYBOARD_MAX_NUMBER = 5;

        public const int SCE_USER_SERVICE_USER_ID_ALL_USERS = 0x000000FE;
        public const int SCE_USER_SERVICE_USER_ID_SYSTEM = 0x000000FF;
        public const int SCE_SYSTEM_SERVICE_PARAM_ID_LANG = 0x00000001;

        public const int MaxTouchNum = 2;
        public const int MaxTouchX = 1919;
        public const int MaxTouchY = 941;

        public const int MaxDataNum = 0x40;
    }
}