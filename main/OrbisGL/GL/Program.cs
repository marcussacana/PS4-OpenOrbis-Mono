using System;
using System.Collections.Generic;
using SharpGLES;

namespace OrbisGL.GL
{
    public class Program
    {
        public readonly int Handler;
        public Program(int hProgram)
        {
            Handler = hProgram;
        }

        private int CurrentAttribOffset = 0;
        private List<BufferAttribute> Attribs = new List<BufferAttribute>();
        
        public void AddBufferAttribute(BufferAttribute Attribute)
        {
            GLES20.UseProgram(Handler);
            
            var Position = GLES20.GetAttribLocation(Handler, Attribute.Name);
            if (Position < 0)
                throw new KeyNotFoundException($"{Attribute.Name} Attribute Not Found");
            
            GLES20.EnableVertexAttribArray(Position);

            Attribute.AttributeIndex = Position;
            Attribute.AttributeSize = GetArraySize(Attribute.Size);
            Attribute.AttributeType = GetGLType(Attribute.Type, Attribute.Size);
            Attribute.AttributeOffset = CurrentAttribOffset;
            
            Attribs.Add(Attribute);
            
            CurrentAttribOffset += GetPrimitiveSize(Attribute.Type) * GetArraySize(Attribute.Size);
        }

        internal void ApplyAttributes()
        {
            for (int i = 0; i < Attribs.Count; i++)
            {
                var Attrib = Attribs[i];
                GLES20.VertexAttribPointer(Attrib.AttributeIndex, Attrib.AttributeSize, Attrib.AttributeType, false, CurrentAttribOffset, new IntPtr(Attrib.AttributeOffset));
            }
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

        int GetGLType(AttributeType Type, AttributeSize Size)
        {
            switch (Type)
            {
                case AttributeType.SByte:
                    if (Size == AttributeSize.None)
                        return GLES20.GL_BYTE;
                    throw new Exception("Unexpected Attribute Size");
                case AttributeType.Byte:
                    if (Size == AttributeSize.None)
                        return GLES20.GL_UNSIGNED_BYTE;
                    throw new Exception("Unexpected Attribute Size");
                case AttributeType.Short:
                    if (Size == AttributeSize.None)
                        return GLES20.GL_SHORT;
                    throw new Exception("Unexpected Attribute Size");
                case AttributeType.UShort:
                    if (Size == AttributeSize.None)
                        return GLES20.GL_UNSIGNED_SHORT;
                    throw new Exception("Unexpected Attribute Size");
                case AttributeType.Int:
                    switch (Size)
                    {
                        case AttributeSize.None:
                            return GLES20.GL_INT;
                        case AttributeSize.Vector2:
                            return GLES20.GL_INT_VEC2;
                        case AttributeSize.Vector3:
                            return GLES20.GL_INT_VEC3;
                        case AttributeSize.Vector4:
                            return GLES20.GL_INT_VEC4;
                        default:
                            throw new Exception("Unexpected Attribute Size");
                    }
                case AttributeType.UInt:
                    if (Size == AttributeSize.None)
                        return GLES20.GL_UNSIGNED_INT;
                    throw new Exception("Unexpected Attribute Size");
                case AttributeType.Float:
                    switch (Size)
                    {
                        case AttributeSize.None:
                            return GLES20.GL_FLOAT;
                        case AttributeSize.Vector2:
                            return GLES20.GL_FLOAT_VEC2;
                        case AttributeSize.Vector3:
                            return GLES20.GL_FLOAT_VEC3;
                        case AttributeSize.Vector4:
                            return GLES20.GL_FLOAT_VEC4;
                        case AttributeSize.FloatMatrix2:
                            return GLES20.GL_FLOAT_MAT2;
                        case AttributeSize.FloatMatrix3:
                            return GLES20.GL_FLOAT_MAT3;
                        case AttributeSize.FloatMatrix4:
                            return GLES20.GL_FLOAT_MAT4;
                        default:
                            throw new Exception("Unexpected Attribute Size");
                    }
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
    }
}