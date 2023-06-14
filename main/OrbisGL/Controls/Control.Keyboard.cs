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
            if (!Focused)
                return;

            if (Children.Any(x => x.Focused))
            {
                Children.Single(x => x.Focused).PropagateUp((Ctrl, e) =>
                {
                    Ctrl.OnKeyDown?.Invoke(this, (KeyboardEventArgs)e);
                }, Args);
                return;
            }

            OnKeyDown?.Invoke(this, Args);
        }
        
        internal void ProcessKeyUp(object Sender, KeyboardEventArgs Args)
        {
            if (!Focused)
                return;

            if (Children.Any(x => x.Focused))
            {
                Children.Single(x => x.Focused).PropagateUp((Ctrl, e) =>
                {
                    Ctrl.OnKeyUp?.Invoke(this, (KeyboardEventArgs)e);
                }, Args);
                return;
            }

            OnKeyUp?.Invoke(this, Args);
        }
    }
}