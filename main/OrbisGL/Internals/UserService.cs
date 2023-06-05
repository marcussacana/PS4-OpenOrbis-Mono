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
                    pArgs[i] = Kernel.AllocString(Args[i]);

                pArgs[Args.Length] = null;

                fixed (void* ppArgs = &pArgs[0])
                {
                    var pStr = Kernel.AllocString(Path);
                    var Rst = NativeLoadExec(pStr, ppArgs);
                    Kernel.free(pStr);
                    foreach (var Arg in pArgs)
                    {
                        Kernel.free(Arg);
                    }
                    return Rst;
                }
            }

            var pPath = Kernel.AllocString(Path);
            var Result = NativeLoadExec(pPath, null);
            Kernel.free(pPath);
            return Result;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        unsafe static extern bool NativeLoadExec(void* Path, void* Args);
    }
}
