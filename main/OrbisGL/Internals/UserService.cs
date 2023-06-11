using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Orbis.Internals
{
    public unsafe class UserService
    {
        private static bool Initialized = false;
        public static int Initialize()
        {
            if (Initialized)
                return 0;
            
            Initialized = true;
            UserServiceInitializeParams Params = new UserServiceInitializeParams();
            Params.priority = ORBIS_KERNEL_PRIO_FIFO_NORMAL;
            return Initialize(&Params);
        }

        public static int GetInitialUser(out int UserID)
        {
            int rst = 0;
            int err = GetInitialUser(&rst);
            UserID = rst;
            return err;
        }
        
        public static int GetForegroundUser(out int UserID)
        {
            int rst = 0;
            int err = GetForegroundUser(&rst);
            UserID = rst;
            return err;
        }
        

        [MethodImpl(MethodImplOptions.InternalCall)]
        static extern int Initialize(void* Params);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int Terminate();
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int GetForegroundUser(int* UserID);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int GetInitialUser(int* UserID);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int HideSplashScreen();


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct UserServiceInitializeParams
        {
            public uint priority;
        }

        public const uint ORBIS_KERNEL_PRIO_FIFO_LOWEST  = 0x2FF;
        public const uint ORBIS_KERNEL_PRIO_FIFO_NORMAL  = 0x2BC;
        public const uint ORBIS_KERNEL_PRIO_FIFO_HIGHEST = 0x100;

        public static int LoadExec(string Path, params string[] Args)
        {
            //Not sure how to generate the pArgs, no documentation at all
            //therefore this code is just a guess

            if (Args.Length > 0)
            {
                void*[] pArgs = new void*[Args.Length + 1];

                for (int i = 0; i < Args.Length; i++)
                    pArgs[i] = Kernel.AllocString(Args[i]);

                pArgs[Args.Length] = null;

                fixed (void* ppArgs = &pArgs[0])
                {
                    var pStr = Kernel.AllocString(Path);
                    var Rst = NativeLoadExec(pStr, ppArgs);
                    Kernel.free(pStr);
                    foreach (var Arg in pArgs)
                    {
                        Kernel.free(Arg);
                    }
                    return Rst;
                }
            }

            var pPath = Kernel.AllocString(Path);
            var Result = NativeLoadExec(pPath, null);
            Kernel.free(pPath);
            return Result;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        static extern int NativeLoadExec(void* Path, void* Args);
    }
}
