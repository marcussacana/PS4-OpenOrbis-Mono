using System;
using System.Runtime.CompilerServices;

namespace Orbis.Internals
{
    public unsafe class IO
    {

        public static string GetAppBaseDirectory()
        {
            return Kernel.ParseString(GetBaseDirectory());
        }
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        static extern byte* GetBaseDirectory();
    }
}