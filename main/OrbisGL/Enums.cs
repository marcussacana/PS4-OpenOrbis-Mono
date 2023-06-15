using System;
using SharpGLES;

namespace OrbisGL
{
    public enum RenderMode : int
    {
        Triangle = GLES20.GL_TRIANGLES,
        MultipleLines = GLES20.GL_LINES,
        SingleLine = GLES20.GL_LINE_STRIP,
        ClosedLine = GLES20.GL_LINE_LOOP
    }

    public enum AttributeType : int
    {
        Int = GLES20.GL_INT,
        UInt = GLES20.GL_UNSIGNED_INT,
        SByte = GLES20.GL_UNSIGNED_BYTE,
        Byte = GLES20.GL_BYTE,
        Short = GLES20.GL_SHORT,
        UShort = GLES20.GL_UNSIGNED_SHORT,
        Float = GLES20.GL_FLOAT,
        Double = GLES20.GL_HIGH_FLOAT
    }

    public enum AttributeSize : int
    {
        None = 1 << 0,
        FloatMatrix2 = 1 << 1,
        FloatMatrix3 = 1 << 2,
        FloatMatrix4 = 1 << 3,
        Vector2 = 1 << 4,
        Vector3 = 1 << 5,
        Vector4 = 1 << 6
    }

    public enum PixelFormat : int
    {
        RGBA = GLES20.GL_RGBA,
        RGB = GLES20.GL_RGB
    }

    [Flags]
    public enum MouseButtons
    {
        Right = 1 << 0,
        Left = 1 << 1,
        Middle = 1 << 2,
        All = Right | Left | Middle
    }

    public enum OrbisPadPortType
    {
        Standard = 0,
        Special = 2
    }

    public enum OrbisPadDeviceClass
    {
        Pad = 0,
        Guitar = 1,
        Drums = 2
    }

    public enum OrbisPadConnectionType
    {
        Standard = 0,
        Remote = 2
    }

    [Flags]
    public enum OrbisPadButton
    {
        L3 = 0x0002,
        R3 = 0x0004,
        Options = 0x0008,
        Up = 0x0010,
        Right = 0x0020,
        Down = 0x0040,
        Left = 0x0080,
        L2 = 0x0100,
        R2 = 0x0200,
        L1 = 0x0400,
        R1 = 0x0800,
        Triangle = 0x1000,
        Circle = 0x2000,
        Cross = 0x4000,
        Square = 0x8000,
        TouchPad = 0x100000
    }


    #region IME

