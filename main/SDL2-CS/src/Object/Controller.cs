using SDL2.Types;

namespace SDL2.Object
{
    public static class Joystick
    {
        public static int Online => SDL.SDL_NumJoysticks();

        public static void Open(int Index)
        {
            SDL.SDL_JoystickOpen(Index);
        }
    }
}