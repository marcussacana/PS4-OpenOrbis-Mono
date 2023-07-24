using Orbis.Internals;
using OrbisGL.GL;
using OrbisGL.GL2D;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Input
{
    internal class OSKTypewriter : ITypewriter, IDisposable
    {
        string _CurrentText = string.Empty;
        public string CurrentText {
            get => _CurrentText;
            set 
            {
                _CurrentText = value;
                if (CurrentText.Length > Constants.SCE_IME_MAX_TEXT_LENGTH)
                    _CurrentText = CurrentText.Substring(0, Constants.SCE_IME_MAX_TEXT_LENGTH);

                byte[] Data = Encoding.Unicode.GetBytes(_CurrentText + "\x0");
                Marshal.Copy(Data, 0, Buffer, Data.Length);
            }
        }
        public string CurrentAccumulator { get; set; }
        public int CaretPosition { get; set; }
        public int SelectionLength { get; set; }

        public event EventHandler OnComplete;
        public event EventHandler OnTextChanged;
        public event EventHandler OnCaretMove;

        bool IMEOpen;

        IntPtr Buffer;
        public OSKTypewriter()
        {
            Buffer = Marshal.AllocHGlobal((Constants.SCE_IME_MAX_TEXT_LENGTH + 1) * 2);
            CurrentText = "";
        }

        ~OSKTypewriter()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (IMEOpen)
            {
                sceImeDialogAbort();
                sceImeDialogTerm();
            }

            if (Buffer == IntPtr.Zero)
                return;

            Marshal.FreeHGlobal(Buffer);
            Buffer = IntPtr.Zero;
        }

        public unsafe void Enter(Rectangle TextArea)
        {
            if (IMEOpen || Buffer == IntPtr.Zero)
                return;

            OrbisImeDialogParam DiagParam = new OrbisImeDialogParam();
            
            //inline func, just call memset 0 and set userID to INVALID
            //sceImeDialogParamInit(ref param);
            
            UserService.GetForegroundUser(out DiagParam.userId);

            DiagParam.type = IME_Type.DEFAULT;
            DiagParam.inputMethod = IME_InputMethod.DEFAULT;
            DiagParam.option = IME_Option.EXT_KEYBOARD;
            DiagParam.enterLabel = IME_EnterLabel.DEFAULT;
            DiagParam.horizontalAlignment = IME_Horizontal_Align.LEFT;
            DiagParam.verticalAlignment = IME_Vertical_Align.TOP;
            DiagParam.inputTextBuffer = Buffer;
            DiagParam.maxTextLength = Constants.SCE_IME_MAX_TEXT_LENGTH;
            DiagParam.filter = null;
            DiagParam.reserved = new byte[16];

            IntPtr pStructBuff = Marshal.AllocHGlobal(96);
            Marshal.StructureToPtr(DiagParam, pStructBuff, false);

            OrbisImeDialogParam* pParam = (OrbisImeDialogParam*)pStructBuff.ToPointer();

            sceImeDialogGetPanelSizeExtended(pParam, null, out int Width, out int Height);

            DiagParam.posx = TextArea.X + (TextArea.Width / 2) - (Width / 2);
            
            DiagParam.posx = Math.Max(Math.Min(DiagParam.posx, Coordinates2D.Width - Width), 0);
            DiagParam.posy = Math.Max(Math.Min(TextArea.Bottom, Coordinates2D.Height - Height), 0);
            
            Marshal.StructureToPtr(DiagParam, pStructBuff, false);

            int rst = sceImeDialogInit(pParam, null);
            
            Marshal.FreeHGlobal(pStructBuff);
            
            if (rst == Constants.SCE_OK)
                IMEOpen = true;
        }

        public void Exit() {
            if (!IMEOpen)
                return;

            sceImeDialogAbort();
            IMEOpen = false;
        }

        public void Refresh()
        {
            if (!IMEOpen)
                return;

            switch (sceImeDialogGetStatus())
            {
                case OrbisImeDialogStatus.ORBIS_IME_DIALOG_STATUS_FINISHED:
                    IMEOpen = false;
                    UpdateText();
                    break;
            }
        }

        private unsafe void UpdateText()
        {
            var pResultBuff = Marshal.AllocHGlobal(16);
            Marshal.Copy(new byte[16], 0, pResultBuff, 16);
            
            var pResult = (OrbisImeDialogResult*)pResultBuff.ToPointer();

            if (sceImeDialogGetResult(pResult) != Constants.SCE_OK)
                return;

            sceImeDialogTerm();

            switch (pResult->endstatus)
            {
                case OrbisImeDialogEndStatus.ORBIS_IME_DIALOG_END_STATUS_USER_CANCELED:
                case OrbisImeDialogEndStatus.ORBIS_IME_DIALOG_END_STATUS_ABORTED:
                    Marshal.FreeHGlobal(pResultBuff);
                    return;
            }
            
            Marshal.FreeHGlobal(pResultBuff);
            

            List<byte> Buffer = new List<byte>();

            ushort* pBuffer = (ushort*)this.Buffer.ToPointer();
            while (*pBuffer != 0)
            {
                ushort Char = *pBuffer++;

                Buffer.Add((byte)(Char & 0xFF));
                Buffer.Add((byte)((Char >> 8) & 0xFF));
            }

            string Text = Encoding.Unicode.GetString(Buffer.ToArray(), 0, Buffer.Count);

            CurrentText = Text;
            CurrentAccumulator = string.Empty;
            CaretPosition = CurrentText.Length;
            SelectionLength = 0;

            OnCaretMove?.Invoke(this, EventArgs.Empty);
            OnTextChanged?.Invoke(this, EventArgs.Empty);
            OnComplete?.Invoke(this, EventArgs.Empty);
        }

        [DllImport("libSceImeDialog.sprx")]
        private unsafe static extern int sceImeDialogInit(OrbisImeDialogParam* param, OrbisImeParamExtended* extended);

        [DllImport("libSceImeDialog.sprx")]
        private unsafe static extern int sceImeDialogGetPanelSizeExtended(OrbisImeDialogParam* param, OrbisImeParamExtended* extended, out int Width, out int Height);

        [DllImport("libSceImeDialog.sprx")]
        private static extern OrbisImeDialogStatus sceImeDialogGetStatus();

        [DllImport("libSceImeDialog.sprx")]
        private unsafe static extern int sceImeDialogGetResult(OrbisImeDialogResult* Result);

        [DllImport("libSceImeDialog.sprx")]
        private static extern int sceImeDialogTerm();

        [DllImport("libSceImeDialog.sprx")]
        private static extern int sceImeDialogAbort();

        [StructLayout(LayoutKind.Sequential)]
        struct OrbisRtcTick
        {
            public ulong tick;
        }

        enum OrbisImeDialogStatus : int
        {
            ORBIS_IME_DIALOG_STATUS_NONE = 0,
            ORBIS_IME_DIALOG_STATUS_RUNNING,
            ORBIS_IME_DIALOG_STATUS_FINISHED,
        }

        enum OrbisImeDialogEndStatus
        {
            ORBIS_IME_DIALOG_END_STATUS_OK = 0,
            ORBIS_IME_DIALOG_END_STATUS_USER_CANCELED,
            ORBIS_IME_DIALOG_END_STATUS_ABORTED,
        }

        [StructLayout(LayoutKind.Sequential)]
        struct OrbisImeDialogResult
        {
            public OrbisImeDialogEndStatus endstatus;//+4
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] reserved;//+16
        }


        [StructLayout(LayoutKind.Sequential)]
        struct OrbisImeDialogParam
        {
            public int userId;//+4
            public IME_Type type;//+8
            public ulong supportedLanguages;//+16
            public IME_EnterLabel enterLabel;//+20
            public IME_InputMethod inputMethod;//+24
            public OrbisImeTextFilter filter;//+32
            public IME_Option option;//+36
            public uint maxTextLength;//+40
            public IntPtr inputTextBuffer;//+48
            public float posx;//+52
            public float posy;//+56
            public IME_Horizontal_Align horizontalAlignment;//+60
            public IME_Vertical_Align verticalAlignment;//+64
            public IntPtr placeholder;//+72
            public IntPtr title;//+80
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] reserved;//+96
        }

        [StructLayout(LayoutKind.Sequential)]
        struct OrbisImeColor
        {
            public byte r;
            public byte g;
            public byte b;
            public byte a;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct OrbisImeKeycode
        {
            public ushort keycode;
            public ushort character;
            public uint status;
            public IME_KeyboardType type;
            public int userId;
            public uint resourceId;
            public OrbisRtcTick timestamp;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct OrbisImeParamExtended
        {
            public IME_ExtraOption option;
            public OrbisImeColor colorBase;
            public OrbisImeColor colorLine;
            public OrbisImeColor colorTextField;
            public OrbisImeColor colorPreedit;
            public OrbisImeColor colorButtonDefault;
            public OrbisImeColor colorButtonFunction;
            public OrbisImeColor colorButtonSymbol;
            public OrbisImeColor colorText;
            public OrbisImeColor colorSpecial;
            public IME_PanelPriority priority;
            [MarshalAs(UnmanagedType.LPStr)]
            public string additionalDictionaryPath;
            public OrbisImeExtKeyboardFilter extKeyboardFilter;
            public uint disableDevice;
            public uint extKeyboardMode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            public byte[] reserved;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate int OrbisImeTextFilter([MarshalAs(UnmanagedType.LPWStr)] string outText,
                                            ref uint outTextLength,
                                            [MarshalAs(UnmanagedType.LPWStr)] string srcText,
                                            uint srcTextLength);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate int OrbisImeExtKeyboardFilter(ref OrbisImeKeycode srcKeycode,
                                                      ref ushort outKeycode,
                                                      ref uint outStatus,
                                                      IntPtr reserved);

    }
}