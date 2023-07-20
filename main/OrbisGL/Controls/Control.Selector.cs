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

        internal static Selector Selector = new Selector();

        public void SetAsSelected()
        {
            Selector.Select(this);
        }
    }
}
