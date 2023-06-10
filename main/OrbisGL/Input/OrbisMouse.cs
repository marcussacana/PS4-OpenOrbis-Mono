using OrbisGL.GL2D;
using System.Numerics;
using System.Runtime.InteropServices;

namespace OrbisGL.Input
{
    public unsafe class OrbisMouse : IMouse
    {
        int CurrentX;
        int CurrentY;

        int MouseHandle = 0;
        int CurrentUserID = 0;
        SceMouseData CurrentData = default;

        public void RefreshData() 
        {
            fixed (SceMouseData* pCurrData = &CurrentData)
            {
                sceMouseRead(MouseHandle, pCurrData, 1);
            }
        }

        public MouseButtons GetMouseButtons()
        {
            MouseButtons CurrentButtons = 0;

            if ((CurrentData.buttons & Constants.SCE_MOUSE_BUTTON_PRIMARY) != 0)
                CurrentButtons |= MouseButtons.Left;

            if ((CurrentData.buttons & Constants.SCE_MOUSE_BUTTON_SECONDARY) != 0)
                CurrentButtons |= MouseButtons.Right;

            if ((CurrentData.buttons & Constants.SCE_MOUSE_BUTTON_OPTIONAL) != 0)
                CurrentButtons |= MouseButtons.Middle;

            return CurrentButtons;
        }

        public Vector2 GetPosition()
        {
            CurrentX += CurrentData.xAxis;
            CurrentY += CurrentData.yAxis;
            return new Vector2(CurrentX, CurrentY);
        }

        public bool Initialize(int UserID = -1)
        {
            CurrentUserID = UserID;
            
            if (sceMouseInit() != Constants.SCE_OK)
                return false;

            var OpenParam = new SceMouseOpenParam();
            OpenParam.behaviorFlag = 0; //Constants.SCE_MOUSE_OPEN_PARAM_MERGED;

            MouseHandle = sceMouseOpen(CurrentUserID, Constants.SCE_MOUSE_PORT_TYPE_STANDARD, 0, OpenParam);

            CurrentX = Coordinates2D.Width / 2;
            CurrentY = Coordinates2D.Height / 2;

            return true;
        }

        public void Dispose()
        {
            sceMouseClose();
        }


        [DllImport("libSceMouse.sprx")]
        static extern int sceMouseInit();

        [DllImport("libSceMouse.sprx")]
        static extern int sceMouseClose();

        [DllImport("libSceMouse.sprx")]
        static extern int sceMouseOpen(int UserID, int Type, int Index, SceMouseOpenParam pParam);

        [DllImport("libSceMouse.sprx")]
        static extern int sceMouseRead(int Handle, SceMouseData* pData, int num);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct SceMouseOpenParam
        {
            public byte behaviorFlag;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public byte[] reserve;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct SceMouseData
        {
            public ulong timestamp;
            [MarshalAs(UnmanagedType.I1)]
            public bool connected;
            public uint buttons;
            public int xAxis;
            public int yAxis;
            public int wheel;
            public int tilt;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] reserve;
        }

    }
}
