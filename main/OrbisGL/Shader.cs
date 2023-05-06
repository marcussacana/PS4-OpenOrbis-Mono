using System;
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
        
        public static int GetProgram(string VertexSource, string FragmentSource)
        {
            int Program = GLES20.CreateProgram();

            int Vertex, Fragment;

            if (GLES20.HasShaderCompiler)
            {
                Fragment = GetShader(GLES20.GL_FRAGMENT_SHADER, FragmentSource);
                Vertex = GetShader(GLES20.GL_VERTEX_SHADER, VertexSource);
            }
            else
            {
                throw new NotImplementedException();
            }

            GLES20.AttachShader(Program, Vertex);
            GLES20.AttachShader(Program, Fragment);
            GLES20.LinkProgram(Program);

            GLES20.DeleteShader(Vertex);
            GLES20.DeleteShader(Fragment);

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