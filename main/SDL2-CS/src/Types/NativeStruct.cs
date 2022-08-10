using System;
using System.Runtime.InteropServices;
using static SDL2.SDL;

namespace SDL2.Types
{
    public class NativeStruct<T> : IDisposable where T : struct  
    {
        public T Inner;
        private IntPtr? Address;

        public NativeStruct(object dummy = null)
        {
            Inner = new T();
            Address = null;
        }

        public void Dispose()
        {
            if (Address == IntPtr.Zero || Address == null)
                return;
				
            Marshal.DestroyStructure(Address.Value, typeof(T));
        }

        public static explicit operator T(NativeStruct<T> Struct)
        {
            if (Struct == IntPtr.Zero)
            {
                throw new NullReferenceException("The given struct is null");
            }
            return Struct.Inner;
        }
        public static implicit operator NativeStruct<T>(T Struct)
        {
            var NStruct = new NativeStruct<T>();
            NStruct.Inner = Struct;
            
            return NStruct;
        }
        
        public static implicit operator NativeStruct<T>(IntPtr Ptr)
        {
            if (Ptr == IntPtr.Zero)
            {
                return new NativeStruct<T>()
                {
                    Address = IntPtr.Zero
                };
            }
            
            var MSurface = new NativeStruct<T>();
            MSurface.Address = Ptr;
            MSurface.Inner = (T)Marshal.PtrToStructure(Ptr, typeof(T));
            return MSurface;
        }
			
        public static implicit operator IntPtr(NativeStruct<T> Data)
        {
            if (Data.Address == null)
            {
                int Size = Marshal.SizeOf(typeof(T));
                Data.Address = Marshal.AllocHGlobal(Size);
            }

            if (Data.Address == IntPtr.Zero)
                return IntPtr.Zero;
				
            Marshal.StructureToPtr(Data.Inner, Data.Address.Value, true);
            return Data.Address.Value;
        }
    }
}