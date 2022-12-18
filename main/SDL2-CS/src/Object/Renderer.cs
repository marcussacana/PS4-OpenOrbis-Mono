using System;
using System.Runtime.CompilerServices;
using SDL2.Interface;

namespace SDL2.Object
{
    public class Renderer : INative
    {
        public uint DefaultFrameDelay { get; set; }
        public Renderer(IntPtr Handler, uint FPS)
        {
            DefaultFrameDelay = 1000 / FPS;
            this.Handler = Handler;
        }
        
        public IntPtr Handler { get; }
    }
}