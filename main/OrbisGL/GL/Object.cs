using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using SharpGLES;

namespace OrbisGL.GL
{
    public class GLObject : IRenderable
    {
        private bool BufferInvalidated = true;
        
        public GLProgram Program { get; protected set; }

        protected int RenderMode = 0;

        private List<byte> ArrayBuffer = new List<byte>();
        private List<byte> IndexBuffer = new List<byte>();

        private IntPtr pArrayBuffer = IntPtr.Zero;
        private IntPtr pIndexBuffer = IntPtr.Zero;

        private int GLArrayBuffer = 0;
        private int GLIndexBuffer = 0;

        private Texture Texture;

        private bool ValidBuffer = false;

        private bool Disposed = false;

        protected GLObject() {
            this.RenderMode = (int)OrbisGL.RenderMode.Triangle;
        }

        int TimeUniformLocation = int.MinValue;

        public GLObject(GLProgram Program, RenderMode Mode)
        {
            this.Program = Program;
            this.RenderMode = (int)Mode;
        }
        protected void AddArray(Vector2 XY, float Z) => AddArray(XY.X, XY.Y, Z);

        protected void AddArray(params Vector3[] Points) => AddArray(Points.SelectMany(x => new[] { x.X, x.Y, x.Z }).ToArray());
        protected void AddArray(params Vector2[] Points) => AddArray(Points.SelectMany(x => new[] { x.X, x.Y }).ToArray());

        protected unsafe void AddArray(params float[] Points)
        {
            if (Disposed)
                throw new ObjectDisposedException(this.GetType().Name);
            
            BufferInvalidated = true;
            fixed (void* Pointer = Points)
            {
                byte* pPoints = (byte*)Pointer;
                for (int i = 0; i < Points.Length * sizeof(float); i++)
                    ArrayBuffer.Add(pPoints[i]);
            }
        }

        protected unsafe void IntAddArray(params int[] Numbers)
        {
            if (Disposed)
                throw new ObjectDisposedException(this.GetType().Name);
            
            BufferInvalidated = true;
            fixed (void* Pointer = Numbers)
            {
                byte* pPoints = (byte*)Pointer;
                for (int i = 0; i < Numbers.Length * sizeof(int); i++)
                    ArrayBuffer.Add(pPoints[i]);
            }
        }


        protected void AddArray(byte Alpha, params RGBColor[] Colors)
        {
            if (Disposed)
                throw new ObjectDisposedException(this.GetType().Name);
            
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
            if (Disposed)
                throw new ObjectDisposedException(this.GetType().Name);
            
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
            if (Disposed)
                throw new ObjectDisposedException(this.GetType().Name);
            
            if (!BufferInvalidated)
                return;

            ValidBuffer = false;
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

                ValidBuffer = true;
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
            if (Disposed)
                throw new ObjectDisposedException(this.GetType().Name);
            
            this.Texture = Texture;
            Program.SetUniform(UniformName, Texture.Active());
        }
        
        public void SetTexture(int UniformLocation, Texture Texture)
        {
            if (Disposed)
                throw new ObjectDisposedException(this.GetType().Name);
            
            this.Texture = Texture;
            Program.SetUniform(UniformLocation, Texture.Active());
        }

        public void FreeBuffer()
        {
            if (pIndexBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(pIndexBuffer);
                pIndexBuffer = IntPtr.Zero;
            }

            if (pArrayBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(pArrayBuffer);
                pArrayBuffer = IntPtr.Zero;
            }
        }

        public void UpdateUniforms(long Tick)
        {
            if (TimeUniformLocation >= 0)
            {
                Program.SetUniform(TimeUniformLocation, (Tick & 0xFFFFFFFFF) / 1_000_000.0f);
            }
            else if (TimeUniformLocation == int.MinValue)
            {
                TimeUniformLocation = GLES20.GetUniformLocation(Program.Handler, "Time");

                if (TimeUniformLocation != -1)
                    Program.SetUniform(TimeUniformLocation, (Tick & 0xFFFFFFFFF) / 1_000_000.0f);
            }
        }

        public virtual void Draw(long Tick)
        {
            if (Disposed)
                throw new ObjectDisposedException(this.GetType().Name);
            
            GLES20.UseProgram(Program.Handler);

            UpdateUniforms(Tick);

            BuildBuffers();

            if (!ValidBuffer)
                return;

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
        }

        public virtual void Dispose()
        {
            if (Disposed)
                return;
            
            Program?.Dispose();
            Texture?.Dispose();
            int Count = 0;
            
            int[] Buffers = new int[2];
            if (GLArrayBuffer != 0)
                Buffers[Count++] = GLArrayBuffer;

            if (GLIndexBuffer != 0)
                Buffers[Count++] = GLIndexBuffer;
            
            GLES20.DeleteBuffers(Count, Buffers);

            Disposed = true;
        }
    }
}