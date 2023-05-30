using System.Runtime.InteropServices;

namespace OrbisGL.FreeType
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Generic
    {
        public void* Data;
        public void* Finalizer;
    }
}