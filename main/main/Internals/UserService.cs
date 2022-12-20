using Orbis.String;
using System.Runtime.CompilerServices;

namespace Orbis.Internals
{
    internal class UserService
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool HideSplashScreen();


        public unsafe static bool LoadExec(string Path, params string[] Args)
        {
            //Not sure how to generate the pArgs, no documentation at all
            //therefore this code is just a guess

            if (Args.Length > 0)
            {
                void*[] pArgs = new void*[Args.Length + 1];

                for (int i = 0; i < Args.Length; i++)
                    pArgs[i] = (CString)Args[i];

                pArgs[Args.Length] = null;

                fixed (void* ppArgs = &pArgs[0])
                {
                    return NativeLoadExec((CString)Path, ppArgs);
                }
            }

            return NativeLoadExec((CString)Path, null);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        unsafe static extern bool NativeLoadExec(void* Path, void* Args);
    }
}
