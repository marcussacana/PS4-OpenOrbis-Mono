using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using SharpGLES;

namespace OrbisGL.GL
{
    public class Display : IRenderable
    {
        private IntPtr Handler = IntPtr.Zero;
        public int Width { get; private set; }
        public int Height { get; private set; }
        
        public RGBColor ClearColor = RGBColor.Black;
        private EGLDisplay GLDisplay;
        public readonly int FrameDelay = 0;

        public readonly IList<IRenderable> Objects = new List<IRenderable>();

        /// <summary>
        /// Create an OpenGL ES 2 Environment 
        /// </summary>
        /// <param name="Width">Sets the rendering Width</param>
        /// <param name="Height">Sets the rendering Height</param>
        /// <param name="FramePerSecond">Set the default frame delay</param>
        /// <param name="Handler">(WINDOWS ONLY) Set the Control Render Handler</param>
        public Display(int Width, int Height, int FramePerSecond, IntPtr? Handler = null)
        {
#if ORBIS
            FrameDelay = Constants.SCE_SECOND / FramePerSecond;
#else
            FrameDelay = 1000 / FramePerSecond;
#endif
            
            GL2D.Coordinates2D.Width = this.Width = Width;
            GL2D.Coordinates2D.Height = this.Height = Height;
            
            GL2D.Coordinates2D.XUnity = Math.Abs(-1 - GL2D.Coordinates2D.XToPoint(1));
            GL2D.Coordinates2D.YUnity = Math.Abs(-1 - GL2D.Coordinates2D.YToPoint(1));

            this.Handler = Handler ?? IntPtr.Zero;
            
#if ORBIS
            GLDisplay = new EGLDisplay(IntPtr.Zero, Width, Height);
#endif
        }


        public void Run() => Run(CancellationToken.None);
        public virtual void Run(CancellationToken Abort)
        {
#if !ORBIS
            GLDisplay = new EGLDisplay(Handler, Width, Height);
#endif
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

#if ORBIS
        [DllImport("libkernel.sprx")]
        static extern void sceKernelUsleep(uint MicroSecond);
        
        [DllImport("libSceRtc.srpx")]
        static extern int sceRtcGetCurrentTick(out long CurrentTick);
#else
        public void DrawOnce()
        {
            if (GLDisplay == null)
                GLDisplay = new EGLDisplay(Handler, Width, Height);
            Draw();
        }
#endif

        public virtual void Draw()
        {
            GLES20.Viewport(0,0, GLDisplay.Width, GLDisplay.Height);
            GLES20.ClearColor(ClearColor.RedF, ClearColor.GreenF, ClearColor.BlueF, 1);
            GLES20.Clear(GLES20.GL_COLOR_BUFFER_BIT);
            
            foreach (var Object in  Objects)
            {
                Object.Draw();
            }
        }
        
#if !ORBIS
        public void SwapBuffers() => GLDisplay?.SwapBuffers();
#endif

        public void Dispose()
        {
            foreach (var Object in Objects)
            {
                Object.Dispose();
            }
            
            GLDisplay?.Dispose();
        }
    }
}