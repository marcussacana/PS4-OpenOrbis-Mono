using OrbisGL.Controls.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Controls
{
    public partial class Control
    {
        /// <summary>
        /// An gamepad event propagated to all enabled controllers
        /// </summary>
        public event ButtonEventHandler OnButtonDown;

        /// <summary>
        /// An gamepad event propagated to all enabled controllers
        /// </summary>
        public event ButtonEventHandler OnButtonUp;


        /// <summary>
        /// An gamepad event propagated to the focused controllers
        /// </summary>
        public event ButtonEventHandler OnButtonPressed;

        static Control LastButtonController;

        internal void ProcessButtonDown(object Sender, ButtonEventArgs Args)
        {
            PropagateAll((Ctrl, e) =>
            {
                Ctrl.OnButtonDown?.Invoke(Ctrl, (ButtonEventArgs)e);
            }, Args);

            LastButtonController = FocusedControl;
        }

        internal void ProcessButtonUp(object Sender, ButtonEventArgs Args)
        {
            PropagateAll((Ctrl, e) =>
            {
                Ctrl.OnButtonUp?.Invoke(Ctrl, (ButtonEventArgs)e);
            }, Args);

            if (LastButtonController == FocusedControl && FocusedControl != null)
            {
                Args.Handled = false;

                if (EnableSelector)
                {
                    switch (Args.Button)
                    {
                        case OrbisPadButton.Up:
                        case OrbisPadButton.Left:
                        case OrbisPadButton.Right:
                        case OrbisPadButton.Down:
                            LastButtonController.ProcessSelection(Args.Button);
                            Args.Handled = true;
                            return;
                    }
                }

                LastButtonController.PropagateUp((Ctrl, e) => 
                {
                    Ctrl.OnButtonPressed?.Invoke(Ctrl, (ButtonEventArgs)e);
                }, Args);
            } 
            else if (EnableSelector && FocusedControl == null)
            {
                switch (Args.Button)
                {
                    case OrbisPadButton.Up:
                    case OrbisPadButton.Left:
                    case OrbisPadButton.Right:
                    case OrbisPadButton.Down:
                        RootControl.Focus();
                        Args.Handled = true;
                        return;
                }
            }
        }
    }
}