    //Stolen from fpPS4 project: https://github.com/red-prig/fpPS4/blob/d0fd45eb63f775f52cefebff437bb010bb2ba747/src/ps4_libsceime.pas
    public enum IME_Error : int
    {
        BUSY = -2135162879,
        NOT_OPENED = -2135162878,
        NO_MEMORY = -2135162877,
        CONNECTION_FAILED = -2135162876,
        TOO_MANY_REQUESTS = -2135162875,
        INVALID_TEXT = -2135162874,
        OVERFLOW = -2135162873,
        NOT_ACTIVE = -2135162872,
        IME_SUSPENDING = -2135162871,
        DEVICE_IN_USE = -2135162870,
        INVALID_USER_ID = -2135162864,
        INVALID_TYPE = -2135162863,
        INVALID_SUPPORTED_LANGUAGES = -2135162862,
        INVALID_ENTER_LABEL = -2135162861,
        INVALID_INPUT_METHOD = -2135162860,
        INVALID_OPTION = -2135162859,
        INVALID_MAX_TEXT_LENGTH = -2135162858,
        INVALID_INPUT_TEXT_BUFFER = -2135162857,
        INVALID_POSX = -2135162856,
        INVALID_POSY = -2135162855,
        INVALID_HORIZONTAL_ALIGNMENT = -2135162854,
        INVALID_VERTICAL_ALIGNMENT = -2135162853,
        INVALID_EXTENDED = -2135162852,
        INVALID_KEYBOARD_TYPE = -2135162851,
        INVALID_WORK = -2135162848,
        INVALID_ARG = -2135162847,
        INVALID_HANDLER = -2135162846,
        NO_RESOURCE_ID = -2135162845,
        INVALID_MODE = -2135162844,
        INVALID_PARAM = -2135162832,
        INVALID_ADDRESS = -2135162831,
        INVALID_RESERVED = -2135162830,
        INVALID_TIMING = -2135162829,
        INTERNAL = -2135162625,
    }
    public enum IME_KeyCode : ushort
    {
        NOEVENT = 0x0000,
        ERRORROLLOVER = 0x0001,
        POSTFAIL = 0x0002,
        ERRORUNDEFINED = 0x0003,
        A = 0x0004,
        B = 0x0005,
        C = 0x0006,
        D = 0x0007,
        E = 0x0008,
        F = 0x0009,
        G = 0x000A,
        H = 0x000B,
        I = 0x000C,
        J = 0x000D,
        K = 0x000E,
        L = 0x000F,
        M = 0x0010,
        N = 0x0011,
        O = 0x0012,
        P = 0x0013,
        Q = 0x0014,
        R = 0x0015,
        S = 0x0016,
        T = 0x0017,
        U = 0x0018,
        V = 0x0019,
        W = 0x001A,
        X = 0x001B,
        Y = 0x001C,
        Z = 0x001D,
        N1 = 0x001E,
        N2 = 0x001F,
        N3 = 0x0020,
        N4 = 0x0021,
        N5 = 0x0022,
        N6 = 0x0023,
        N7 = 0x0024,
        N8 = 0x0025,
        N9 = 0x0026,
        N0 = 0x0027,
        RETURN = 0x0028,
        ESCAPE = 0x0029,
        BACKSPACE = 0x002A,
        TAB = 0x002B,
        SPACEBAR = 0x002C,
        MINUS = 0x002D,
        EQUAL = 0x002E,
        LEFTBRACKET = 0x002F,
        RIGHTBRACKET = 0x0030,
        BACKSLASH = 0x0031,
        NONUS_POUND = 0x0032,
        SEMICOLON = 0x0033,
        SINGLEQUOTE = 0x0034,
        BACKQUOTE = 0x0035,
        COMMA = 0x0036,
        PERIOD = 0x0037,
        SLASH = 0x0038,
        CAPSLOCK = 0x0039,
        F1 = 0x003A,
        F2 = 0x003B,
        F3 = 0x003C,
        F4 = 0x003D,
        F5 = 0x003E,
        F6 = 0x003F,
        F7 = 0x0040,
        F8 = 0x0041,
        F9 = 0x0042,
        F10 = 0x0043,
        F11 = 0x0044,
        F12 = 0x0045,
        PRINTSCREEN = 0x0046,
        SCROLLLOCK = 0x0047,
        PAUSE = 0x0048,
        INSERT = 0x0049,
        HOME = 0x004A,
        PAGEUP = 0x004B,
        DELETE = 0x004C,
        END = 0x004D,
        PAGEDOWN = 0x004E,
        RIGHTARROW = 0x004F,
        LEFTARROW = 0x0050,
        DOWNARROW = 0x0051,
        UPARROW = 0x0052,
        KEYPAD_NUMLOCK = 0x0053,
        KEYPAD_SLASH = 0x0054,
        KEYPAD_ASTERISK = 0x0055,
        KEYPAD_MINUS = 0x0056,
        KEYPAD_PLUS = 0x0057,
        KEYPAD_ENTER = 0x0058,
        KEYPAD_1 = 0x0059,
        KEYPAD_2 = 0x005A,
        KEYPAD_3 = 0x005B,
        KEYPAD_4 = 0x005C,
        KEYPAD_5 = 0x005D,
        KEYPAD_6 = 0x005E,
        KEYPAD_7 = 0x005F,
        KEYPAD_8 = 0x0060,
        KEYPAD_9 = 0x0061,
        KEYPAD_0 = 0x0062,
        KEYPAD_PERIOD = 0x0063,
        NONUS_BACKSLASH = 0x0064,
        APPLICATION = 0x0065,
        POWER = 0x0066,
        KEYPAD_EQUAL = 0x0067,
        F13 = 0x0068,
        F14 = 0x0069,
        F15 = 0x006A,
        F16 = 0x006B,
        F17 = 0x006C,
        F18 = 0x006D,
        F19 = 0x006E,
        F20 = 0x006F,
        F21 = 0x0070,
        F22 = 0x0071,
        F23 = 0x0072,
        F24 = 0x0073,
        EXECUTE = 0x0074,
        HELP = 0x0075,
        MENU = 0x0076,
        SELECT = 0x0077,
        STOP = 0x0078,
        AGAIN = 0x0079,
        UNDO = 0x007A,
        CUT = 0x007B,
        COPY = 0x007C,
        PASTE = 0x007D,
        FIND = 0x007E,
        MUTE = 0x007F,
        VOLUMEUP = 0x0080,
        VOLUMEDOWN = 0x0081,
        LOCKING_CAPSLOCK = 0x0082,
        LOCKING_NUMLOCK = 0x0083,
        LOCKING_SCROLLLOCK = 0x0084,
        KEYPAD_COMMA = 0x0085,
        KEYPAD_EQUALSIGN = 0x0086,
        INTERNATIONAL1 = 0x0087,
        INTERNATIONAL2 = 0x0088,
        INTERNATIONAL3 = 0x0089,
        INTERNATIONAL4 = 0x008A,
        INTERNATIONAL5 = 0x008B,
        INTERNATIONAL6 = 0x008C,
        INTERNATIONAL7 = 0x008D,
        INTERNATIONAL8 = 0x008E,
        INTERNATIONAL9 = 0x008F,
        LANG1 = 0x0090,
        LANG2 = 0x0091,
        LANG3 = 0x0092,
        LANG4 = 0x0093,
        LANG5 = 0x0094,
        LANG6 = 0x0095,
        LANG7 = 0x0096,
        LANG8 = 0x0097,
        LANG9 = 0x0098,
        ALTERASE = 0x0099,
        SYSREQ = 0x009A,
        CANCEL = 0x009B,
        CLEAR = 0x009C,
        PRIOR = 0x009D,
        RETURN2 = 0x009E,
        SEPARATOR = 0x009F,
        OUT = 0x00A0,
        OPER = 0x00A1,
        CLEAR_AGAIN = 0x00A2,
        CRSEL_PROPS = 0x00A3,
        EXSEL = 0x00A4,
        KEYPAD_00 = 0x00B0,
        KEYPAD_000 = 0x00B1,
        THOUSANDSSEPARATOR = 0x00B2,
        DECIMALSEPARATOR = 0x00B3,
        CURRENCYUNIT = 0x00B4,
        CURRENCYSUBUNIT = 0x00B5,
        KEYPAD_LEFTPARENTHESIS = 0x00B6,
        KEYPAD_RIGHTPARENTHESIS = 0x00B7,
        KEYPAD_LEFTCURLYBRACKET = 0x00B8,
        KEYPAD_RIGHTCURLYBRACKET = 0x00B9,
        KEYPAD_TAB = 0x00BA,
        KEYPAD_BACKSPACE = 0x00BB,
        KEYPAD_A = 0x00BC,
        KEYPAD_B = 0x00BD,
        KEYPAD_C = 0x00BE,
        KEYPAD_D = 0x00BF,
        KEYPAD_E = 0x00C0,
        KEYPAD_F = 0x00C1,
        KEYPAD_XOR = 0x00C2,
        KEYPAD_HAT = 0x00C3,
        KEYPAD_PERCENT = 0x00C4,
        KEYPAD_LESSTHAN = 0x00C5,
        KEYPAD_GREATERTHAN = 0x00C6,
        KEYPAD_AND = 0x00C7,
        KEYPAD_LOGICALAND = 0x00C8,
        KEYPAD_OR = 0x00C9,
        KEYPAD_LOGICALOR = 0x00CA,
        KEYPAD_COLON = 0x00CB,
        KEYPAD_NUMBER = 0x00CC,
        KEYPAD_SPACE = 0x00CD,
        KEYPAD_ATSIGN = 0x00CE,
        KEYPAD_EXCLAMATION = 0x00CF,
        KEYPAD_MEMORY_STORE = 0x00D0,
        KEYPAD_MEMORY_RECALL = 0x00D1,
        KEYPAD_MEMORY_CLEAR = 0x00D2,
        KEYPAD_MEMORY_ADD = 0x00D3,
        KEYPAD_MEMORY_SUBTRACT = 0x00D4,
        KEYPAD_MEMORY_MULTIPLY = 0x00D5,
        KEYPAD_MEMORY_DIVIDE = 0x00D6,
        KEYPAD_PLUS_MINUS = 0x00D7,
        KEYPAD_CLEAR = 0x00D8,
        KEYPAD_CLEARENTRY = 0x00D9,
        KEYPAD_BINARY = 0x00DA,
        KEYPAD_OCTAL = 0x00DB,
        KEYPAD_DECIMAL = 0x00DC,
        KEYPAD_HEXADECIMAL = 0x00DD,
        LEFTCONTROL = 0x00E0,
        LEFTSHIFT = 0x00E1,
        LEFTALT = 0x00E2,
        LEFTGUI = 0x00E3,
        RIGHTCONTROL = 0x00E4,
        RIGHTSHIFT = 0x00E5,
        RIGHTALT = 0x00E6,
        RIGHTGUI = 0x00E7,
    }

