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

    public struct DualshockSettings
    {
        /// <summary>
        /// Sets a virtual mouse mode
        /// </summary>
        public VirtualMouse Mouse;


        /// <summary>
        /// When true, the Left Analog will be converted as Up/Down/Left/Right buttons
        /// </summary>
        public bool LeftAnalogAsPad;

        //[WIP] Implement Deadzone and Sensitivity
        //https://stackoverflow.com/questions/43240440/c-sharp-joystick-sensitivity-formula/43245072#43245072
    }
}
