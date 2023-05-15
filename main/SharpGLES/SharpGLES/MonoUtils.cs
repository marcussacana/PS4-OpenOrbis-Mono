using System;
using System.Runtime.InteropServices;

namespace SharpGLES
{
    public static class MonoUtils
    {
        /// <summary>
        /// Get the Module Base Address by Name
        /// </summary>
        /// <param name="String">The Module name (With extension)</param>
        /// <param name="Base">The Module Base Address</param>
        /// <param name="Size">The Module Section Size</param>
        /// <returns>When failed returns false, otherwise returns true</returns>
        [DllImport("libMonoUtils.sprx", EntryPoint = "sceGetModuleBase", CharSet = CharSet.Ansi)]
        public static extern bool GetModuleBase(string Name, out IntPtr Base, out UIntPtr Size);//sceJailbreak
        
        /// <summary>
        /// Jailbreak the Process for the Given AuthID and return a simple status result
        /// </summary>
        /// <param name="AuthID">AuthID Credential for Jailbreak, when 0 selected the default credentials</param>
        /// <returns>When fails return -1, When already jailbroken return 0, if not, jailbreak it and returns 1</returns>
        [DllImport("libMonoUtils.sprx", EntryPoint = "jailbreak")]
        public static extern int Jailbreak(ulong AuthID);
        
        /// <summary>
        /// Jailbreak the Process for the Given AuthID and return a simple status result
        /// </summary>
        /// <returns>When fails return -1, When already jailed return 0, if not, re-jail it and returns 1</returns>
        [DllImport("libMonoUtils.sprx", EntryPoint = "unjailbreak")]
        public static extern int Unjailbreak();
        
        /// <summary>
        /// Check if the process is jailbroken
        /// </summary>
        /// <returns>If is jailbroken return true, otherwise false</returns>
        [DllImport("libMonoUtils.sprx", EntryPoint = "IsJailbroken")]
        public static extern bool IsJailbroken();

        public static void Log(string Message, params object[] Format)
        {
            string FinalMessage = string.Format(Message + "\n", Format);
            KernelDebugOutText(0, FinalMessage);
        }

        [DllImport("libkernel.sprx", EntryPoint = "sceKernelDebugOutText", CharSet = CharSet.Ansi)]
        static extern int KernelDebugOutText(int DBG_CHANNEL, string text);
        
        public static int LoadStartModule(string Name, out int result) => LoadStartModule(Name, UIntPtr.Zero, IntPtr.Zero, 0, IntPtr.Zero, out result);
        
        [DllImport("libkernel.sprx", EntryPoint = "sceKernelLoadStartModule", CharSet = CharSet.Ansi)]
        public static extern int LoadStartModule(string name, UIntPtr argc, IntPtr argv, uint flags, IntPtr KernelLMOptions, out int result);
    }
}