    public enum IME_Max : int
    {
        PREEDIT_LENGTH = 30,
        EXPANDED_PREEDIT_LENGTH = 120,
        TEXT_LENGTH = 2048,
        TEXT_AREA = 4,
        CANDIDATE_WORD_LENGTH = 55,
        CANDIDATE_LIST_SIZE = 100,
    }

    public enum IME_OSK : int
    {
        DISPLAY_SIZE_WIDTH = 1920,
        DISPLAY_SIZE_HEIGHT = 1080,
        OVER_2K_DISPLAY_SIZE_WIDTH = 3840,
        OVER_2K_DISPLAY_SIZE_HEIGHT = 2160,
    }

    public enum IME_Language : long
    {
        DANISH = 0x0000000000000001,
        GERMAN = 0x0000000000000002,
        ENGLISH_US = 0x0000000000000004,
        SPANISH = 0x0000000000000008,
        FRENCH = 0x0000000000000010,
        ITALIAN = 0x0000000000000020,
        DUTCH = 0x0000000000000040,
        NORWEGIAN = 0x0000000000000080,
        POLISH = 0x0000000000000100,
        PORTUGUESE_PT = 0x0000000000000200,
        RUSSIAN = 0x0000000000000400,
        FINNISH = 0x0000000000000800,
        SWEDISH = 0x0000000000001000,
        JAPANESE = 0x0000000000002000,
        KOREAN = 0x0000000000004000,
        SIMPLIFIED_CHINESE = 0x0000000000008000,
        TRADITIONAL_CHINESE = 0x0000000000010000,
        PORTUGUESE_BR = 0x0000000000020000,
        ENGLISH_GB = 0x0000000000040000,
        TURKISH = 0x0000000000080000,
        SPANISH_LA = 0x0000000000100000,
        ARABIC = 0x0000000001000000,
        FRENCH_CA = 0x0000000002000000,
        THAI = 0x0000000004000000,
        CZECH = 0x0000000008000000,
        GREEK = 0x0000000010000000,
        INDONESIAN = 0x0000000020000000,
        VIETNAMESE = 0x0000000040000000,
        ROMANIAN = 0x0000000080000000,
        HUNGARIAN = 0x0000000100000000
    }
    public enum IME_Option : int
    {
        DEFAULT = 0x00000000,
        MULTILINE = 0x00000001,
        NO_AUTO_CAPITALIZATION = 0x00000002,
        PASSWORD = 0x00000004,
        LANGUAGES_FORCED = 0x00000008,
        EXT_KEYBOARD = 0x00000010,
        NO_LEARNING = 0x00000020,
        FIXED_POSITION = 0x00000040,
        DISABLE_COPY_PASTE = 0x00000080,
        DISABLE_RESUME = 0x00000100,
        DISABLE_AUTO_SPACE = 0x00000200,
        DISABLE_POSITION_ADJUSTMENT = 0x00000800,
        EXPANDED_PREEDIT_BUFFER = 0x00001000,
        USE_JAPANESE_EISUU_KEY_AS_CAPSLOCK = 0x00002000,
        USE_OVER_2K_COORDINATES = 0x00004000,
    }

