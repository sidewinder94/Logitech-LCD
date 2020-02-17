using Logitech_LCD.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Logitech_LCD
{
    /// <summary>
    /// The class to effectively use to access the SDK functions
    /// </summary>
    public class LogitechLcd
    {
        public static List<string> SearchPaths = new List<string>()
        {
            $"C:\\Program Files\\Logitech Gaming Software\\SDK\\LCD\\{(Environment.Is64BitOperatingSystem && Environment.Is64BitProcess ? "x64" : "x86")}",
            $"C:\\Program Files (x86)\\Logitech Gaming Software\\SDK\\LCD\\{(Environment.Is64BitOperatingSystem && Environment.Is64BitProcess ? "x64" : "x86")}",
        };

        public static HashSet<string> DllPossibleNames = new HashSet<string>()
        {
            "LogitechLcd.dll",
            "LogitechLcdEnginesWrapper.dll"
        };

        private static readonly string LocalDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "Logitech-LCD");

        private static readonly string LocalName = Path.Combine(LocalDirectory, "LogitechLcd.dll");

        #region Singleton implementation
        public static LogitechLcd Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        private class Nested
        {
            internal static readonly LogitechLcd instance = new LogitechLcd();
        }
        #endregion

        #region constructor / destructor

        private static bool IsProgramDataPresent()
        {
            var local = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "Logitech-LCD");

            try
            {
                if (Directory.Exists(local))
                {
                    if (File.Exists(LocalName))
                    {
                        return true;
                    }

                    return false;
                }

                Directory.CreateDirectory(local);
            }
            catch
            {
                // ignored
            }

            return false;
        }

        private static void CopyToProgramDataAndLoad(string sourceDll)
        {
            try
            {
                if (!Directory.Exists(LocalDirectory))
                {
                    Directory.CreateDirectory(LocalDirectory);
                }

                File.Copy(sourceDll, LocalName);

                NativeMethods.SetDllDirectoryA(LocalDirectory);
            }
            catch
            {
                NativeMethods.SetDllDirectoryA(Path.GetDirectoryName(sourceDll));
            }
        }

        private LogitechLcd()
        {
            if (IsProgramDataPresent())
            {
                NativeMethods.SetDllDirectoryA(LocalDirectory);
                return;
            }

            var localDlls = Directory.GetFiles(".", "*.dll", SearchOption.AllDirectories);

            var local = localDlls.FirstOrDefault(ldl => DllPossibleNames.Contains(Path.GetFileName(ldl)));

            if (local != null)
            {
                CopyToProgramDataAndLoad(local);
                return;
            }

            foreach (var posiblePath in SearchPaths.Where(Directory.Exists))
            {
                var pathDlls = Directory.GetFiles(posiblePath, "*.dll", SearchOption.TopDirectoryOnly);

                var path = pathDlls.FirstOrDefault(ldl => DllPossibleNames.Contains(Path.GetFileName(ldl)));

                if (path != null)
                {
                    CopyToProgramDataAndLoad(path);
                    return;
                }
            }
        }

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

        public bool IsInit { get; private set; }

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
