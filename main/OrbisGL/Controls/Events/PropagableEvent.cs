﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Controls.Events
{
    public class PropagableEvent : EventArgs
    {
        public bool Handled { get; set; }
    }
}