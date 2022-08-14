using System;
using System.Runtime.CompilerServices;
using SDL2.Types;

namespace SDL2.Events
{
    public class JoyButtonEvent : EventArgs
    {
        public readonly SDL.SDL_JoyButtonEvent Args;
        public readonly Type ButtonState;
        
        public DS4Button Button => (DS4Button)Args.button;
        public int DeviceID => Args.which;

        public enum Type
        {
            Up, Down
        }
        public JoyButtonEvent(SDL.SDL_JoyButtonEvent Args, Type ButtonState)
        {
            this.Args = Args;
        }
    }
}