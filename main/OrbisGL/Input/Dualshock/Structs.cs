using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static OrbisGL.GL2D.Coordinates2D;

namespace OrbisGL.Input.Dualshock
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Stick
    {
        public byte X, Y;

        public static explicit operator Vector2(Stick Stick)
        {
            return new Vector2(XToPoint(Stick.X, 255), YToPoint(Stick.Y, 255));
        }
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
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

        public static explicit operator Vector2(OrbisPadTouch Touch)
        {
            return new Vector2(XToPoint(Touch.X, 1919), YToPoint(Touch.Y, 941));
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct OrbisPadTouchData
    {
        public byte Fingers;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Pad1;
        public uint Pad2;

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
