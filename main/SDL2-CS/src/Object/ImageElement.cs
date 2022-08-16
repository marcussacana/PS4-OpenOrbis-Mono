using System;
using System.IO;
using System.Runtime.InteropServices;
using SDL2.Exceptions;
using SDL2.Interface;
using SDL2.Types;
using SixLabors.ImageSharp;

using static SDL2.SDL;
using static SDL2.SDL_image;

using SDLSize = SDL2.Types.Size;

namespace SDL2.Object
{
    public class ImageElement : Element
    {
        #region Constructor
        public ImageElement(Element Parent, string Path) : base(Path)
        {
            NativeStruct<SDL_Surface> Surface;
            if (System.IO.Path.GetExtension(Path).EndsWith(".tga", StringComparison.InvariantCultureIgnoreCase))
            {
                Surface = IMG_Load(Path);
            }
            else
            {
                IntPtr RawMem = IntPtr.Zero;
                int RawMemSize = 0;
                
                using (MemoryStream Mem = new MemoryStream())
                {
                    var IMG = Image.Load(Path);
                    IMG.SaveAsTga(Mem);
                    var Buffer = Mem.ToArray();
                    
                    RawMem = Marshal.AllocHGlobal(Buffer.Length);
                    Marshal.Copy(Buffer, 0, RawMem, Buffer.Length);
                    
                    RawMemSize = Buffer.Length;
                }

                var RWOps = SDL_RWFromMem(RawMem, RawMemSize);
                Surface = IMG_LoadTyped_RW(RWOps, 1, "TGA");
                Marshal.FreeHGlobal(RawMem);
                
            }

            if (Surface.Handler == IntPtr.Zero)
                throw new SDLException();

            this.Parent = Parent;
            Renderer = Parent.Renderer;
            
            var gpuTexture = SDL_CreateTextureFromSurface(Renderer.Handler, Surface);

            Size = new SDLSize(Surface.Inner.w, Surface.Inner.h);
            
            SDL_FreeSurface(Surface);
            
            if (gpuTexture == IntPtr.Zero)
                throw new SDLException();

            Texture = new Texture(gpuTexture);
            
            Parent.Childs.Add(this);
        }
        #endregion

        
        public void ChangeColor(byte R, byte G, byte B) {
            SDL_SetTextureColorMod(Texture.Handler, R, G, B);
            Invalidated = true;
        }
        
        public override Element Parent { get; set; }
        public override Renderer Renderer { get; set; }
        public override INative Texture { get; set; }
    }
}