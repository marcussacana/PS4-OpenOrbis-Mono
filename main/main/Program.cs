using SharpGLES;
using System;
using System.Reflection;
using System.Security.Policy;
using System.Security.Principal;

namespace Orbis
{

    public static class Program
    {
        public static void Main(string[] args)
        {
            var Display = new MainDisplay();
            Display.Run();
        }
      
    }

}