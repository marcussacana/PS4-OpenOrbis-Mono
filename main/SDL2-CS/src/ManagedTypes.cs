using System;
using System.Runtime.InteropServices;
using static SDL2.SDL;

namespace SDL2
{
    public class Surface : IDisposable
    {
        public SDL_Surface Inner;
        private IntPtr? Address;

        public Surface()
        {
            Inner = new SDL_Surface();
            Address = null;
        }
			
        ~Surface()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (Address == IntPtr.Zero || Address == null)
                return;
				
            Marshal.DestroyStructure(Address.Value, typeof(SDL_Surface));
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
            MSurface.Inner = (SDL_Surface)Marshal.PtrToStructure(Ptr, typeof(SDL_Surface));
            return MSurface;
        }
			
        public static implicit operator IntPtr(Surface Data)
        {
            if (Data.Address == null)
            {
                int Size = Marshal.SizeOf(typeof(SDL_Surface));
                Data.Address = Marshal.AllocHGlobal(Size);
            }

            if (Data.Address == IntPtr.Zero)
                return IntPtr.Zero;
				
            Marshal.StructureToPtr(Data.Inner, Data.Address.Value, true);
            return Data.Address.Value;
        }
    }
    public class Rect : IDisposable
    {
        public SDL_Rect Inner;
        private IntPtr? Address;

        public Rect()
        {
            Inner = new SDL_Rect();
            Address = null;
        }
			
        ~Rect()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (Address == IntPtr.Zero || Address == null)
                return;
				
            Marshal.DestroyStructure(Address.Value, typeof(SDL_Rect));
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
            MSurface.Inner = (SDL_Rect)Marshal.PtrToStructure(Ptr, typeof(SDL_Rect));
            return MSurface;
        }
			
        public static implicit operator IntPtr(Rect Data)
        {
            if (Data.Address == null)
            {
                int Size = Marshal.SizeOf(typeof(SDL_Rect));
                Data.Address = Marshal.AllocHGlobal(Size);
            }

            if (Data.Address == IntPtr.Zero)
                return IntPtr.Zero;
				
            Marshal.StructureToPtr(Data.Inner, Data.Address.Value, true);
            return Data.Address.Value;
        }
    }

    public struct Size
    {
        public int Width;
        public int Height;
    }
}