using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Input
{
    public interface IPad
    {
        object Data { get; }
        void Open(int UserID);
        void Close();
        void Refresh();
    }
}
