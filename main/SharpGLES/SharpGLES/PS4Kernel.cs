using System.Runtime.CompilerServices;
using System.Text;

namespace Orbis.Internals
{
    public unsafe class Kernel
    {
        public static bool Jailbreak(long AuthID = 0)
        {
            return JailbreakCred(AuthID);
        }

        public static int LoadStartModule(string Path, out int Status)
        {
            var pPath = AllocString(Path);

            int LoadStatus = 0;
            var Result = LoadStartModule(pPath, null, null, 0, null, &LoadStatus);
            
            Status = LoadStatus;
            
            free(pPath);
            
            return Result;
        }

        public static bool GetModuleBase(string Name, out long BaseAddress, out long ModuleSize)
        {
            var pName = AllocString(Name);
            
            long bAddr = 0;
            long mSize = 0;
            
            var Success = GetModuleBase(pName, &bAddr, &mSize);

            BaseAddress = bAddr;
            ModuleSize = mSize;

            free(pName);
            
            return Success;
        }

        public static void Log(string Message)
        {
            var pMsg = AllocString(Message);
            Log(pMsg);
            free(pMsg);
        }
        private static  void* AllocString(string String)
        {
            var Data = Encoding.UTF8.GetBytes(String + "\x0");

            byte* Buffer = (byte*)malloc(Data.Length);

            for (int i = 0; i < Data.Length; i++)
                Buffer[i] = Data[i];

            return Buffer;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        static extern void Log(void* Line);

        [MethodImpl(MethodImplOptions.InternalCall)]
        static extern int LoadStartModule(void* path, void* args, void* argp, int flags, void* option, void* status);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        static extern bool GetModuleBase(void* Name, void* BaseAddress, void* ModuleSize);
        [MethodImpl(MethodImplOptions.InternalCall)]
        static extern bool JailbreakCred(long AuthID);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool Unjailbreak();
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool IsJailbroken();
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void* malloc(int Size);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void free(void* Size);
    }
}