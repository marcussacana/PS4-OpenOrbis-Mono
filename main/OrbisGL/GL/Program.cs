using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SharpGLES;

namespace OrbisGL.GL
{
    public class GLProgram : IDisposable
    {
        public int AttributeCount => Attribs.Count;

        public int VerticeSize => MaxAttribOffset;
        
        public readonly int Handler;
        public GLProgram(int hProgram)
        {
            Handler = hProgram;
        }

        private int MaxAttribOffset = 0;
        private List<BufferAttribute> Attribs = new List<BufferAttribute>();

        public void AddBufferAttribute(string Name, AttributeType Type, AttributeSize Size) => AddBufferAttribute(new BufferAttribute(Name, Type, Size));
        public void AddBufferAttribute(BufferAttribute Attribute)
        {
            GLES20.UseProgram(Handler);

            GLES20.BindAttribLocation(Handler, Attribs.Count, Attribute.Name);

            var Position = GLES20.GetAttribLocation(Handler, Attribute.Name);
            if (Position < 0)
                throw new KeyNotFoundException($"{Attribute.Name} Attribute Not Found");
            
            GLES20.EnableVertexAttribArray(Position);

            Attribute.AttributeIndex = Position;
            Attribute.AttributeSize = GetArraySize(Attribute.Size);
            Attribute.AttributeType = GetGLType(Attribute.Type, Attribute.Size);
            Attribute.AttributeOffset = MaxAttribOffset;
            
            Attribs.Add(Attribute);
            
            MaxAttribOffset += GetPrimitiveSize(Attribute.Type) * GetArraySize(Attribute.Size);
        }

        internal void ApplyAttributes()
        {
            for (int i = 0; i < Attribs.Count; i++)
            {
                var Attrib = Attribs[i];
                GLES20.EnableVertexAttribArray(Attrib.AttributeIndex);
                GLES20.VertexAttribPointer(Attrib.AttributeIndex, Attrib.AttributeSize, Attrib.AttributeType, false, MaxAttribOffset, new IntPtr(Attrib.AttributeOffset));
            }
        }

        public void SetUniform(string Name, RGBColor Value, byte Alpha) => SetUniform(GLES20.GetUniformLocation(Handler, Name), Value, Alpha);

        public void SetUniform(int Location, RGBColor Value, byte Alpha)
        {
            GLES20.UseProgram(Handler);
            var AlphaF = Alpha / 255F;
            GLES20.Uniform4f(Location, Value.RedF, Value.GreenF, Value.BlueF, AlphaF);
        }

        public void SetUniform(string Name, int Value)  => SetUniform(GLES20.GetUniformLocation(Handler, Name), Value);
        
        public void SetUniform(int Location, int Value)
        {
            GLES20.UseProgram(Handler);
            GLES20.Uniform1i(Location, Value);
        }
        
        public void SetUniform(string Name, int ValueA, int ValueB)  => SetUniform(GLES20.GetUniformLocation(Handler, Name), ValueA, ValueB);
        public void SetUniform(int Location, int ValueA, int ValueB)
        {
            GLES20.UseProgram(Handler);
            GLES20.Uniform2i(Location, ValueA, ValueB);
        }
        
        public void SetUniform(string Name, int ValueA, int ValueB, int ValueC)  => SetUniform(GLES20.GetUniformLocation(Handler, Name), ValueA, ValueB, ValueC);
        public void SetUniform(int Location, int ValueA, int ValueB, int ValueC)
        {
            GLES20.UseProgram(Handler);
            GLES20.Uniform3i(Location, ValueA, ValueB, ValueC);
        }
        
        public void SetUniform(string Name, int ValueA, int ValueB, int ValueC, int ValueD)  => SetUniform(GLES20.GetUniformLocation(Handler, Name), ValueA, ValueB, ValueC, ValueD);
        public void SetUniform(int Location, int ValueA, int ValueB, int ValueC, int ValueD)
        {
            GLES20.UseProgram(Handler);
            GLES20.Uniform4i(Location, ValueA, ValueB, ValueC, ValueD);
        }
        
        
        public void SetUniform(string Name, float Value)  => SetUniform(GLES20.GetUniformLocation(Handler, Name), Value);
        public void SetUniform(int Location, float Value)
        {
            GLES20.UseProgram(Handler);
            GLES20.Uniform1f(Location, Value);
        }
        
