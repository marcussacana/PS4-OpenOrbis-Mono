using System;
using System.Dynamic;

namespace SDL2.Exceptions
{
    public class SDLException : Exception
    {
        public int ErrorCode { get; }
        public string Message { get; }

        internal SDLException()
        {
            Message = SDL.SDL_GetError();
            SDL.SDL_ClearError();
            ErrorCode = -1;
        }
        
        internal SDLException(string Message)
        {
            this.ErrorCode = -1;
            this.Message = SDL.SDL_GetError();
            SDL.SDL_ClearError();
        }
        
        internal SDLException(int ErrorCode)
        {
            this.ErrorCode = ErrorCode;
            Message = SDL.SDL_GetError();
            SDL.SDL_ClearError();
        }

        public SDLException(string Message, int ErrorCode)
        {
            this.ErrorCode = ErrorCode;
            this.Message = Message;
        }
    }
}