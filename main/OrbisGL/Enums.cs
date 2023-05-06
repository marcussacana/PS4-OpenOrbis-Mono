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
}