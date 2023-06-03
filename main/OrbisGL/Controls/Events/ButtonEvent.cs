using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Controls.Events
{
    public class ButtonEvent : PropagableEvent
    {
        public OrbisPadButton Button { get; }
        public ButtonEvent(OrbisPadButton Button) {
            this.Button = Button;
        }
    }
}
