using System;
using System.IO;
using Orbis.Internals;
using SDL2;
using SDL2.Types;
using static SDL2.SDL;

namespace Orbis
{
    internal class Program
    {
        private const int WINDOW_WIDTH = 1920;
        private const int WINDOW_HEIGHT = 1080;
        private static Random Rand = new Random();

        private const int DVDSpeed = 8;
        
        
        const int FramesPerSecond = 60;
        const int FrameDelay = 1000 / FramesPerSecond;
        public static void Main()
        {
            Kernel.Log("DotNet Main() Called");

            User.Notify("Hello World from C#");
            if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_JOYSTICK) != 0)
            {
                User.Notify("Failed to Init the SDL2");
                return;
            }

            var Window = SDL_CreateWindow("main", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, WINDOW_WIDTH, WINDOW_HEIGHT, SDL_WindowFlags.NONE);

            if (Window == IntPtr.Zero)
            {
                User.Notify("Failed to Create the Window");
                return;
            }

            var WindowSurface = SDL_GetWindowSurface(Window);
            var Renderer = SDL_CreateSoftwareRenderer(WindowSurface);

            if (Renderer == IntPtr.Zero)
            {
                User.Notify("Failed to Create the Renderer");
                return;
            }

            SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_WEBP);

            string BaseDir = IO.GetAppBaseDirectory();
            string PNGPath = Path.Combine(BaseDir, "assets", "images", "dvd-logo.tga");
            
            Surface Surface = SDL_image.IMG_Load(PNGPath);
            if (Surface == IntPtr.Zero)
            {
                Kernel.Log("Failed to load the assets: {0}", PNGPath);
                return;
            }

            var Texture = SDL_CreateTextureFromSurface(Renderer, Surface);
            if (Texture == IntPtr.Zero)
            {
                Kernel.Log("Failed to get the Surface texture");
                return;
            }
            
            Size DVDImgSize = new Size()
            {
                Width = Surface.Inner.w,
                Height = Surface.Inner.h
            };
            
            Kernel.Log("Image Loaded, Width: {0}, Height: {1}", DVDImgSize.Width, DVDImgSize.Height);
                
            SDL_FreeSurface(Surface);


            Rect DVDPos = new Rect();
            DVDPos.Inner.w = DVDImgSize.Width;
            DVDPos.Inner.h = DVDImgSize.Height;

            DVDPos.Inner.x = Rand.Next(0,WINDOW_WIDTH - DVDImgSize.Width);
            DVDPos.Inner.y = Rand.Next(0, WINDOW_HEIGHT - DVDImgSize.Height);
            
            
            Loop(Window, Renderer, Texture, DVDPos);
        }

        private static void Loop(IntPtr Window, IntPtr Renderer, IntPtr Texture, Rect DVDPos)
        {
            bool Reverse = Rand.Next(2) == 0;

            int VelX, VelY;
            VelX = VelY = Reverse ? -1 : 1;
            
            ChangeColor(Texture);
            
            while (true)
            {
                var FrameStart = SDL_GetTicks();

                SDL_SetRenderDrawColor(Renderer, 0, 0, 0, 0xFF);
                SDL_RenderClear(Renderer);
                SDL_RenderCopy(Renderer, Texture, IntPtr.Zero, DVDPos);
                SDL_RenderPresent(Renderer);

                Collision(DVDPos, Texture, ref VelX, ref VelY);
                
                var FrameTime = SDL_GetTicks() - FrameStart;

                if (FrameTime < FrameDelay)
                {
                    SDL_Delay(FrameDelay - FrameTime);
                }
                
                SDL_UpdateWindowSurface(Window);
                
                DVDPos.Inner.x += VelX * DVDSpeed / 2;
                DVDPos.Inner.y += VelY * DVDSpeed / 2;
            }
        }

        private static void Collision(Rect DVDPos, IntPtr Texture, ref int VelX, ref int VelY)
        {
            if (DVDPos.Inner.x + DVDPos.Inner.w >= WINDOW_WIDTH) {
                VelX = -1;
                ChangeColor(Texture);
            }
            if (DVDPos.Inner.x <= 0) {
                VelX = 1;
                ChangeColor(Texture);
            }
            if (DVDPos.Inner.y <= 0) {
                VelY = 1;
                ChangeColor(Texture);
            }
            if (DVDPos.Inner.y + DVDPos.Inner.h >= WINDOW_HEIGHT) {
                VelY = -1;
                ChangeColor(Texture);
            }
        }
        
        private static void ChangeColor(IntPtr Texture) {
            SDL_SetTextureColorMod(Texture, (byte)Rand.Next(256), (byte)Rand.Next(256), (byte)Rand.Next(256));
        }
    }
}