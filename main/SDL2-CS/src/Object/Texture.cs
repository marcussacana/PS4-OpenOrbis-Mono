using System;
using SDL2.Interface;

namespace SDL2.Object
{
    public class Texture : INative
    {
        public Texture(IntPtr Handler)
        {
            this.Handler = Handler;
        }
        
        public IntPtr Handler { get; }
    }
}