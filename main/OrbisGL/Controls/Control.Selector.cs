using OrbisGL.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Controls
{
    public partial class Control
    {

        internal static bool EnableSelector = true;
        internal static Selector Selector = new Selector();

        /// <summary>
        /// Sets up the directional PAD shortcut focus swapping between controls.
        /// When the user presses directional buttons (up, down, left, right) on the controller,
        /// the focus will move to the adjacent control in that direction.
        /// </summary>
        public ControlLink Links = default;

        private Control LastChildFocus = null;

        public void SetAsSelected()
        {
            if (!EnableSelector)
                return;
            
            Selector.Select(this);
        }

        private void ProcessSelection(OrbisPadButton Button)
        {
            switch (Button)
            {
                 case OrbisPadButton.Up:
                     if (Links.Up != null)
                     {
                         if (Links.Up.Focus())
                             return;
                     }
                     break;
                 case OrbisPadButton.Down:
                     if (Links.Down != null) {
                         if (Links.Down.Focus())
                            return;
                     }
                     break;
                 case OrbisPadButton.Left:
                     if (Links.Left != null)
                     {
                         if (Links.Left.Focus())
                             return;
                     }
                     break;
                 case OrbisPadButton.Right:
                     if (Links.Right != null)
                     {
                         if (Links.Right.Focus())
                             return;
                     }
                     break;
            }
            
            Parent?.ProcessSelection(Button);
        }
    }
}
