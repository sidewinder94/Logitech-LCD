using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Logitech_LCD
{

    public enum MonoBitmap
    {
        Width = 160,
        Height = 43,
        Bpp = 1,
    }

    public enum ColorBitmap
    {
        Width = 320,
        Height = 240,
        Bpp = 4,
    }
    public enum ReturnValue
    {
        ErrorSuccess = 0,
        ErrorFileNotFound = 2,
        ErrorAccessDenied = 5,
        ErrorInvalidParameter = 87,
        ErrorLockFailed = 167,
        ErrorAlreadyExists = 183,
        ErrorNoMoreItems = 259,
        ErrorOldWinVersion = 1150,
        ErrorServiceNotActive = 1062,
        ErrorDeviceNotConnected = 1167,
        ErrorAlreadyInitialized = 1247,
        ErrorNoSystemResources = 1450,
        RcpSServerUnavailable = 1722,
        RcpXWrongPipeVersion = 1832,
    }

    /// <summary>
    /// Class containing necessary informations and calls to the Logitech SDK
    /// </summary>
    public class NativeMethods
    {
        private const String basePath = @"Lib\";
        private const String dllName = @"Lib\x86\LogitechLcd.dll";
        private static String dllPath = Environment.Is64BitOperatingSystem ? basePath + "x64\\" + dllName
                                                                           : basePath + "x86\\" + dllName;


        #region Enumerations
        /// <summary>
        /// LCD Types
        /// </summary>
        [Flags]
        public enum LcdType
        {
            Mono = 0x1,
            Color = 0x2,
        }

        /// <summary>
        /// Screen buttons
        /// </summary>
        [Flags]
        public enum Button
        {
            MonoButton0 = 0x1,
            ManoButton1 = 0x2,
            MonoButton2 = 0x4,
            MonoButton3 = 0x8,
            ColorLeft = 0x100,
            ColorRight = 0x200,
            ColorOK = 0x400,
            ColorCancel = 0x800,
            ColorUp = 0x1000,
            ColorDown = 0x2000,
            ColorMenu = 0x4000,
        }
        #endregion

        #region Mapping methods
        //General functions

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdInit", CharSet = CharSet.Unicode)]
        public static extern bool Init(String friendlyName, LcdType lcdType);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdIsConnected", CharSet = CharSet.Unicode)]
        public static extern bool IsConnected(LcdType lcdType);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdIsButtonPressed", CharSet = CharSet.Unicode)]
        public static extern bool IsButtonPressed(Button button);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdUpdate", CharSet = CharSet.Unicode)]
        public static extern void Update();

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdShutdown", CharSet = CharSet.Unicode)]
        public static extern void Shutdown();

        //Monochrome LCD Functions
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdMonoSetBackground", CharSet = CharSet.Unicode)]
        public static extern bool MonoSetBackground(byte[] monoBitmap);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdMonoSetText", CharSet = CharSet.Unicode)]
        public static extern bool MonoSetText(int lineNumber, String text);
        //Color LCD Functions
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdColorSetBackground", CharSet = CharSet.Unicode)]
        public static extern bool ColorSetBackground(byte[] colorBitmap);
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdColorSetTitle", CharSet = CharSet.Unicode)]
        public static extern bool ColorSetTitle(String text, int red, int green, int blue);
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdColorSetText", CharSet = CharSet.Unicode)]
        public static extern bool ColorSetText(int lineNumber, String text, int red, int green, int blue);

        #endregion
    }
}
