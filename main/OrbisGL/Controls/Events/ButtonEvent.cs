using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Controls.Events
{
    public class ButtonEventArgs : PropagableEventArgs
    {
        public OrbisPadButton Button { get; }
        public ButtonEventArgs(OrbisPadButton Button) {
            this.Button = Button;
        }
    }
}
