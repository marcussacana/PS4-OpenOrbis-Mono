using System;
using System.Runtime.CompilerServices;
using Orbis.String;

namespace Orbis.Internals
{
    public unsafe class Kernel
    {

        public static void Log(string Line, params object[] Format)
        {
            LogStr((CString)string.Format(Line, Format));
        }
        static void LogStr(CString Line)
        {
            Log((void*)Line);
        }

        public static bool Jailbreak(long AuthID = 0)
        {
            return JailbreakCred(AuthID);
        }

        public static int LoadStartModule(string Path, out int Status)
        {
            int LoadStatus = 0;
            var Result = LoadStartModule((CString)Path, null, null, 0, null, &LoadStatus);
            Status = LoadStatus;
            return Result;
        }

        public static bool GetModuleBase(string Name, out long BaseAddress, out long ModuleSize)
        {
            long bAddr = 0;
            long mSize = 0;
            
            var Success = GetModuleBase((CString)Name, &bAddr, &mSize);

            BaseAddress = bAddr;
            ModuleSize = mSize;

            return Success;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        static extern int LoadStartModule(void* path, void* args, void* argp, int flags, void* option, void* status);


        [MethodImpl(MethodImplOptions.InternalCall)]
        static extern bool GetModuleBase(void* Name, void* BaseAddress, void* ModuleSize);

        [MethodImpl(MethodImplOptions.InternalCall)]
        static extern void Log(void* Line);
        
        //[MethodImpl(MethodImplOptions.InternalCall)]
        //public static extern bool Jailbreak();
        
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