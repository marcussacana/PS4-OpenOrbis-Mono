using System;
using System.Runtime.CompilerServices;
using SDL2.Types;

namespace SDL2.Events
{
    public class JoyDeviceRemovedEvent : EventArgs
    {
        public readonly SDL.SDL_JoyDeviceEvent Args;
        public int DeviceID => Args.which;
        
        public JoyDeviceRemovedEvent(SDL.SDL_JoyDeviceEvent Args)
        {
            this.Args = Args;
        }
    }
}