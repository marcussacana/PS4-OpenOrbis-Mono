using System;
using System.Collections.Generic;
using System.Linq;
using SharpGLES;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace OrbisGL.GL
{
    public class Texture : IDisposable
    {
        private static List<int> SlotQueue = new List<int>(32);

        private static List<int> SlotTexture = new List<int>(32);

        static Texture()
        {
            for (int i = 0; i < 32; i++)
                SlotQueue.Add(i);

            for (int i = 0; i < 32; i++)
                SlotTexture.Add(0);
        }

        internal int TextureID;
        private int TextureType;

        public Texture(bool Is2DTexture)
        {
            TextureType = Is2DTexture ? GLES20.GL_TEXTURE_2D : GLES20.GL_TEXTURE;

            int[] Textures = new int[1];
            GLES20.GenTextures(1, Textures);
            TextureID = Textures.First();
        }

        private void Bind(int Slot)
        {
            GLES20.ActiveTexture(GLES20.GL_TEXTURE0 + Slot);
            GLES20.BindTexture(TextureType, TextureID);
        }

        public unsafe void SetData(int Width, int Height, byte[] Data, PixelFormat Format)
        {
            Bind(Active());

            fixed (byte* pData = Data)
            {
                GLES20.TexImage2D(TextureType, 0, (int)Format, Width, Height, 0, (int)Format, GLES20.GL_UNSIGNED_BYTE, new IntPtr(pData));
                GLES20.TexParameteri(TextureType, GLES20.GL_TEXTURE_MAG_FILTER, GLES20.GL_NEAREST);
                GLES20.TexParameteri(TextureType, GLES20.GL_TEXTURE_MIN_FILTER, GLES20.GL_NEAREST);
            }
        }

        /// <summary>
        /// Set the Texture from common image format (.png, .jpg, .bmp)
        /// WARNING - SLOW METHOD, NOT RECOMMENDED
        /// </summary>
        [Obsolete("Slow Method, use SetData instead", false)]
        public void SetImage(byte[] Data, PixelFormat TextureFormat)
        {
            int Width, Height;
            byte[] Buffer;
            switch (TextureFormat)
            {
                case PixelFormat.RGB:
                    var Img24 = Image.Load<Rgb24>(Data);
                    Width = Img24.Width;
                    Height = Img24.Height;
                    Buffer = new byte[Width * Height * 3];
                    Img24.CopyPixelDataTo(Buffer);
                    break;
                case PixelFormat.RGBA:
                    var Img32 = Image.Load<Rgba32>(Data);
                    Width = Img32.Width;
                    Height = Img32.Height;
                    Buffer = new byte[Width * Height * 4];
                    Img32.CopyPixelDataTo(Buffer);
                    break;

                default:
                    throw new Exception("Unexpected Pixel Format");
            }

            SetData(Width, Height, Data, TextureFormat);
        }

        /// <summary>
        /// Select the oldest texture slot, activate this texture and bind it
        /// </summary>
        /// <returns>The used texture slot</returns>
        public int Active()
        {
            if (SlotTexture.Contains(TextureID))
            {
                var Slot = SlotTexture.IndexOf(TextureID);
                
                if (SlotQueue.Contains(Slot))
                    SlotQueue.Remove(Slot);

                SlotQueue.Add(Slot);
                return Slot;
            }
            
            var ActiveSlot = SlotQueue.First();
            SlotQueue.RemoveAt(0);
            SlotQueue.Add(ActiveSlot);

            SlotTexture[ActiveSlot] = TextureID;

            Bind(ActiveSlot);

            return ActiveSlot;
        }
        
        //Todo: Compressed Texture Support
        
        public void Dispose()
        {
            int[] Textures = new int[] { TextureID };
            GLES20.DeleteTextures(Textures.Length, Textures);
        }
    }
}