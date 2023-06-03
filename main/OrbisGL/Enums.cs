using System;
using SharpGLES;

namespace OrbisGL
{
    public enum RenderMode : int
    {
        Triangle = GLES20.GL_TRIANGLES,
        MultipleLines = GLES20.GL_LINES,
        SingleLine = GLES20.GL_LINE_STRIP,
        ClosedLine = GLES20.GL_LINE_LOOP
    }

    public enum AttributeType : int
    {
        Int = GLES20.GL_INT,
        UInt = GLES20.GL_UNSIGNED_INT,
        SByte = GLES20.GL_UNSIGNED_BYTE,
        Byte = GLES20.GL_BYTE,
        Short = GLES20.GL_SHORT,
        UShort = GLES20.GL_UNSIGNED_SHORT,
        Float = GLES20.GL_FLOAT,
        Double = GLES20.GL_HIGH_FLOAT
    }
    
    public enum AttributeSize : int
    {
        None = 1 << 0,
        FloatMatrix2 = 1 << 1,
        FloatMatrix3 = 1 << 2,
        FloatMatrix4 = 1 << 3,
        Vector2 = 1 << 4,
        Vector3 = 1 << 5,
        Vector4 = 1 << 6
    }

    public enum PixelFormat : int
    {
        RGBA = GLES20.GL_RGBA,
        RGB = GLES20.GL_RGB
    }

    [Flags]
    public enum ClickType
    {
        SingleClick = 1 << 0,
        DoubleClick = 1 << 1,
        Right = 1 << 2,
        Left = 1 << 3,
        Middle = 1 << 4
    }

    public enum OrbisPadPortType
    {
        Standard = 0,
        Special = 2
    }

    public enum OrbisPadDeviceClass
    {
        Pad = 0,
        Guitar = 1,
        Drums = 2
    }

    public enum OrbisPadConnectionType
    {
        Standard = 0,
        Remote = 2
    }

    [Flags]
    public enum OrbisPadButton
    {
        L3 = 0x0002,
        R3 = 0x0004,
        Options = 0x0008,
        Up = 0x0010,
        Right = 0x0020,
        Down = 0x0040,
        Left = 0x0080,
        L2 = 0x0100,
        R2 = 0x0200,
        L1 = 0x0400,
        R1 = 0x0800,
        Triangle = 0x1000,
        Circle = 0x2000,
        Cross = 0x4000,
        Square = 0x8000,
        TouchPad = 0x100000
    }
}