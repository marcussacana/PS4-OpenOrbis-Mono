using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using OrbisGL.GL;
using Timer = System.Windows.Forms.Timer;

namespace GLTest
{

    public partial class GLControl : Panel
    {
#if !ORBIS
        public Display GLDisplay;
        public GLControl(int Width, int Height)
        {
            Size = new Size(Width, Height);

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        }

        const int WM_CREATE = 0x0001;
        const int WM_TIMER = 0x0113;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WM_CREATE:
                    SetTimer(Handle, 1, 20, IntPtr.Zero);
                    break;
                case WM_TIMER:
                    Invalidate();
                    break;
            }
        }

        [DllImport("user32.dll")]
        static extern UIntPtr SetTimer(IntPtr hWnd, int nIDEvent, uint uElapse, IntPtr lpTimerFunc);

        protected override void OnPaint(PaintEventArgs e)
        {
            GLDisplay?.DrawOnce();
            GLDisplay?.SwapBuffers();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            GLDisplay = new Display(Width, Height, 30, Handle);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            GLDisplay.Dispose();

            base.OnHandleDestroyed(e);
        }
#endif
    }
}