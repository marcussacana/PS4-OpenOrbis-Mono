namespace OrbisGL
{
    public static class Constants
    {
#if ORBIS
        public const int SCE_SECOND =  1000000;
        public const int SCE_MILISECOND = 10000;
#else
        public const int SCE_SECOND = 100000;
        public const int SCE_MILISECOND = 1000;

#endif

        public const int MaxTouchNum = 2;
        public const int MaxDataNum = 0x40;
    }
}