using System;
using System.Runtime.InteropServices;

namespace SDL2.Types
{
    public class Rect
    {
        public SDL.SDL_Rect Inner;
        private IntPtr? Address;
        
        public static explicit operator SDL.SDL_Rect(Rect Struct)
        {
            if (Struct == IntPtr.Zero)
            {
                throw new NullReferenceException("The given struct is null");
            }
            return Struct.Inner;
        }
        public static implicit operator Rect(SDL.SDL_Rect Struct)
        {
            var NStruct = new Rect();
            NStruct.Inner = Struct;
            
            return NStruct;
        }
        
        public static implicit operator Rect(IntPtr Ptr)
        {
            if (Ptr == IntPtr.Zero)
            {
                return new Rect()
                {
                    Address = IntPtr.Zero
                };
            }
            
            var MSurface = new Rect();
            MSurface.Address = Ptr;
            MSurface.Inner = (SDL.SDL_Rect)Marshal.PtrToStructure(Ptr, typeof(SDL.SDL_Rect));
            return MSurface;
        }
			
        public static implicit operator IntPtr(Rect Data)
        {
            if (Data.Address == null)
            {
                int Size = Marshal.SizeOf(typeof(SDL.SDL_Rect));
                Data.Address = Marshal.AllocHGlobal(Size);
            }

            if (Data.Address == IntPtr.Zero)
                return IntPtr.Zero;
				
            Marshal.StructureToPtr(Data.Inner, Data.Address.Value, true);
            return Data.Address.Value;
        }
    }
}