using OrbisGL.GL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Controls
{
    public interface IControl : IDisposable
    {
        bool Invalidated { get; }
    }
}
