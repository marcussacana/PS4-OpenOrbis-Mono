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

        public const uint ORBIS_KERNEL_PRIO_FIFO_LOWEST  = 0x2FF;
        public const uint ORBIS_KERNEL_PRIO_FIFO_NORMAL  = 0x2BC;
        public const uint ORBIS_KERNEL_PRIO_FIFO_HIGHEST = 0x100;

        public const int MaxTouchNum = 2;
        public const int MaxDataNum = 0x40;
    }
}