    public enum IME_ExtraOption : int
    {
        DEFAULT = 0x00000000,
        SET_COLOR = 0x00000001,
        SET_PRIORITY = 0x00000002,
        PRIORITY_SHIFT = 0x00000004,
        PRIORITY_FULL_WIDTH = 0x00000008,
        PRIORITY_FIXED_PANEL = 0x00000010,
        DISABLE_POINTER = 0x00000040,
        ENABLE_ADDITIONAL_DICTIONARY = 0x00000080,
        DISABLE_STARTUP_SE = 0x00000100,
        DISABLE_LIST_FOR_EXT_KEYBOARD = 0x00000200,
        HIDE_KEYPANEL_IF_EXT_KEYBOARD = 0x00000400,
        INIT_EXT_KEYBOARD_MODE = 0x00000800,

        DIALOG_EXT_OPTION_ENABLE_ACCESSIBILITY = 0x00001000,
        DIALOG_EXT_OPTION_ACCESSIBILITY_PANEL_FORCED = 0x00002000,
        ADDITIONAL_DICTIONARY_PRIORITY_MODE = 0x00004000,
    }

    public enum IME_DisableDevice : int
    {
        DEFAULT = 0x00000000,
        CONTROLLER = 0x00000001,
        EXT_KEYBOARD = 0x00000002,
        REMOTE_OSK = 0x00000004,
    }

