using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using Orbis.Internals;
using OrbisGL.Controls;
using OrbisGL.Controls.Events;
using OrbisGL.GL2D;
using OrbisGL.Input;
using OrbisGL.Input.Dualshock;
using SharpGLES;

namespace OrbisGL.GL
{
    public class Application : IRenderable
    {

        public static bool PhysicalKeyboardAvailable { get; set; }

        public int UserID = -1;
        public IMouse MouseDriver { get; set; } = null;

        private IntPtr Handler = IntPtr.Zero;
        public int Width { get; private set; }
        public int Height { get; private set; }
        
        public RGBColor ClearColor = null;
        private EGLDisplay GLDisplay;
        public readonly int FrameDelay = 0;

        public IEnumerable<Control> Controllers => Objects.Where(x => x is Control).Cast<Control>();
        public readonly IList<IRenderable> Objects = new List<IRenderable>();

        /// <summary>
        /// Create an OpenGL ES 2 Environment 
        /// </summary>
        /// <param name="Width">Sets the rendering Width</param>
        /// <param name="Height">Sets the rendering Height</param>
        /// <param name="FramePerSecond">Set the default frame delay</param>
        /// <param name="Handler">(WINDOWS ONLY) Set the Control Render Handler</param>
        public Application(int Width, int Height, int FramePerSecond, IntPtr? Handler = null)
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
            
            Kernel.LoadStartModule("libSceMbus.sprx");//For Mouse and Dualshock Support
#endif

            this.Width = Width;
            this.Height = Height;
        }


