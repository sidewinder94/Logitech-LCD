using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitech_LCD.Exceptions;

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
            if (isInit)
            {
                return method.DynamicInvoke(args);
            }
            else
            {
                throw new LcdNotInitializedException();
            }
        }

        private bool _init;

        public bool isInit
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

        public bool init(String friendlyName, LcdType lcdType)
        {
            isInit = NativeMethods.Init(friendlyName, lcdType);
            return isInit;
        }

        public bool isConnected(LcdType lcdType)
        {
            return (bool)InvokeMethod(new Func<LcdType, bool>(NativeMethods.IsConnected), lcdType);
        }

        public bool isButtonPressed(Button button)
        {
            return (bool)InvokeMethod(new Func<Button, bool>(NativeMethods.IsButtonPressed), button);
        }

        public void update()
        {
            InvokeMethod(new Action(NativeMethods.Update));
        }

        public bool monoSetBackground(byte[] monoBitmap)
        {
            return (bool)InvokeMethod(new Func<byte[], bool>(NativeMethods.MonoSetBackground), monoBitmap);
        }

        public bool monoSetText(int lineNumber, String text)
        {
            if ((lineNumber < 0) || (lineNumber > 3))
            {
                throw new ArgumentOutOfRangeException("lineNumber", lineNumber,
                    "Should be between 0 and 3 included");
            }
            return (bool)InvokeMethod(new Func<int, String, bool>(NativeMethods.MonoSetText), lineNumber, text);
        }

        public bool colorSetBackground(byte[] colorBitmap)
        {
            return (bool)InvokeMethod(new Func<byte[], bool>(NativeMethods.ColorSetBackground), colorBitmap);
        }

        public bool colorSetTitle(String text, int red, int green, int blue)
        {
            return (bool)InvokeMethod(new Func<String, int, int, int, bool>(NativeMethods.ColorSetTitle),
                text, red, green, blue);
        }

        public bool colorSetText(int lineNumber, String text, int red, int green, int blue)
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
