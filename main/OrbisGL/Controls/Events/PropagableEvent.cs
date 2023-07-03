using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Controls.Events
{
    public class PropagableEventArgs : EventArgs
    {
        /// <summary>
        /// When true, stops the event propagation to the parent controller
        /// </summary>
        public bool Handled { get; set; }
    }
}
