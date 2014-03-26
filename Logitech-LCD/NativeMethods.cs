using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Logitech_LCD
{
    /// <summary>
    /// 
    /// </summary>
    public enum MonoBitmap
    {
        Width = 160,
        Height = 43,
        Bpp = 1,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ColorBitmap
    {
        Width = 320,
        Height = 240,
        Bpp = 4,
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="friendlyName"></param>
        /// <param name="lcdType"></param>
        /// <returns></returns>
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdInit", CharSet = CharSet.Unicode)]
        public static extern bool Init(String friendlyName, LcdType lcdType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lcdType"></param>
        /// <returns></returns>
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdIsConnected", CharSet = CharSet.Unicode)]
        public static extern bool IsConnected(LcdType lcdType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdIsButtonPressed", CharSet = CharSet.Unicode)]
        public static extern bool IsButtonPressed(Button button);

        /// <summary>
        /// 
        /// </summary>
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdUpdate", CharSet = CharSet.Unicode)]
        public static extern void Update();

        /// <summary>
        /// 
        /// </summary>
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdShutdown", CharSet = CharSet.Unicode)]
        public static extern void Shutdown();

        //Monochrome LCD Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monoBitmap"></param>
        /// <returns></returns>
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdMonoSetBackground", CharSet = CharSet.Unicode)]
        public static extern bool MonoSetBackground(byte[] monoBitmap);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lineNumber"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdMonoSetText", CharSet = CharSet.Unicode)]
        public static extern bool MonoSetText(int lineNumber, String text);
        //Color LCD Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colorBitmap"></param>
        /// <returns></returns>
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdColorSetBackground", CharSet = CharSet.Unicode)]
        public static extern bool ColorSetBackground(byte[] colorBitmap);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <returns></returns>
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdColorSetTitle", CharSet = CharSet.Unicode)]
        public static extern bool ColorSetTitle(String text, int red, int green, int blue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lineNumber"></param>
        /// <param name="text"></param>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <returns></returns>
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdColorSetText", CharSet = CharSet.Unicode)]
        public static extern bool ColorSetText(int lineNumber, String text, int red, int green, int blue);

        #endregion
    }
}
