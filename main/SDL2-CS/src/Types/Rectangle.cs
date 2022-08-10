using System;
using System.Runtime.InteropServices;

namespace SDL2.Types
{
    public class Surface
    {
        public SDL.SDL_Surface Inner;
        private IntPtr? Address;
        
        public static explicit operator SDL.SDL_Surface(Surface Struct)
        {
            if (Struct == IntPtr.Zero)
            {
                throw new NullReferenceException("The given struct is null");
            }
            return Struct.Inner;
        }
        public static implicit operator Surface(SDL.SDL_Surface Struct)
        {
            var NStruct = new Surface();
            NStruct.Inner = Struct;
            
            return NStruct;
        }
        
        public static implicit operator Surface(IntPtr Ptr)
        {
            if (Ptr == IntPtr.Zero)
            {
                return new Surface()
                {
                    Address = IntPtr.Zero
                };
            }
            
            var MSurface = new Surface();
            MSurface.Address = Ptr;
            MSurface.Inner = (SDL.SDL_Surface)Marshal.PtrToStructure(Ptr, typeof(SDL.SDL_Surface));
            return MSurface;
        }
			
        public static implicit operator IntPtr(Surface Data)
        {
            if (Data.Address == null)
            {
                int Size = Marshal.SizeOf(typeof(SDL.SDL_Surface));
                Data.Address = Marshal.AllocHGlobal(Size);
            }

            if (Data.Address == IntPtr.Zero)
                return IntPtr.Zero;
				
            Marshal.StructureToPtr(Data.Inner, Data.Address.Value, true);
            return Data.Address.Value;
        }
    }
}