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

        private readonly IList<IRenderable> Objects = new List<IRenderable>(); 
        
        public Display(uint Width, uint Height, int FramePerSecond)
        {
#if ORBIS
            FrameDelay = SCE_SECOND / FramePerSecond;
#else
            FrameDelay = 1000 / FramePerSecond;
#endif
            
            GL2D.Coordinates2D.Width = Width;
            GL2D.Coordinates2D.Height = Height;
            
            GL2D.Coordinates2D.XUnity = GL2D.Coordinates2D.XToPoint(1);
            GL2D.Coordinates2D.YUnity = GL2D.Coordinates2D.YToPoint(1);
            
            GLDisplay = new EGLDisplay(IntPtr.Zero, Width, Height);
        }


        public void Run() => Run(CancellationToken.None);
        public virtual void Run(CancellationToken Abort)
        {
            long LastDrawTick = 0;
            while (!Abort.IsCancellationRequested)
            {
#if ORBIS
                long CurrentTick = 0;
                sceRtcGetCurrentTick(out CurrentTick);

                long NextDrawTick = LastDrawTick + FrameDelay;

                if (NextDrawTick > CurrentTick)
                {
                    uint ReamingTicks = (uint)(NextDrawTick - CurrentTick);
                    sceKernelUsleep(ReamingTicks);
                }
#else
                long CurrentTick = DateTime.UtcNow.Ticks;
                
                long NextDrawTick = LastDrawTick + FrameDelay;
                
                if (NextDrawTick > CurrentTick)
                {
                    int ReamingTicks = (int)(NextDrawTick - CurrentTick);
                    Thread.Sleep(ReamingTicks);
                }
#endif

                LastDrawTick = CurrentTick;
                Draw();
                
                GLDisplay.SwapBuffers();
            }
        }

        [DllImport("libkernel.sprx")]
        static extern void sceKernelUsleep(uint MicroSecond);
        
        [DllImport("libSceRtc.srpx")]
        static extern int sceRtcGetCurrentTick(out long CurrentTick);


        public virtual void Draw()
        {
            foreach (var Object in  Objects)
            {
                Object.Draw();
            }
        }
    }
}