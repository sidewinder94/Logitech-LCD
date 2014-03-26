using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Logitech_LCD
{
    /// <summary>
    /// Class containing necessary informations and calls to the Logitech SDK
    /// </summary>
    public class NativeMethods
    {
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
        public enum Buttons
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
        [DllImport("LogitechLcd.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdInit")]
        public static extern int Init(String friendlyName, LcdType lcdType);

        [DllImport("LogitechLcd.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdIsConnected")]
        public static extern int IsConnected(LcdType lcdType);
        #endregion
    }
}
