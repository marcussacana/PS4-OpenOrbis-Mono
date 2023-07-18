using System;
using System.Runtime.InteropServices;

namespace OrbisGL.Input.Dualshock
{
    public class Gamepad : GenericPad<OrbisPadData>
    {
        int Handler;

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

        public unsafe override void Refresh()
        {
            if (Handler < 0)
            {
                throw new Exception("Gampad not open");
            }

            OrbisPadData PadData = new OrbisPadData();

            if (scePadReadState(Handler, &PadData) != Constants.SCE_OK)
                return;

            CurrentData = PadData;
        }
        public override void Close()
        {
            if (Handler >= 0)
            {
                scePadClose(Handler);
                Handler = 0;
            }
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
        static unsafe extern int scePadRead(int handle, OrbisPadData* Data, int Count);

        [DllImport("libScePad.sprx")]
        static unsafe extern int scePadReadState(int handle, OrbisPadData* Data);

        [DllImport("libScePad.sprx")]
        static extern int scePadResetLightBar(int handle);

        [DllImport("libScePad.sprx")]
        static extern int scePadSetLightBar(int handle, OrbisPadColor Color);

        [DllImport("libScePad.sprx")]
        static extern int scePadSetVibration(int handle, OrbisPadVibeParam Color);
    }
}
