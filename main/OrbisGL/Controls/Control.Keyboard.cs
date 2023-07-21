using System.Linq;
using OrbisGL.Controls.Events;

namespace OrbisGL.Controls
{
    public partial class Control
    {
        /// <summary>
        /// An keyboard event propagated to all enabled controllers
        /// </summary>
        public event KeyboardEventDelegate OnKeyDown;

        /// <summary>
        /// An keyboard event propagated to all enabled controllers
        /// </summary>
        public event KeyboardEventDelegate OnKeyUp;

        internal void ProcessKeyDown(object Sender, KeyboardEventArgs Args)
        {
            PropagateAll((Ctrl, e) =>
            {
                Ctrl.OnKeyDown?.Invoke(Ctrl, (KeyboardEventArgs)e);
            }, Args);
        }
        internal void ProcessKeyUp(object Sender, KeyboardEventArgs Args)
        {
            PropagateAll((Ctrl, e) =>
            {
                Ctrl.OnKeyUp?.Invoke(Ctrl, (KeyboardEventArgs)e);
            }, Args);
        }
    }
}