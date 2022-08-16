using System;
using System.IO;
using Orbis.Internals;
using SDL2.Events;
using SDL2.Object;
using SDL2.Types;
using static SDL2.SDL;

namespace Orbis
{
    internal class Program
    {

        private const int DVDSpeed = 8;
        
        
        const int FramesPerSecond = 60;
        const int FrameDelay = 1000 / FramesPerSecond;
        public static void Main()
        {
            Kernel.Log("DotNet Main() Called");

            User.Notify("Hello World from C#");
            var Window = new SampleWindow();
            Window.JoyButtonEvent += WindowOnJoyButtonEvent;
            
            Window.Run();
        }

        private static void WindowOnJoyButtonEvent(object sender, JoyButtonEvent e)
        {
            if (e.ButtonState != JoyButtonEvent.Type.Up)
                return;
            
            switch (e.Button)
            {
                case DS4Button.SCE_PAD_BUTTON_CROSS:
                    User.Notify("Cross");
                    break;
                case DS4Button.SCE_PAD_BUTTON_SQUARE:
                    User.Notify("Square is shit");
                    break;
                case DS4Button.SCE_PAD_BUTTON_CIRCLE:
                    User.Notify("Circle");
                    break;
                case DS4Button.SCE_PAD_BUTTON_TRIANGLE:
                    User.Notify("Triangle");
                    break;
            }
        }

    }
}