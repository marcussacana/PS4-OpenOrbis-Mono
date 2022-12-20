using System;
using System.Runtime.CompilerServices;
using SDL2.Types;

namespace SDL2.Events
{
    public class JoyDeviceAddedEvent : EventArgs
    {
        public readonly SDL.SDL_JoyDeviceEvent Args;
        public int DeviceID => Args.which;
        
        public JoyDeviceAddedEvent(SDL.SDL_JoyDeviceEvent Args)
        {
            this.Args = Args;
        }
    }
}