        public void SetUniform(string Name, float ValueA, float ValueB)  => SetUniform(GLES20.GetUniformLocation(Handler, Name), ValueA, ValueB);
        public void SetUniform(int Location, float ValueA, float ValueB)
        {
            GLES20.UseProgram(Handler);
            GLES20.Uniform2f(Location, ValueA, ValueB);
        }
        
        public void SetUniform(string Name, float ValueA, float ValueB, float ValueC)  => SetUniform(GLES20.GetUniformLocation(Handler, Name), ValueA, ValueB, ValueC);
        public void SetUniform(int Location, float ValueA, float ValueB, float ValueC)
        {
            GLES20.UseProgram(Handler);
            GLES20.Uniform3f(Location, ValueA, ValueB, ValueC);
        }
        
        public void SetUniform(string Name, float ValueA, float ValueB, float ValueC, float ValueD)  => SetUniform(GLES20.GetUniformLocation(Handler, Name), ValueA, ValueB, ValueC, ValueD);
        public void SetUniform(int Location, float ValueA, float ValueB, float ValueC, float ValueD)
        {
            GLES20.UseProgram(Handler);
            GLES20.Uniform4f(Location, ValueA, ValueB, ValueC, ValueD);
        }

        public void SetUniform(string Name, Vector2 Value) => SetUniform(GLES20.GetUniformLocation(Handler, Name), Value);
        public void SetUniform(int Location, Vector2 Value) => SetUniform(Location, Value.X, Value.Y);

        public void SetUniform(string Name, Vector3 Value) => SetUniform(GLES20.GetUniformLocation(Handler, Name), Value);
        public void SetUniform(int Location, Vector3 Value) => SetUniform(Location, Value.X, Value.Y, Value.Z);

        public void SetUniform(string Name, Matrix4x4 Matrix) => SetUniform(GLES20.GetUniformLocation(Handler, Name), Matrix);
        public unsafe void SetUniform(int Location, Matrix4x4 Matrix)
        {
            GLES20.UseProgram(Handler);
            Matrix4x4* pMatrix = &Matrix;
            GLES20.UniformMatrix4fv(Location, 1, false, pMatrix);
        }

        int GetGLType(AttributeType Type, AttributeSize Size)
        {
            switch (Type)
            {
                case AttributeType.SByte:
                    return GLES20.GL_BYTE;
                case AttributeType.Byte:
                    return GLES20.GL_UNSIGNED_BYTE;
                case AttributeType.Short:
                    return GLES20.GL_SHORT;
                case AttributeType.UShort:
                    return GLES20.GL_UNSIGNED_SHORT;
                case AttributeType.Int:
                    return GLES20.GL_INT;
                case AttributeType.UInt:
                    return GLES20.GL_UNSIGNED_INT;
                case AttributeType.Float:
                    return GLES20.GL_FLOAT;
                default:
                    throw new Exception("Unexpected Attribute Type");
            }
        }

        int GetArraySize(AttributeSize Size)
        {
            switch (Size)
            {
                case AttributeSize.None:
                    return 1;
                case AttributeSize.Vector2:
                    return 2;
                case AttributeSize.Vector3:
                    return 3;
                case AttributeSize.Vector4:
                    return 4;
                case AttributeSize.FloatMatrix2:
                    return 2 * 2;
                case AttributeSize.FloatMatrix3:
                    return 3 * 3;
                case AttributeSize.FloatMatrix4:
                    return 4 * 4;
            }
            
            throw new Exception("Unexpected Attribute Size");
        }

        int GetPrimitiveSize(AttributeType Type)
        {
            switch (Type)
            {
                case AttributeType.SByte:
                case AttributeType.Byte:
                    return 1;
                case AttributeType.Short:
                case AttributeType.UShort:
                    return 2;
                case AttributeType.Int:
                case AttributeType.UInt:
                case AttributeType.Float:
                    return 4;
            }

            throw new Exception("Unexpected Attribute Type");
        }

        public void Dispose()
        {
            GLES20.DeleteProgram(Handler);
        }
    }
}