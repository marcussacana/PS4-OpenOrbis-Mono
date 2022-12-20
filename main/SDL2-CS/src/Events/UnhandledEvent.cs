using System;
using System.Runtime.CompilerServices;
using SDL2.Types;

namespace SDL2.Events
{
    public class UnhandledEvent : EventArgs
    {
        public readonly SDL.SDL_Event Args;
        
        public UnhandledEvent(SDL.SDL_Event Args)
        {
            this.Args = Args;
        }
    }
}