        public void Run() => Run(CancellationToken.None);
        public virtual void Run(CancellationToken Abort)
        {
#if !ORBIS
            GLDisplay = new EGLDisplay(Handler, Width, Height);
#else
            UserService.Initialize();
            UserService.HideSplashScreen();
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
                    Debugger.Log(1, "WARN", "Frame Loop too Late\n");
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

                ProcessEvents(CurrentTick);

#if ORBIS
                Draw(CurrentTick);
#else
                Draw(CurrentTick/10);
#endif
                GLDisplay.SwapBuffers();

            }
        }
        public GamepadListener Dualshock { get; private set; } = null;

        private bool LeftAnalogCentered = true;
        private bool DualshockEnabled = false;
        public void EnableDualshock(DualshockSettings Settings)
        {
            if (DualshockEnabled)
                return;

#if ORBIS
            if (UserID == -1)
            {
                UserService.Initialize();
                UserService.GetInitialUser(out UserID);
            }

            Dualshock = new GamepadListener(UserID);

            Dualshock.OnButtonDown += (sender, args) =>
            {
                foreach (var Child in Controllers)
                {
                    Child.ProcessButtonDown(sender, args);
                }
            };

            Dualshock.OnButtonUp += (sender, args) =>
            {
                foreach (var Child in Controllers)
                {
                    Child.ProcessButtonUp(sender, args);
                }
            };

            if (Settings.LeftAnalogAsPad)
            {
                Dualshock.OnLeftStickMove += (sender, args) =>
                {
                    var Offset = args.CurrentOffset;

                    bool XCentered = Offset.X <= 0.2 && Offset.X >= -0.2;
                    bool YCentered = Offset.Y <= 0.2 && Offset.Y >= -0.2;

                    bool Centered = XCentered && YCentered;

                    if (!LeftAnalogCentered && Centered)
                    {
                        LeftAnalogCentered = Centered;
                        args.Handled = true;
                        return;
                    }

                    if (!Centered && LeftAnalogCentered)
                    {
                        LeftAnalogCentered = Centered;
                        args.Handled = true;

                        EmulatePad(Offset, XCentered, YCentered);
                        return;
                    }
                };
            }

            if (Settings.Mouse == VirtualMouse.Touchpad)
            {
                MouseDriver = new TouchpadMouse(Dualshock);
            }

            DualshockEnabled = true;
#endif
        }

        private void EmulatePad(Vector2 Offset, bool XCentered, bool YCentered)
        {
            ButtonEventArgs EventDown = null, EventUp = null;

            if (!XCentered && !YCentered)
            {
                if (Math.Abs(Offset.X) > Math.Abs(Offset.Y))
                    YCentered = true;
                else
                    XCentered = true;
            }

            if (Offset.X <= -0.2 && YCentered)
            {
                EventDown = new ButtonEventArgs(OrbisPadButton.Left);
                EventUp = new ButtonEventArgs(OrbisPadButton.Left);
            }

            if (Offset.X >= 0.2 && YCentered)
            {
                EventDown = new ButtonEventArgs(OrbisPadButton.Right);
                EventUp = new ButtonEventArgs(OrbisPadButton.Right);
            }

            if (Offset.Y >= 0.2 && XCentered)
            {
                EventDown = new ButtonEventArgs(OrbisPadButton.Up);
                EventUp = new ButtonEventArgs(OrbisPadButton.Up);
            }
            if (Offset.Y <= -0.2 && XCentered)
            {
                EventDown = new ButtonEventArgs(OrbisPadButton.Down);
                EventUp = new ButtonEventArgs(OrbisPadButton.Down);
            }

            foreach (var Child in Controllers)
            {
                Child.ProcessButtonDown(Child, EventDown);
                Child.ProcessButtonUp(Child, EventUp);
            }
        }

        public IKeyboard KeyboardDriver;

        private bool KeyboardEnabled = false;

        public void EnableKeyboard()
        {
            if (KeyboardEnabled)
                return;
#if ORBIS

            if (UserID == -1)
            {
                UserService.Initialize();
                UserService.GetInitialUser(out UserID);
            }

            KeyboardDriver = new OrbisKeyboard();
#endif

            KeyboardEnabled = true;

            KeyboardDriver.Initialize(UserID);

            KeyboardDriver.OnKeyDown += (sender, args) =>
            {
                foreach (var Child in Controllers)
                {
                    Child.ProcessKeyDown(sender, args);
                }
            };

            KeyboardDriver.OnKeyUp += (sender, args) =>
            {
                foreach (var Child in Controllers)
                {
                    Child.ProcessKeyUp(sender, args);
                }
            };
        }

        public Vector2 CursorPosition { get; private set; } = Vector2.Zero;
        public MouseButtons MousePressedButtons { get; private set; }

        IMouse InitializedMouse = null;

        private void ProcessEvents(long Tick)
        {
            if (MouseDriver != null)
            {
                if (InitializedMouse != MouseDriver)
                {
                    EnableMouse();
                }

                MouseDriver.RefreshData(Tick);

                var CurrentPosition = MouseDriver.GetPosition();
                bool Moved = CursorPosition != CurrentPosition;

                if (Moved)
                {
                    CursorPosition = CurrentPosition;
                    foreach (var Child in Controllers)
                    {
                        if (Child.AbsoluteRectangle.IsInBounds(CurrentPosition))
                        {
                            Child.ProcessMouseMove(CurrentPosition);
                            break;
                        }
                    }
                }

                var CurrentButtons = MouseDriver.GetMouseButtons();
                bool Changed = CurrentButtons != MousePressedButtons;

                if (Changed)
                {
                    var OldButtons = MousePressedButtons;
                    MousePressedButtons = CurrentButtons;

                    foreach (var Child in Controllers)
                    {
                        if (Child.AbsoluteRectangle.IsInBounds(CurrentPosition))
                        {
                            Child.ProcessMouseButtons(OldButtons, CurrentButtons);
                        }
                    }
                }
            }

#if ORBIS
            KeyboardDriver?.RefreshData();
            Dualshock?.RefreshData();
#endif
        }

        public void EnableMouse()
        {
#if ORBIS
            if (UserID == -1)
            {
                UserService.Initialize();
                UserService.GetInitialUser(out UserID);
            }
#endif

            InitializedMouse = MouseDriver;
            MouseDriver.Initialize(UserID);
            
            Control.Cursor = new Cursor()
            {
                ContourWidth = Coordinates2D.Height / 720f,
                Height = (int)((Coordinates2D.Height / 720f) * 19),
                Visible = false
            };
            
            Control.Cursor.RefreshVertex();
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
        bool GLReady = false;
        public virtual void Draw(long Tick)
        {
            if (ClearColor != null)
            {
                GLES20.ClearColor(ClearColor.RedF, ClearColor.GreenF, ClearColor.BlueF, 1);
                GLES20.Clear(GLES20.GL_COLOR_BUFFER_BIT);
            }

            if (!GLReady)
            {
                //GLES20.BlendEquation(GLES20.GL_FUNC_ADD);

                //GLES20.BlendFunc(GLES20.GL_SRC_ALPHA, GLES20.GL_ONE_MINUS_SRC_ALPHA);
                GLES20.BlendFunc(GLES20.GL_ONE, GLES20.GL_ONE_MINUS_SRC_ALPHA);
                GLES20.Enable(GLES20.GL_BLEND);

                //GLES20.Disable(GLES20.GL_CULL_FACE);
                GLReady = true;
            }



            foreach (var Object in Objects)
            {
                if (Object is Control Controller)
                    Controller.FlushMouseEvents(Tick);

                Object.Draw(Tick);
            }

            Control.Selector.Draw(Tick);
            Control.Cursor.Draw(Tick);
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