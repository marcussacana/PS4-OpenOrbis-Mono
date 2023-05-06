using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using SharpGLES;
using static OrbisGL.Constants;

namespace OrbisGL.GL
{
    public class Display : IRenderable
    {
        private EGLDisplay GLDisplay;
        public readonly int FrameDelay = 0;

        private IList<IRenderable> Objects = new List<IRenderable>(); 
        
        public Display(uint Width, uint Height, int FramePerSecond)
        {
            FrameDelay = SCE_SECOND / FramePerSecond;
            GLDisplay = new EGLDisplay(IntPtr.Zero, Width, Height);
        }


        public void Run() => Run(CancellationToken.None);
        public virtual void Run(CancellationToken Abort)
        {
            while (!Abort.IsCancellationRequested)
            {
                
                
                GLDisplay.SwapBuffers();
            }
        }

        [DllImport("libkernel.sprx")]
        public static extern void sceKernelUsleep(uint MicroSecond);

        public virtual void Draw() { }
    }
}