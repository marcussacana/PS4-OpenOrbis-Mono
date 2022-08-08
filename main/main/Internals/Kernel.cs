using System.Runtime.CompilerServices;
using Orbis.String;

namespace Orbis.Internals
{
    public unsafe class Kernel
    {

        public static void Log(string Line, params object[] Format)
        {
            Log((CString)string.Format(Line, Format));
        }
        public static void Log(CString Line)
        {
            Log((void*)Line);
        }
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        static extern void Log(void* Line);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool Jailbreak();
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool Unjailbreak();
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void* malloc(int Size);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void free(void* Size);
    }
}