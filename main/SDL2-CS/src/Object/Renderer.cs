using System;
using SDL2.Interface;

namespace SDL2.Object
{
    public class Renderer : INative
    {
        public Renderer(IntPtr Handler)
        {
            this.Handler = Handler;
        }
        
        public IntPtr Handler { get; }
    }
}