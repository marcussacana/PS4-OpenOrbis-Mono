using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Input
{
    public abstract class GenericPad<T> : IPad where T : struct
    {
        public T CurrentData { get; protected set; }

        object IPad.Data => CurrentData;

        public abstract void Close();

        public abstract void Open(int UserID);

        public abstract void Refresh();
    }
}
