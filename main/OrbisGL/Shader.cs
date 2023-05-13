using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SharpGLES;

namespace OrbisGL
{
    public static class Shader
    {  
        public static int GetShader(int Type, string Source)
        {
            int hShader = GLES20.CreateShader(Type);

            if (hShader == 0)
                throw new Exception("Failed to Create the Shader");

            GLES20.ShaderSource(hShader, Source);

            GLES20.CompileShader(hShader);


            GLES20.GetShaderiv(hShader, GLES20.GL_COMPILE_STATUS, out int Status);


            if (Status == 0)
            {
                var Info = GLES20.GetShaderInfoLog(hShader);

                GLES20.DeleteShader(hShader);

                throw new Exception("Failed to Compile the Shader: " + Info);
            }

            return hShader;
        }


#if ORBIS
        public static int GetShader(int Type, byte[] Data)
        {
            int hShader = GLES20.CreateShader(Type);

            if (hShader == 0)
                throw new Exception("Failed to Create the Shader");

            IntPtr Buffer = Marshal.AllocHGlobal(Data.Length);
            Marshal.Copy(Data, 0, Buffer, Data.Length);

            GLES20.ShaderBinary(1, new int[] { hShader }, GLES20.PrecompiledFormat, Buffer, Data.Length);


            GLES20.GetShaderiv(hShader, GLES20.GL_COMPILE_STATUS, out int Status);


            if (Status == 0)
            {
                var Info = GLES20.GetShaderInfoLog(hShader);

                GLES20.DeleteShader(hShader);

                throw new Exception("Failed to Compile the Shader: " + Info);
            }

            Marshal.FreeHGlobal(Buffer);

            return hShader;
        }

        public static byte[] GetShaderBinary(int Type, string Source)
        {
            var hShader = GetShader(Type, Source);

            var Data = GLES20.GetShaderBinary(hShader);
            
            GLES20.DeleteShader(hShader);

            return Data;
        }

        public static int GetProgram(byte[] VertexData, byte[] FragmentData)
        {
            int hVertex = GetShader(GLES20.GL_VERTEX_SHADER, VertexData);
            int hFragment = GetShader(GLES20.GL_FRAGMENT_SHADER, FragmentData);

            return GetProgram(hVertex, hFragment);
        }

#else
        public static byte[] GetProgramBinary(string VertexSource, string FragmentSource, out int BinaryFormat)
        {
            var hVertexShader = GetShader(GLES20.GL_VERTEX_SHADER, VertexSource);
            var hFragmenetShader = GetShader(GLES20.GL_FRAGMENT_SHADER, FragmentSource);

            var hProgram = GetProgram(hVertexShader, hFragmenetShader);

            var Data = GLES20.GetShaderBinary(hProgram, out BinaryFormat);

            GLES20.DeleteProgram(hProgram);

            return Data;
        }

        public unsafe static int GetProgram(byte[] Program, int Format)
        {
            int hProgram = GLES20.CreateProgram();

            fixed (void* hData = Program) {
                GLES20.ProgramBinary(hProgram, Format, new IntPtr(hData), Program.Length);

                GLES20.GetProgramiv(hProgram, GLES20.GL_LINK_STATUS, out int Status);

                if (Status == 0)
                {
                    var Info = GLES20.GetProgramInfoLog(hProgram);

                    GLES20.DeleteProgram(hProgram);

                    throw new Exception("Failed to Initialize the GL Program: " + Info);
                }

                int LastError = State.GetLastGLError();
                if (LastError != GLES20.GL_NO_ERROR)
                {
                    throw new Exception("OpenGL Error: 0x" + LastError.ToString("X8"));
                }

                return hProgram;
            }
        }

#endif
        public static int GetProgram(string VertexSource, string FragmentSource)
        {
            int Vertex = GetShader(GLES20.GL_VERTEX_SHADER, VertexSource);
            int Fragment = GetShader(GLES20.GL_FRAGMENT_SHADER, FragmentSource);

            return GetProgram(Vertex, Fragment);
        }

        static int GetProgram(int hVertex, int hFragment)
        {
            int Program = GLES20.CreateProgram();

            GLES20.AttachShader(Program, hVertex);
            GLES20.AttachShader(Program, hFragment);

            GLES20.LinkProgram(Program);

            GLES20.DeleteShader(hVertex);
            GLES20.DeleteShader(hFragment);

            GLES20.GetProgramiv(Program, GLES20.GL_LINK_STATUS, out int Status);

            if (Status == 0)
            {
                var Info = GLES20.GetProgramInfoLog(Program);

                GLES20.DeleteProgram(Program);

                throw new Exception("Failed to Initialize the GL Program: " + Info);
            }

            int LastError = State.GetLastGLError();
            if (LastError != GLES20.GL_NO_ERROR)
            {
                throw new Exception("OpenGL Error: 0x" + LastError.ToString("X8"));
            }

            return Program;
        }
    }
}