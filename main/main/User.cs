using System.Runtime.InteropServices;
using Orbis.String;

namespace Orbis
{
    public unsafe class User
    {
        public static void Notify(string Message)
        {
            sceSysUtilSendSystemNotificationWithText(222, Message);
        }
      
        [DllImport("/system/common/lib/libSceSysUtil.sprx", CharSet = CharSet.Ansi)]
        static extern void sceSysUtilSendSystemNotificationWithText(int IconID, string Message);
    }
}