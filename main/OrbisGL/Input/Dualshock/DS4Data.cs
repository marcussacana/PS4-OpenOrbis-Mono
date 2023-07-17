using System.Numerics;

namespace OrbisGL.Input.Dualshock
{
    public struct DS4Info
    {
        public OrbisPadPortType PortType;
        public OrbisPadDeviceClass DeviceClass;
        public OrbisPadConnectionType ConnectionType;
        public OrbisPadButton PressedButtons;

        public Vector2 LeftStick;
        public Vector2 RightStick;

        public float L2;
        public float R2;

        public Vector2 DPad;
    }
}
