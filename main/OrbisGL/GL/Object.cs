﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using SharpGLES;

namespace OrbisGL.GL
{
    public class GLObject : IRenderable
    {
        private bool BufferInvalidated = true;
        
        private GLProgram Program;
        private int RenderMode = 0;

        private List<byte> ArrayBuffer = new List<byte>();
        private List<byte> IndexBuffer = new List<byte>();

        private IntPtr pArrayBuffer = IntPtr.Zero;
        private IntPtr pIndexBuffer = IntPtr.Zero;

        private int GLArrayBuffer = 0;
        private int GLIndexBuffer = 0;

        private Texture Texture;

        public GLObject(GLProgram Program, RenderMode Mode)
        {
            this.Program = Program;
            this.RenderMode = (int)Mode;
        }

        public unsafe void AddArray(params float[] Points)
        {
            BufferInvalidated = true;
            fixed (void* Pointer = Points)
            {
                byte* pPoints = (byte*)Pointer;
                for (int i = 0; i < Points.Length * sizeof(float); i++)
                    ArrayBuffer.Add(pPoints[i]);
            }
        }

        public unsafe void AddArray(byte Alpha, params RGBColor[] Colors)
        {
            var AlphaF = Alpha/255f;
            BufferInvalidated = true;
            for (int i = 0; i < Colors.Length; i++)
            {
                ArrayBuffer.AddRange(BitConverter.GetBytes(Colors[i].RedF));
                ArrayBuffer.AddRange(BitConverter.GetBytes(Colors[i].GreenF));
                ArrayBuffer.AddRange(BitConverter.GetBytes(Colors[i].BlueF));
                ArrayBuffer.AddRange(BitConverter.GetBytes(AlphaF));
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
                var Buffer = ArrayBuffer.ToArray();
                fixed (byte* pBuffer = Buffer)
                {
                    byte* nBuffer = (byte*)pArrayBuffer.ToPointer();

                    for (int i = 0; i < ArrayBuffer.Count; i++)
                        nBuffer[i] = pBuffer[i];
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
                var Buffer = IndexBuffer.ToArray();
                fixed (byte* pBuffer = Buffer)
                {
                    byte* nBuffer = (byte*)pIndexBuffer.ToPointer();

                    for (int i = 0; i < IndexBuffer.Count; i++)
                        nBuffer[i] = pBuffer[i];
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
                Marshal.FreeHGlobal(pIndexBuffer);

            if (pArrayBuffer != IntPtr.Zero)
                Marshal.FreeHGlobal(pArrayBuffer);
        }

        public void Draw()
        {
            GLES20.UseProgram(Program.Handler);
            
            BuildBuffers();

            GLES20.BindBuffer(GLES20.GL_ARRAY_BUFFER, GLArrayBuffer);

            Program.ApplyAttributes();

            if (GLIndexBuffer != 0)
            {
                GLES20.BindBuffer(GLES20.GL_ELEMENT_ARRAY_BUFFER, GLIndexBuffer);
                
                GLES20.DrawElements(RenderMode, IndexBuffer.Count, GLES20.GL_UNSIGNED_BYTE, IntPtr.Zero);
            }
            else
            {
                GLES20.DrawArrays(RenderMode, 0, ArrayBuffer.Count / Program.VerticeSize);
            }


            //Render, Disposing
        }

        public void Dispose()
        {
            Program?.Dispose();
            Texture?.Dispose();
            int Count = 0;
            
            int[] Buffers = new int[2];
            if (GLArrayBuffer != 0)
                Buffers[Count++] = GLArrayBuffer;

            if (GLIndexBuffer != 0)
                Buffers[Count++] = GLIndexBuffer;
            
            GLES20.DeleteBuffers(Count, Buffers);
        }
    }
}