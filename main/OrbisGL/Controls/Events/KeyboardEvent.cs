using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Controls.Events
{
    public delegate void KeyboardEventDelegate(object Sender, KeyboardEventArgs Args);
    public class KeyboardEventArgs : PropagableEventArgs
    {
        public char? KeyChar { get; }
        public IME_KeyCode Keycode { get; }
        public IME_KeycodeState Modifiers { get; }
        public KeyboardEventArgs(IME_KeyCode Code, IME_KeycodeState State, char? KeyChar) {
            Keycode = Code;
            Modifiers = State;
            this.KeyChar = KeyChar;
        }
    }
}
