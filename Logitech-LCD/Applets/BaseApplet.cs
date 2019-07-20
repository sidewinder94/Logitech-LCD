using Logitech_LCD.Exceptions;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Logitech_LCD.Utils;

namespace Logitech_LCD.Applets
{
    public class BaseApplet : UserControl, IActivableApplet
    {
        private readonly int _height;
        private readonly int _width;
        private readonly LcdType? _lcdType;

        public event EventHandler DataUpdate;

        //Color LCD button events
        public event EventHandler LcdColorLeftButtonPressed;
        public event EventHandler LcdColorRightButtonPressed;
        public event EventHandler LcdColorUpButtonPressed;
        public event EventHandler LcdColorDownButtonPressed;
        public event EventHandler LcdColorOkButtonPressed;
        public event EventHandler LcdColorCancelButtonPressed;
        public event EventHandler LcdColorMenuButtonPressed;

        //Mono LCD Button Events
        public event EventHandler LcdMonoButton0Pressed;
        public event EventHandler LcdMonoButton1Pressed;
        public event EventHandler LcdMonoButton2Pressed;
        public event EventHandler LcdMonoButton3Pressed;

        //Timers
        private System.Timers.Timer _updateTimer;
        private System.Timers.Timer _buttonCheckTimer;

        /// <inheritdoc cref="IActivableApplet.IsActive"/>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or Sets the graphics update rate same unit as <see cref="Timer.Interval"/>
        /// </summary>
        public double UpdateRate
        {
            get { return this._updateTimer.Interval; }
            set { this._updateTimer.Interval = value; }
        }


        public BaseApplet()
            : this(StaticMethods.DetectLcdType())
        {

        }

        public BaseApplet(LcdType? lcdType)
        {
            this.IsActive = true;

            if (lcdType != null)
            {
                _lcdType = lcdType;

                this._updateTimer = new System.Timers.Timer
                {
                    Interval = 100 / 6,
                    AutoReset = true
                };
                this._updateTimer.Elapsed += UpdateGraphics;

                _buttonCheckTimer = new System.Timers.Timer
                {
                    Interval = 200,
                    AutoReset = true
                };
                _buttonCheckTimer.Elapsed += CheckButtons;

                if (lcdType == LcdType.Color)
                {
                    _height = (int)ColorBitmap.Height;
                    _width = (int)ColorBitmap.Width;
                }
                else if (lcdType == LcdType.Mono)
                {
                    _height = (int)MonoBitmap.Height;
                    _width = (int)MonoBitmap.Width;
                }
                else
                {
                    throw new ArgumentException("Bad LCD Type", "lcdType");
                }
                DataUpdate += OnDataUpdate;
                this._updateTimer.Start();
                _buttonCheckTimer.Start();
            }
        }

        /// <summary>
        /// Finalizer for the type <see cref="BaseApplet"/>
        /// </summary>
        ~BaseApplet()
        {
            this.Dispose(false);
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            try
            {
                this._buttonCheckTimer.Stop();
                this._updateTimer.Stop();
            }
            catch (Exception)
            {
                // ignored
            }

            if (disposing)
            {
                this._buttonCheckTimer.Dispose();
                this._updateTimer.Dispose();
            }
        }

        private void CheckButtons(object sender, EventArgs e)
        {
            if (!this.IsActive) return;

            if (LogitechLcd.Instance.IsButtonPressed(Buttons.ColorLeft))
            {
                if (LcdColorLeftButtonPressed != null)
                {
                    LcdColorLeftButtonPressed(this, EventArgs.Empty);
                }
            }
            else if (LogitechLcd.Instance.IsButtonPressed(Buttons.ColorRight))
            {
                if (LcdColorRightButtonPressed != null)
                {
                    LcdColorRightButtonPressed(this, EventArgs.Empty);
                }
            }
            else if (LogitechLcd.Instance.IsButtonPressed(Buttons.ColorUp))
            {
                if (LcdColorUpButtonPressed != null)
                {
                    LcdColorUpButtonPressed(this, EventArgs.Empty);
                }
            }
            else if (LogitechLcd.Instance.IsButtonPressed(Buttons.ColorDown))
            {
                if (LcdColorDownButtonPressed != null)
                {
                    LcdColorDownButtonPressed(this, EventArgs.Empty);
                }
            }
            else if (LogitechLcd.Instance.IsButtonPressed(Buttons.ColorOk))
            {
                if (LcdColorOkButtonPressed != null)
                {
                    LcdColorOkButtonPressed(this, EventArgs.Empty);
                }
            }
            else if (LogitechLcd.Instance.IsButtonPressed(Buttons.ColorCancel))
            {
                if (LcdColorCancelButtonPressed != null)
                {
                    LcdColorCancelButtonPressed(this, EventArgs.Empty);
                }
            }
            else if (LogitechLcd.Instance.IsButtonPressed(Buttons.ColorMenu))
            {
                if (LcdColorMenuButtonPressed != null)
                {
                    LcdColorMenuButtonPressed(this, EventArgs.Empty);
                }
            }
            else if (LogitechLcd.Instance.IsButtonPressed(Buttons.MonoButton0))
            {
                if (LcdMonoButton0Pressed != null)
                {
                    LcdMonoButton0Pressed(this, EventArgs.Empty);
                }
            }
            else if (LogitechLcd.Instance.IsButtonPressed(Buttons.MonoButton1))
            {
                if (LcdMonoButton1Pressed != null)
                {
                    LcdMonoButton1Pressed(this, EventArgs.Empty);
                }
            }
            else if (LogitechLcd.Instance.IsButtonPressed(Buttons.MonoButton2))
            {
                if (LcdMonoButton2Pressed != null)
                {
                    LcdMonoButton2Pressed(this, EventArgs.Empty);
                }
            }
            else if (LogitechLcd.Instance.IsButtonPressed(Buttons.MonoButton3))
            {
                if (LcdMonoButton3Pressed != null)
                {
                    LcdMonoButton3Pressed(this, EventArgs.Empty);
                }
            }
        }

        private void UpdateGraphics(object sender, EventArgs e)
        {
            if (!this.IsActive) return;

            this.DataUpdate(this, EventArgs.Empty);
            PixelFormat format = PixelFormat.Format32bppArgb;

            Bitmap bm = new Bitmap(_width, _height, format);
            this.DrawToBitmap(bm, new Rectangle(0, 0, _width, _height));

            byte[] pixels = new byte[_width * _height * 4];

            BitmapData bitmapData = bm.LockBits(
                new Rectangle(0, 0, _width, _height),
                ImageLockMode.ReadOnly,
                format);

            Marshal.Copy(bitmapData.Scan0, pixels, 0, pixels.Length);

            bm.UnlockBits(bitmapData);

            if (_lcdType == LcdType.Color)
            {
                LogitechLcd.Instance.ColorSetBackground(pixels);
            }
            else
            {
                LogitechLcd.Instance.MonoSetBackground(StaticMethods.ConvertToMonochrome(pixels));
            }
            LogitechLcd.Instance.Update();
        }


        protected virtual void OnDataUpdate(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
