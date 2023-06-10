using System.Runtime.InteropServices;

namespace OrbisGL.Native
{
    public static class UserService
    {
        static bool Initialized = false;
        public static void Initialize()
        {
            OrbisUserServiceInitializeParams InitializeParams = new OrbisUserServiceInitializeParams();
            InitializeParams.priority = Constants.ORBIS_KERNEL_PRIO_FIFO_NORMAL;
            sceUserServiceInitialize(InitializeParams);
            Initialized = true;
        }

        public static int GetForegroundUser()
        {
            if (!Initialized)
                Initialize();

            sceUserServiceGetForegroundUser(out int UserID);
            return UserID;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct OrbisUserServiceInitializeParams
        {
            public uint priority;
        }

        [DllImport("libSceUserService.sprx")]
        static extern int sceUserServiceInitialize(OrbisUserServiceInitializeParams Params);


        [DllImport("libSceUserService.sprx")]
        static extern int sceUserServiceGetForegroundUser(out int UserID);
    }
}
