using System.Runtime.InteropServices;

namespace OrbisGL.FreeTypeLib
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Charmap
    {
        public FT_Face* Face;
        public FT_Encoding Encoding;
        public ushort PlatformID;
        public ushort EncodingID;
    }
}