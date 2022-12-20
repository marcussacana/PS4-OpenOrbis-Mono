using System.Runtime.InteropServices;

namespace SDL2.Types.FreeType
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Generic
    {
        public void* Data;
        public void* Finalizer;
    }
}