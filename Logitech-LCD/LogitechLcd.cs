using Logitech_LCD.Exceptions;
using System;

namespace Logitech_LCD
{
    /// <summary>
    /// The class to effectively use to access the SDK functions
    /// </summary>
    public class LogitechLcd
    {
        #region Singleton implementation
        public static LogitechLcd Instance
        {
            get
            {
                return Nested.instance;
            }
            private set
            {
                Instance = value;
            }
        }

        private class Nested
        {
            internal static readonly LogitechLcd instance = new LogitechLcd();
        }
        #endregion

        #region constructor / destructor
        private LogitechLcd()
        { }

        ~LogitechLcd()
        {
            NativeMethods.Shutdown();
        }
        #endregion

        /// <summary>
        /// Used to call all <see cref="NativeMethods"/> methods. Will throw an exception
        /// if the initialization is note done before callig the method 
        /// </summary>
        /// <param name="method">The method to execute</param>
        /// <param name="args">The args of the called function</param>
        /// <returns>The function returns, needs to be casted</returns>
        private object InvokeMethod(Delegate method, params object[] args)
        {
            if (IsInit)
            {
                return method.DynamicInvoke(args);
            }
            else
            {
                throw new LcdNotInitializedException();
            }
        }

        private bool _init;

        public bool IsInit
        {
            get
            {
                return this._init;
            }
            private set
            {
                this._init = value;
            }
        }

        /// <summary>
        /// Allows the initialization of the SDK, MUST be called before any other function
        /// </summary>
        /// <param name="friendlyName">The name of the applet, cannot be changed after</param>
        /// <param name="lcdType">The lcdType to initialize</param>
        /// <returns>True if success, False if failed</returns>
        public bool Init(String friendlyName, LcdType lcdType)
        {
            IsInit = NativeMethods.Init(friendlyName, lcdType);
            return IsInit;
        }

        /// <summary>
        /// Check if a screen is connected, the <see cref="Init"/> function have to be called before, or it could return
        /// unexpected results
        /// </summary>
        /// <param name="lcdType">The lcd type to check</param>
        /// <returns>True if connected, false otherwise</returns>
        /// <exception cref="LcdNotInitializedException">If the LCD Screen has not been initialized</exception>
        public bool IsConnected(LcdType lcdType)
        {
            try
            {
                return (bool)InvokeMethod(new Func<LcdType, bool>(NativeMethods.IsConnected), lcdType);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check if a button is pressed
        /// </summary>
        /// <param name="button">The button to check</param>
        /// <returns>True if specified button pressed, false otherwise</returns>
        /// <exception cref="LcdNotInitializedException">If the LCD Screen has not been initialized</exception>
        public bool IsButtonPressed(Buttons button)
        {
            return (bool)InvokeMethod(new Func<Buttons, bool>(NativeMethods.IsButtonPressed), button);
        }

        /// <summary>
        /// Refresh the screen
        /// </summary>
        /// <exception cref="LcdNotInitializedException">If the LCD Screen has not been initialized</exception>
        public void Update()
        {
            InvokeMethod(new Action(NativeMethods.Update));
        }

        /// <summary>
        /// Displays a bitmap on a Monochrome screen
        /// </summary>
        /// <param name="monoBitmap">The array of bytes to display, a byte will be displayed if it's value is > 128 <see cref="MonoBitmap"/></param>
        /// <returns>True if succeeds false otherwise</returns>
        /// <exception cref="LcdNotInitializedException">If the LCD Screen has not been initialized</exception>
        public bool MonoSetBackground(byte[] monoBitmap)
        {
            return (bool)InvokeMethod(new Func<byte[], bool>(NativeMethods.MonoSetBackground), monoBitmap);
        }

        /// <summary>
        /// Displays text on a monochrome screen
        /// </summary>
        /// <param name="lineNumber">The line number [0-3]</param>
        /// <param name="text">The text to display</param>
        /// <returns>True if succeeds false otherwise</returns>
        /// <exception cref="LcdNotInitializedException">If the LCD Screen has not been initialized</exception>
        public bool MonoSetText(int lineNumber, String text)
        {
            if ((lineNumber < 0) || (lineNumber > 3))
            {
                throw new ArgumentOutOfRangeException("lineNumber", lineNumber,
                    "Should be between 0 and 3 included");
            }
            return (bool)InvokeMethod(new Func<int, String, bool>(NativeMethods.MonoSetText), lineNumber, text);
        }

        /// <summary>
        /// Displays a bitmap on a color screen
        /// </summary>
        /// <param name="colorBitmap">The array of bytes to be displayed <see cref="ColorBitmap"/></param>
        /// <returns>True if succeeds false otherwise</returns>
        /// <exception cref="LcdNotInitializedException">If the LCD Screen has not been initialized</exception>
        public bool ColorSetBackground(byte[] colorBitmap)
        {
            return (bool)InvokeMethod(new Func<byte[], bool>(NativeMethods.ColorSetBackground), colorBitmap);
        }

        /// <summary>
        /// Displays a line of text as a title on a color screen
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="red">Red component of the title's color</param>
        /// <param name="green">Green component of the title's color</param>
        /// <param name="blue">Blue component of the title's color</param>
        /// <returns>True if succeeds false otherwise</returns>
        /// <exception cref="LcdNotInitializedException">If the LCD Screen has not been initialized</exception>
        public bool ColorSetTitle(String text, int red, int green, int blue)
        {
            return (bool)InvokeMethod(new Func<String, int, int, int, bool>(NativeMethods.ColorSetTitle),
                text, red, green, blue);
        }

        /// <summary>
        /// Displays a line of text on a color screen
        /// </summary>
        /// <param name="lineNumber">The line number [0-7]</param>
        /// <param name="text">The text to display</param>
        /// <param name="red">Red component of the text color</param>
        /// <param name="green">Green component of the text color</param>
        /// <param name="blue">Blue component of the text color</param>
        /// <returns>True if succeeds false otherwise</returns>
        /// <exception cref="LcdNotInitializedException">If the LCD Screen has not been initialized</exception>
        public bool ColorSetText(int lineNumber, String text, int red, int green, int blue)
        {
            if ((lineNumber < 0) || (lineNumber > 7))
            {
                throw new ArgumentOutOfRangeException("lineNumber", lineNumber,
                    "Should be between 0 and 7 included");
            }
            return (bool)InvokeMethod(new Func<int, String, int, int, int, bool>(NativeMethods.ColorSetText),
                lineNumber, text, red, green, blue);
        }
    }
}
