using System;
using System.Runtime.InteropServices;

namespace Orbis {
   public static class Program {
   	public static void Main(){
   		sceSysUtilSendSystemNotificationWithText(222, "Hello world from C#");
   		while(true){
   		  continue;
   		}
   	}

  	[DllImport("/system/common/lib/libSceSysUtil.sprx")]
 	public static extern void sceSysUtilSendSystemNotificationWithText(int IconID, string Message);
   }
}
