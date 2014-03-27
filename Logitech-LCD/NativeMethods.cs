﻿using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Logitech_LCD
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
    /// <summary>
    /// The screen infromations about Monochrome display
    /// </summary>
    public enum MonoBitmap
    {
        Width = 160,
        Height = 43,
        Bpp = 1,
    }

    /// <summary>
    /// The screen informations about Color display
    /// </summary>
    public enum ColorBitmap
    {
        Width = 320,
        Height = 240,
        Bpp = 4,
    }
    #endregion


    /// <summary>
    /// Class containing necessary informations and calls to the Logitech SDK
    /// </summary>
    class NativeMethods
    {
        private const String basePath = @"Lib\";
        private const String dllName86 = @"Lib\x86\LogitechLcd.dll";
        private const String dllName64 = @"Lib\x64\LogitechLcd.dll";

        #region Mapping methods
        //General functions

        /// <summary>
        /// Allows the initialization of the SDK, MUST be called before any other function
        /// </summary>
        /// <param name="friendlyName">The name of the applet, cannot be changed after</param>
        /// <param name="lcdType">The lcdType to initialize</param>
        /// <returns>True if success, False if failed</returns>
        [DllImport(dllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdInit", CharSet = CharSet.Unicode)]
        public static extern bool Init(String friendlyName, LcdType lcdType);

        /// <summary>
        /// Check if a screen is connected, the <see cref="Init"/> function have to be called before, or it could return
        /// unexpected results
        /// </summary>
        /// <param name="lcdType">The lcd type to check</param>
        /// <returns>True if connected, False if not</returns>
        [DllImport(dllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdIsConnected", CharSet = CharSet.Unicode)]
        public static extern bool IsConnected(LcdType lcdType);

        /// <summary>
        /// Check if a button is pressed
        /// </summary>
        /// <param name="button">The button to check</param>
        /// <returns></returns>
        [DllImport(dllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdIsButtonPressed", CharSet = CharSet.Unicode)]
        public static extern bool IsButtonPressed(Button button);

        /// <summary>
        /// Refresh the screen
        /// </summary>
        [DllImport(dllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdUpdate", CharSet = CharSet.Unicode)]
        public static extern void Update();

        /// <summary>
        /// Shutdown the SDK, closes the applet and frees the memory
        /// </summary>
        [DllImport(dllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdShutdown", CharSet = CharSet.Unicode)]
        public static extern void Shutdown();

        //Monochrome LCD Functions

        /// <summary>
        /// Displays a bitmap on a Monochrome screen
        /// </summary>
        /// <param name="monoBitmap">The array of bytes to display, a byte will be displayed if it's value is > 128 <see cref="MonoBitmap"/></param>
        /// <returns></returns>
        [DllImport(dllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdMonoSetBackground", CharSet = CharSet.Unicode)]
        public static extern bool MonoSetBackground(byte[] monoBitmap);

        /// <summary>
        /// Displays text on a monochrome screen
        /// </summary>
        /// <param name="lineNumber">The line number [0-3]</param>
        /// <param name="text">The text to display</param>
        /// <returns>True if succeeds false otherwise</returns>
        [DllImport(dllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdMonoSetText", CharSet = CharSet.Unicode)]
        public static extern bool MonoSetText(int lineNumber, String text);
        //Color LCD Functions

        /// <summary>
        /// Displays a bitmap on a clor screen
        /// </summary>
        /// <param name="colorBitmap">The array of bytes to be displayed <see cref="ColorBitmap"/></param>
        /// <returns>True if succeeds false otherwise</returns>
        [DllImport(dllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdColorSetBackground", CharSet = CharSet.Unicode)]
        public static extern bool ColorSetBackground(byte[] colorBitmap);

        /// <summary>
        /// Displays a line of text as a title on a color screen
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="red">Red component of the title's color</param>
        /// <param name="green">Green component of the title's color</param>
        /// <param name="blue">Blue component of the title's color</param>
        /// <returns>True if succeeds false otherwise</returns>
        [DllImport(dllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdColorSetTitle", CharSet = CharSet.Unicode)]
        public static extern bool ColorSetTitle(String text, int red, int green, int blue);

        /// <summary>
        /// Displays a line of text on a color screen
        /// </summary>
        /// <param name="lineNumber">The line number [0-7]</param>
        /// <param name="text">The text to display</param>
        /// <param name="red">Red component of the text color</param>
        /// <param name="green">Green component of the text color</param>
        /// <param name="blue">Blue component of the text color</param>
        /// <returns>True if succeeds false otherwise</returns>
        [DllImport(dllName86, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLcdColorSetText", CharSet = CharSet.Unicode)]
        public static extern bool ColorSetText(int lineNumber, String text, int red, int green, int blue);

        #endregion
    }
}
