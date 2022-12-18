using System;

namespace SDL2.Interface
{
    public interface INative
    {
        IntPtr Handler { get; }
        
    }
}