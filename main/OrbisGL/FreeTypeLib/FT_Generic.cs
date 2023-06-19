using System.Runtime.InteropServices;

namespace OrbisGL.FreeTypeLib
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Generic
    {
        public void* Data;
        public void* Finalizer;
    }
}