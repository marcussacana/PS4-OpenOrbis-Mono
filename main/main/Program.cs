using System;
using SDL2;

namespace Orbis
{
    internal class Program
    {
        public static void Main()
        {
            User.Notify("Hello World from C#");
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO | SDL.SDL_INIT_JOYSTICK) != 0)
            {
                Kernel.Log("Failed to Init the SDL2");
                return;
            }
            
            Kernel.Log("SDL2 Initialized");

            while (true) continue;
        }
    }
}