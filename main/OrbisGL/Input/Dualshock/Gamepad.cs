using OrbisGL.GL;
using System;
using System.Runtime.InteropServices;

namespace OrbisGL.Input.Dualshock
{
    public class Gamepad : GenericPad<OrbisPadData>
    {
        int Handler = int.MinValue;

        static bool Initialized = false;
        public override void Open(int UserID)
        {
            if (!Initialized)
            {
                scePadInit();
                Initialized = true;
            }

            Handler = scePadOpen(UserID, OrbisPadPortType.Standard, 0, new OrbisPadOpenParam());

            if (Handler < 0)
                throw new Exception("Failed to open the gamepad");
        }
        
        OrbisPadData PadData = new OrbisPadData()
        {
            Touch = new OrbisPadTouchData()
            {
                Pad1 = new byte[3],
                Touch = new OrbisPadTouch[Constants.ORBIS_PAD_MAX_TOUCH_NUM]
            },
            Extension = new byte[16],
            Unknown = new byte[15]
        };
        public override void Refresh()
        {
            if (Handler < 0)
            {
                throw new Exception("Gampad not open");
            }

            if (scePadReadState(Handler, ref PadData) != Constants.SCE_OK)
                return;

            CurrentData = PadData;
        }
        public override void Close()
        {
            if (Handler < 0)
                return;

            scePadClose(Handler);
            Handler = int.MinValue;
        }

        public void SetTouchColor(RGBColor Color, byte Intensity = 255)
        {
            if (Handler < 0)
                return;

            if (Color == null)
            {
                scePadResetLightBar(Handler);
                return;
            }

            scePadSetLightBar(Handler, new OrbisPadColor() { 
                    R = (byte)Color.R,
                    G = (byte)Color.G,
                    B = (byte)Color.B,
                    A = Intensity
            });
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct OrbisPadOpenParam
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] reserve;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct OrbisPadColor
        {
            public byte R, G, B, A;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct OrbisPadVibeParam
        {
            public byte LargeMotor, SmallMotor;
        }

        [DllImport("libScePad.sprx")]
        static extern int scePadInit();

        [DllImport("libScePad.sprx")]
        static extern int scePadOpen(int userId, OrbisPadPortType Type, int index, OrbisPadOpenParam pParam);

        [DllImport("libScePad.sprx")]
        static extern int scePadClose(int handle);

        [DllImport("libScePad.sprx")]
        static extern int scePadRead(int handle, ref OrbisPadData Data, int Count);

        [DllImport("libScePad.sprx")]
        static extern int scePadReadState(int handle, ref OrbisPadData Data);

        [DllImport("libScePad.sprx")]
        static extern int scePadResetLightBar(int handle);

        [DllImport("libScePad.sprx")]
        static extern int scePadSetLightBar(int handle, OrbisPadColor Color);

        [DllImport("libScePad.sprx")]
        static extern int scePadSetVibration(int handle, OrbisPadVibeParam Color);
    }
}
