using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Orbis {
   public static class Program {
   	public static void Main(){
   		sceSysUtilSendSystemNotificationWithText(222, "Hello world from C#\nResult" + Test.GetTwo());
   		while(true){
   		  continue;
   		}
   	}

  	[DllImport("/system/common/lib/libSceSysUtil.sprx")]
 	public static extern void sceSysUtilSendSystemNotificationWithText(int IconID, string Message);
   }
}

class Test {
	[MethodImplAttribute(MethodImplOptions.InternalCall)]
	public extern static int GetTwo();
}