    public enum IME_InputMethodState : int
    {
        PREEDIT = 0x01000000,
        SELECTED = 0x02000000,
        NATIVE = 0x04000000,
        NATIVE2 = 0x08000000,
        FULL_WIDTH = 0x10000000,
    }

    public enum IME_InitExt : int
    {
        INIT_EXT_KEYBOARD_MODE_DISABLE_ARABIC_INDIC_NUMERALS = 0x00000001,
        INIT_EXT_KEYBOARD_MODE_ENABLE_FORMAT_CHARACTERS = 0x00000002,
        INIT_EXT_KEYBOARD_MODE_INPUT_METHOD_STATE_NATIVE = IME_InputMethodState.NATIVE,
        INIT_EXT_KEYBOARD_MODE_INPUT_METHOD_STATE_NATIVE2 = IME_InputMethodState.NATIVE2,
        INIT_EXT_KEYBOARD_MODE_INPUT_METHOD_STATE_FULL_WIDTH = IME_InputMethodState.FULL_WIDTH,
    }

    public enum IME_Event : int
    {
        OPEN = 0,
        UPDATE_TEXT = 1,
        UPDATE_CARET = 2,
        CHANGE_SIZE = 3,
        PRESS_CLOSE = 4,
        PRESS_ENTER = 5,
        ABORT = 6,
        CANDIDATE_LIST_START = 7,
        CANDIDATE_LIST_END = 8,
        CANDIDATE_WORD = 9,
        CANDIDATE_INDEX = 10,
        CANDIDATE_DONE = 11,
        CANDIDATE_CANCEL = 12,
        CHANGE_DEVICE = 14,
        JUMP_TO_NEXT_OBJECT = 15,
        JUMP_TO_BEFORE_OBJECT = 16,
        CHANGE_WINDOW_TYPE = 17,
        CHANGE_INPUT_METHOD_STATE = 18,
    }

    public enum IME_KeyboardEvent : long
    {
        OPEN = 256,
        KEYCODE_DOWN = 257,
        KEYCODE_UP = 258,
        KEYCODE_REPEAT = 259,
        CONNECTION = 260,
        DISCONNECTION = 261,
        ABORT = 262,
    }

    public enum IME_Align : int
    {
        HORIZONTAL_LEFT = 0,
        HORIZONTAL_CENTER = 1,
        HORIZONTAL_RIGHT = 2,
        VERTICAL_TOP = 0,
        VERTICAL_CENTER = 1,
        VERTICAL_BOTTOM = 2,
    }

    public enum IME_EnterLabel : int
    {
        DEFAULT = 0,
        SEND = 1,
        SEARCH = 2,
        GO = 3,
    }

    public enum IME_Type : int
    {
        DEFAULT = 0,
        BASIC_LATIN = 1,
        URL = 2,
        MAIL = 3,
        NUMBER = 4,
    }

    public enum IME_PanelPriority : int
    {
        DEFAULT = 0,
        ALPHABET = 1,
        SYMBOL = 2,
        ACCENT = 3,
    }

    public enum IME_InputMethod : int
    {
        DEFAULT = 0,
    }

    public enum IME_CarretMove : int
    {
        STILL = 0,
        LEFT = 1,
        RIGHT = 2,
        UP = 3,
        DOWN = 4,
        HOME = 5,
        END = 6,
        PAGE_UP = 7,
        PAGE_DOWN = 8,
        TOP = 9,
        BOTTOM = 10,
    }

    public enum IME_TextAreaMode : int
    {
        DISABLE = 0,
        EDIT = 1,
        PREEDIT = 2,
        SELECT = 3,
    }

