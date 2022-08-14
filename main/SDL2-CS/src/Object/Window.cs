using System;
using SDL2.Events;
using SDL2.Exceptions;
using SDL2.Interface;
using SDL2.Types;

using static SDL2.SDL;

namespace SDL2.Object
{
    public class Window : Element, INative
    {
        
        #region Proprieties
        public sealed override Element Parent { get; set; } = null;
        public sealed override INative Renderer { get; set; }
        public sealed override INative Texture { get; set; } = null;
        public IntPtr Handler { get; private set; }

        private uint _FPS = 60;

        public uint FPS
        {
            get => _FPS;
            set
            {
                _FPS = value;
                FrameDelay = 1000 / value;
            }
        }

        public override Size Size
        {
            get
            {
                SDL_GetWindowSize(Handler, out var Width, out var Height);
                return new Size(Width, Height);
            }
            set => SDL_SetWindowSize(Handler, value.Width, value.Height);
        }

        private NativeStruct<SDL_Surface> _Surface = null;

        public NativeStruct<SDL_Surface> Surface
        {
            get
            {
                if (_Surface != null)
                    return _Surface;
                return _Surface = SDL_GetWindowSurface(Handler);
            }
        }

        public SDL_DisplayMode DisplayMode
        {
            get
            {
                int Status = SDL_GetWindowDisplayMode(Handler, out SDL_DisplayMode Mode);
                
                if (Status < 0)
                    throw new SDLException(Status);

                return Mode;
            }
            set
            {
                int Status = SDL_SetWindowDisplayMode(Handler, ref value);
                
                if (Status < 0)
                    throw new SDLException(Status);
            }
        }

        public event EventHandler<JoyButtonEvent> JoyButtonEvent;
        public event EventHandler<JoyDeviceAddedEvent> JoyAddedEvent;
        public event EventHandler<JoyDeviceRemovedEvent> JoyRemovedEvent;
        public event EventHandler<UnhandledEvent> UnhandledEvent;
        #endregion

        #region Fields
        private static bool SDLInitialized = false;
        private uint FrameDelay;
        private bool Quit;
        #endregion

        #region Constructors
        public Window(int Width, int Height) : this("SDLWindow", Width, Height) { }
        public Window(string Title, int Width, int Height, SDL_WindowFlags Flags = SDL_WindowFlags.NONE) : base(Title)
        {
            if (!SDLInitialized && SDL_Init(SDL_INIT_VIDEO | SDL_INIT_JOYSTICK) != 0)
                throw new SDLException();

            SDLInitialized = true;
            
            Handler = SDL_CreateWindow(Title, SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, Width, Height, Flags);
            if (Handler == IntPtr.Zero)
                throw new SDLException();

            Renderer = new Renderer(SDL_CreateSoftwareRenderer(Surface));
        }
        #endregion

        #region Methods

        public void Run()
        {
            Quit = false;
            while (!Quit)
            {
                var FrameTime = SDL_GetTicks();

                ProcessEvents();
                
                if (!NeedsRedraw(FrameTime))
                {
                    if (FrameTime < FrameDelay)
                        SDL_Delay(FrameDelay - FrameTime);
                    
                    continue;
                }
                
                int Status = SDL_SetRenderDrawColor(Renderer.Handler, 0, 0, 0, 0xFF);
                if (Status < 0)
                    throw new SDLException();
                
                Status = SDL_RenderClear(Renderer.Handler);
                if (Status < 0)
                    throw new SDLException();

                OnDraw(FrameTime);
                
                Status = SDL_UpdateWindowSurface(Handler);
                if (Status < 0)
                    throw new SDLException();
            }
        }
        
        private void ProcessEvents()
        {
            while (SDL_PollEvent(out var Event) != 0)
            {
                switch (Event.type)
                {
                    case SDL_EventType.SDL_JOYBUTTONUP:
                        JoyButtonEvent?.Invoke(this, new JoyButtonEvent(Event.jbutton, Events.JoyButtonEvent.Type.Up));
                        break;
                    case SDL_EventType.SDL_JOYBUTTONDOWN:
                        JoyButtonEvent?.Invoke(this, new JoyButtonEvent(Event.jbutton, Events.JoyButtonEvent.Type.Down));
                        break;
                    case SDL_EventType.SDL_JOYDEVICEADDED:
                        JoyAddedEvent?.Invoke(this, new JoyDeviceAddedEvent(Event.jdevice));
                        break;
                    case SDL_EventType.SDL_JOYDEVICEREMOVED:
                        JoyRemovedEvent?.Invoke(this, new JoyDeviceRemovedEvent(Event.jdevice));
                        break;
                    case SDL_EventType.SDL_QUIT:
                        Quit = true;
                        break;
                    default: 
                        UnhandledEvent?.Invoke(this, new UnhandledEvent(Event));
                        break;
                }

            }
        }

        /// <summary>
        /// Set the flag to end the window main loop
        /// </summary>
        public void Exit()
        {
            Quit = true;
        }
        

        #endregion

        #region Overrides
        public override void Dispose()
        {
            base.Dispose();
            
            if (Handler == IntPtr.Zero)
                return;
            
            
            SDL_DestroyWindow(Handler);
            Handler = IntPtr.Zero;
        }
        
        #endregion
    }
}