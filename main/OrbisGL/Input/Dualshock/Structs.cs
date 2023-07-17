using System.Numerics;
using System.Runtime.InteropServices;

namespace OrbisGL.Input.Dualshock
{
    public struct Stick
    {
        public byte X, Y;
    }
    public struct Analog
    {
        public byte L2, R2;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct OrbisPadTouch
    {
        public ushort X;
        public ushort Y;
        public byte Finger;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        byte[] Pad;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct OrbisPadTouchData
    {
        public byte Fingers;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        byte[] Pad1;
        uint Pad2;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.ORBIS_PAD_MAX_TOUCH_NUM)]
        public OrbisPadTouch[] Touch;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct OrbisPadData
    {
        public OrbisPadButton Buttons;
        public Stick LeftStick;
        public Stick RightStick;
        public Analog AnalogButtons;
        public ushort Padding;
        public Vector4 Orientation;
        public Vector3 Accelerometer;
        public Vector3 AngularVelocity;
        public OrbisPadTouchData Touch;
        public byte Connected;
        public ulong Timestamp;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] Extension;
        public byte ConnectionCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public byte[] Unknown;
    }

}
