using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL
{
    public struct IMEKeyModifier
    {
        public IME_KeyCode Code;
        public bool Shift;
        public bool Alt;
        public bool NumLock;

        public IMEKeyModifier(IME_KeyCode Code, bool Shift, bool Alt, bool NumLock)
        {
            this.Code = Code;
            this.Shift = Shift;
            this.Alt = Alt;
            this.NumLock = NumLock;
        }
    }
}
