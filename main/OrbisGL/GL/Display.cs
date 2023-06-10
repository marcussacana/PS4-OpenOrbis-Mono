using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using Orbis.Internals;
using OrbisGL.Controls;
using OrbisGL.GL2D;
using OrbisGL.Input;
using SharpGLES;

namespace OrbisGL.GL
{
    public class Display : IRenderable
    {

        public int UserID = -1;
        public IMouse MouseDriver { get; set; } = null;

        private IntPtr Handler = IntPtr.Zero;
        public int Width { get; private set; }
        public int Height { get; private set; }
        
        public RGBColor ClearColor = null;
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

            GL2D.Coordinates2D.SetSize(Width, Height);

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

            GLES20.Viewport(0, 0, GLDisplay.Width, GLDisplay.Height);

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
#if DEBUG
                if (CurrentTick > NextDrawTick)
                    Debugger.Log(1, "WARN", "Frame Loop too Late");
#endif
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

                ProcessEvents();

#if ORBIS
                Draw(CurrentTick);
#else
                Draw(CurrentTick/10);
#endif
                GLDisplay.SwapBuffers();

            }
        }


        public Vector2 CursorPosition { get; private set; } = Vector2.Zero;
        public MouseButtons MousePressedButtons { get; private set; }

        IMouse InitializedMouse = null;

        private void ProcessEvents()
        {
            if (MouseDriver != null)
            {
                if (InitializedMouse != MouseDriver)
                {
#if ORBIS
                    if (UserID == -1)
                        UserID = Native.UserService.GetForegroundUser();
#endif

                    InitializedMouse = MouseDriver;
                    MouseDriver.Initialize(UserID);
                }

                MouseDriver.RefreshData();

                var CurrentPosition = MouseDriver.GetPosition();
                bool Moved = CursorPosition != CurrentPosition;

                if (Moved)
                {
                    CursorPosition = CurrentPosition;
                    foreach (var Child in Objects)
                    {
                        if (Child is Control Ctrl)
                        {
                            if (Ctrl.Rectangle.IsInBounds(CurrentPosition))
                            {
                                Ctrl.ProcessMouseMove(CurrentPosition);
                                break;
                            }
                            else
                            {

                            }
                        }
                    }
                }

                var CurrentButtons = MouseDriver.GetMouseButtons();
                bool Changed = CurrentButtons != MousePressedButtons;

                if (Changed)
                {
                    var OldButtons = MousePressedButtons;
                    MousePressedButtons = CurrentButtons;

                    foreach (var Child in Objects)
                    {
                        if (Child is Control Ctrl)
                        {
                            if (Ctrl.Rectangle.IsInBounds(CurrentPosition))
                            {
                                Ctrl.ProcessMouseButtons(OldButtons, CurrentButtons);
                                break;
                            }
                        }
                    }
                }
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

            ProcessEvents();
            Draw(DateTime.Now.Ticks/10);
        }
#endif

        public virtual void Draw(long Tick)
        {
            if (ClearColor != null)
            {
                GLES20.ClearColor(ClearColor.RedF, ClearColor.GreenF, ClearColor.BlueF, 1);
                GLES20.Clear(GLES20.GL_COLOR_BUFFER_BIT);
            }


            //GLES20.BlendEquation(GLES20.GL_FUNC_ADD);

            //GLES20.BlendFunc(GLES20.GL_SRC_ALPHA, GLES20.GL_ONE_MINUS_SRC_ALPHA);
            GLES20.BlendFunc(GLES20.GL_ONE, GLES20.GL_ONE_MINUS_SRC_ALPHA);

            //GLES20.Disable(GLES20.GL_CULL_FACE);
            GLES20.Enable(GLES20.GL_BLEND);

            foreach (var Object in  Objects)
            {

                Object.Draw(Tick);
                GLES20.Flush();
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