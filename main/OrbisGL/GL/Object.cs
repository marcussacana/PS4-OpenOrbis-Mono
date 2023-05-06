using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using OrbisGL.GL;
using SharpGLES;

namespace OrbisGL.GL
{
    public class Object : IRenderable
    {
        private bool BufferInvalidated = true;
        
        private Program Program;
        private int RenderMode = 0;

        private List<byte> ArrayBuffer = new List<byte>();
        private List<byte> IndexBuffer = new List<byte>();

        private IntPtr pArrayBuffer = IntPtr.Zero;
        private IntPtr pIndexBuffer = IntPtr.Zero;

        private int GLArrayBuffer = 0;
        private int GLIndexBuffer = 0;

        private Texture Texture;

        public Object(Program Program, RenderMode Mode)
        {
            this.Program = Program;
            this.RenderMode = (int)Mode;
        }

        public unsafe void AddVertex(params float[] Points)
        {
            BufferInvalidated = true;
            fixed (byte* pPoints = &Points)
            {
                for (int i = 0; i < Points.Length * sizeof(float); i++)
                    ArrayBuffer.Add(pPoints[i]);
            }
        }

        public void AddIndex(params byte[] Indexes)
        {
            BufferInvalidated = true;
            IndexBuffer.AddRange(Indexes);
        }

        public void ClearBuffers()
        {
            ArrayBuffer.Clear();
            IndexBuffer.Clear();
            BufferInvalidated = true;
        }
        
        public unsafe void BuildBuffers()
        {
            if (!BufferInvalidated)
                return;
            
            BufferInvalidated = false;
            
            FreeBuffer();

            if (ArrayBuffer.Count > 0)
            {
                pArrayBuffer = Marshal.AllocHGlobal(ArrayBuffer.Count);
                fixed (byte* Buffer = &ArrayBuffer)
                {
                    byte* nBuffer = (byte*)pArrayBuffer.ToPointer();

                    for (int i = 0; i < ArrayBuffer.Count; i++)
                        nBuffer[i] = Buffer[i];
                }

                if (GLArrayBuffer == 0)
                {
                    GLArrayBuffer = GLES20.GenBuffers(1).First();
                }
                
                GLES20.BindBuffer(GLES20.GL_ARRAY_BUFFER, GLArrayBuffer);
                GLES20.BufferData(GLES20.GL_ARRAY_BUFFER, ArrayBuffer.Count, pArrayBuffer, GLES20.GL_STATIC_DRAW);
                
            }
            
            if (IndexBuffer.Count > 0)
            {
                pIndexBuffer = Marshal.AllocHGlobal(IndexBuffer.Count);
                fixed (byte* Buffer = &IndexBuffer)
                {
                    byte* nBuffer = (byte*)pIndexBuffer.ToPointer();

                    for (int i = 0; i < IndexBuffer.Count; i++)
                        nBuffer[i] = Buffer[i];
                }
                
                if (GLIndexBuffer == 0)
                {
                    GLIndexBuffer = GLES20.GenBuffers(1).First();
                }
                
                GLES20.BindBuffer(GLES20.GL_ELEMENT_ARRAY_BUFFER, GLIndexBuffer);
                GLES20.BufferData(GLES20.GL_ELEMENT_ARRAY_BUFFER, IndexBuffer.Count, pIndexBuffer, GLES20.GL_STATIC_DRAW);
            }
            
        }

        public void SetTexture(string UniformName, Texture Texture)
        {
            this.Texture = Texture;
            Program.SetUniform(UniformName, Texture.Active());
        }
        
        public void SetTexture(int UniformLocation, Texture Texture)
        {
            this.Texture = Texture;
            Program.SetUniform(UniformLocation, Texture.Active());
        }

        public void FreeBuffer()
        {
            if (pIndexBuffer != IntPtr.Zero)    
                Marshal.FreeHGlobal(pArrayBuffer);

            if (pArrayBuffer != IntPtr.Zero)
                Marshal.FreeHGlobal(pArrayBuffer);
        }

        public void Draw()
        {
            GLES20.UseProgram(Program.Handler);
            
            Program.ApplyAttributes();
            
            BuildBuffers();
            
            //Pixel Position Normalization, Render, Disposing
        }
    }
}