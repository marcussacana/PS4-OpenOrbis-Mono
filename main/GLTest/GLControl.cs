using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OrbisGL.Input;
using Application = OrbisGL.GL.Application;
using MButtons = OrbisGL.MouseButtons;
using System.Numerics;

namespace GLTest
{

    public partial class GLControl : Panel
    {
#if !ORBIS
        public Application GLApplication;
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
            GLApplication?.DrawOnce();
            GLApplication?.SwapBuffers();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            GLApplication = new Application(Width, Height, 30, Handle);

            GLApplication.MouseDriver = new GenericMouse(() =>
            {
                var Pos = Cursor.Position;
                var CPos = PointToClient(Pos);
                return new Vector2(CPos.X, CPos.Y);
            }, () =>
            {
                MButtons Buttons = 0;
                if (MouseButtons.HasFlag(MouseButtons.Left))
                    Buttons |= MButtons.Left;
                if (MouseButtons.HasFlag(MouseButtons.Right))
                    Buttons |= MButtons.Right;
                if (MouseButtons.HasFlag(MouseButtons.Middle))
                    Buttons |= MButtons.Middle;

                return Buttons;
            });
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            GLApplication.Dispose();

            base.OnHandleDestroyed(e);
        }
#endif
    }
}