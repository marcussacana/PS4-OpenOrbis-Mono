using System.Linq;
using OrbisGL.Controls.Events;

namespace OrbisGL.Controls
{
    public partial class Control
    {
        public event KeyboardEventDelegate OnKeyDown;
        public event KeyboardEventDelegate OnKeyUp;

        internal void ProcessKeyDown(object Sender, KeyboardEventArgs Args)
        {
            PropagateAll((Ctrl, e) =>
            {
                Ctrl.OnKeyDown?.Invoke(this, (KeyboardEventArgs)e);
            }, Args);
            return;
        }
        internal void ProcessKeyUp(object Sender, KeyboardEventArgs Args)
        {
            PropagateAll((Ctrl, e) =>
            {
                Ctrl.OnKeyUp?.Invoke(this, (KeyboardEventArgs)e);
            }, Args);
        }
    }
}