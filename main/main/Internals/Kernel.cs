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
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        static extern void Log(void* Line);
        
        //[MethodImpl(MethodImplOptions.InternalCall)]
        //public static extern bool Jailbreak();
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        static extern bool JailbreakCred(long AuthID);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool Unjailbreak();
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void* malloc(int Size);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void free(void* Size);
    }
}