    public enum IME_KeycodeState : int
    {
        VALID = 0x00000001,
        CHARACTER_VALID = 0x00000002,
        WITH_IME = 0x00000004,
        FROM_OSK = 0x00000008,
        FROM_OSK_SHORTCUT = 0x00000010,
        FROM_IME_OPERATION = 0x00000020,
        REPLACE_CHARACTER = 0x00000040,
        CONTINUOUS_EVENT = 0x00000080,
        MODIFIER_L_CTRL = 0x00000100,
        MODIFIER_L_SHIFT = 0x00000200,
        MODIFIER_L_ALT = 0x00000400,
        MODIFIER_L_GUI = 0x00000800,
        MODIFIER_R_CTRL = 0x00001000,
        MODIFIER_R_SHIFT = 0x00002000,
        MODIFIER_R_ALT = 0x00004000,
        MODIFIER_R_GUI = 0x00008000,
        LED_NUM_LOCK = 0x00010000,
        LED_CAPS_LOCK = 0x00020000,
        LED_SCROLL_LOCK = 0x00040000,
        RESERVED1 = 0x00080000,
        RESERVED2 = 0x00100000,
        FROM_IME_INPUT = 0x00200000,
    }

    public enum IME_KeyboardOption : int
    {
        DEFAULT = 0x00000000,
        REPEAT = 0x00000001,
        REPEAT_EACH_KEY = 0x00000002,
        ADD_OSK = 0x00000004,
        EFFECTIVE_WITH_IME = 0x00000008,
        DISABLE_RESUME = 0x00000010,
        DISABLE_CAPSLOCK_WITHOUT_SHIFT = 0x00000020,
    }

    public enum IME_KeyboardMode : int
    {
        AUTO = 0x00000000,
        MANUAL = 0x00000001,
        ALPHABET = 0x00000000,
        NATIVE = 0x00000002,
        PART = 0x00000004,
        KATAKANA = 0x00000008,
        HKANA = 0x00000010,
        ARABIC_INDIC_NUMERALS = 0x00000020,
        DISABLE_FORMAT_CHARACTERS = 0x00000040,
    }

    public enum IME_KeyboardResource : int
    {
        ID_INVALID = 0x00000000,
        ID_OSK = 0x00000001,
    }

    public enum IME_KeyboardState : int
    {
        DISCONNECTED = 0,
        CONNECTED = 1,
    }
    public enum IME_KeyboardType : int
    {
        NONE = 0,
        DANISH = 1,
        GERMAN = 2,
        GERMAN_SW = 3,
        ENGLISH_US = 4,
        ENGLISH_GB = 5,
        SPANISH = 6,
        SPANISH_LA = 7,
        FINNISH = 8,
        FRENCH = 9,
        FRENCH_BR = 10,
        FRENCH_CA = 11,
        FRENCH_SW = 12,
        ITALIAN = 13,
        DUTCH = 14,
        NORWEGIAN = 15,
        POLISH = 16,
        PORTUGUESE_BR = 17,
        PORTUGUESE_PT = 18,
        RUSSIAN = 19,
        SWEDISH = 20,
        TURKISH = 21,
        JAPANESE_ROMAN = 22,
        JAPANESE_KANA = 23,
        KOREAN = 24,
        SM_CHINESE = 25,
        TR_CHINESE_ZY = 26,
        TR_CHINESE_PY_HK = 27,
        TR_CHINESE_PY_TW = 28,
        TR_CHINESE_CG = 29,
        ARABIC_AR = 30,
        THAI = 31,
        CZECH = 32,
        GREEK = 33,
        INDONESIAN = 34,
        VIETNAMESE = 35,
        ROMANIAN = 36,
        HUNGARIAN = 37,
    }

    public enum IME_KeyboardDeviceType : int
    {
        KEYBOARD = 0,
        OSK = 1,
    }

    public enum IME_KeyboardPanelType : int
    {
        HIDE = 0,
        OSK = 1,
        DIALOG = 2,
        CANDIDATE = 3,
        EDIT = 4,
        EDIT_AND_CANDIDATE = 5,
        ACCESSIBILITY = 6,
    }

    public enum IME_DeviceType : int
    {
        NONE = 0,
        CONTROLLER = 1,
        EXT_KEYBOARD = 2,
        REMOTE_OSK = 3
    }

    #endregion
}