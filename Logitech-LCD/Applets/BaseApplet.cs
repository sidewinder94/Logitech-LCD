using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Logitech_LCD.Exceptions;

namespace Logitech_LCD.Applets
{
    public class BaseApplet : UserControl
    {
        protected int _height;
        protected int _width;
        protected int _bpp;
        protected LcdType? _lcdType;

        public event EventHandler dataUpdate;

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
        private Timer _updatetTimer;
        private Timer _buttonCheckTimer;

        private static LcdType? detectLcdType()
        {
            try
            {
                if (LogitechLcd.Instance.isConnected(LcdType.Color))
                {
                    return LcdType.Color;
                }
                else if (LogitechLcd.Instance.isConnected(LcdType.Mono))
                {
                    return LcdType.Mono;
                }
                else
                {
                    return null;
                }
            }
            catch (LcdNotInitializedException)
            {
#if !DEBUG
                //Emergency initialization
                LogitechLcd.Instance.init("", LcdType.Color | LcdType.Mono);
                return detectLcdType();
#else
                return null;
#endif
            }
        }

        public BaseApplet()
            : this(detectLcdType())
        {

        }

        public BaseApplet(LcdType? lcdType)
        {
            if (lcdType != null)
            {
                _lcdType = lcdType;

                _updatetTimer = new Timer();
                _updatetTimer.Interval = 100 / 6;
                _updatetTimer.Tick += updateGraphics;

                _buttonCheckTimer = new Timer();
                _buttonCheckTimer.Interval = 200;
                _buttonCheckTimer.Tick += checkButtons;

                if (lcdType == LcdType.Color)
                {
                    _height = (int)ColorBitmap.Height;
                    _width = (int)ColorBitmap.Width;
                    _bpp = (int)ColorBitmap.Bpp;
                }
                else if (lcdType == LcdType.Mono)
                {
                    _height = (int)MonoBitmap.Height;
                    _width = (int)MonoBitmap.Width;
                    _bpp = (int)MonoBitmap.Bpp;
                }
                else
                {
                    throw new ArgumentException("Bad LCD Type", "lcdType");
                }
                dataUpdate += OnDataUpdate;
                _updatetTimer.Start();
                _buttonCheckTimer.Start();
            }
        }

        private void checkButtons(object sender, EventArgs e)
        {
            if (LogitechLcd.Instance.isButtonPressed(Buttons.ColorLeft))
            {
                LcdColorLeftButtonPressed(this, EventArgs.Empty);
            }
            else if (LogitechLcd.Instance.isButtonPressed(Buttons.ColorRight))
            {
                LcdColorRightButtonPressed(this, EventArgs.Empty);
            }
            else if (LogitechLcd.Instance.isButtonPressed(Buttons.ColorUp))
            {
                LcdColorUpButtonPressed(this, EventArgs.Empty);
            }
            else if (LogitechLcd.Instance.isButtonPressed(Buttons.ColorDown))
            {
                LcdColorDownButtonPressed(this, EventArgs.Empty);
            }
            else if (LogitechLcd.Instance.isButtonPressed(Buttons.ColorOK))
            {
                LcdColorOkButtonPressed(this, EventArgs.Empty);
            }
            else if (LogitechLcd.Instance.isButtonPressed(Buttons.ColorCancel))
            {
                LcdColorCancelButtonPressed(this, EventArgs.Empty);
            }
            else if (LogitechLcd.Instance.isButtonPressed(Buttons.ColorMenu))
            {
                LcdColorMenuButtonPressed(this, EventArgs.Empty);
            }
            else if (LogitechLcd.Instance.isButtonPressed(Buttons.MonoButton0))
            {
                LcdMonoButton0Pressed(this, EventArgs.Empty);
            }
            else if (LogitechLcd.Instance.isButtonPressed(Buttons.MonoButton1))
            {
                LcdMonoButton1Pressed(this, EventArgs.Empty);
            }
            else if (LogitechLcd.Instance.isButtonPressed(Buttons.MonoButton2))
            {
                LcdMonoButton2Pressed(this, EventArgs.Empty);
            }
            else if (LogitechLcd.Instance.isButtonPressed(Buttons.MonoButton3))
            {
                LcdMonoButton3Pressed(this, EventArgs.Empty);
            }
        }

        private void updateGraphics(object sender, EventArgs e)
        {
            this.dataUpdate(this, EventArgs.Empty);
            PixelFormat format = format = PixelFormat.Format32bppArgb;

            Bitmap _bm = new Bitmap(_width, _height, format);
            this.DrawToBitmap(_bm, new Rectangle(0, 0, _width, _height));

            byte[] pixels = new byte[_width * _height * 4];

            BitmapData bitmapData = _bm.LockBits(
                new Rectangle(0, 0, _width, _height),
                ImageLockMode.ReadOnly,
                format);

            Marshal.Copy(bitmapData.Scan0, pixels, 0, pixels.Length);

            _bm.UnlockBits(bitmapData);

            if (_lcdType == LcdType.Color)
            {
                LogitechLcd.Instance.colorSetBackground(pixels);
            }
            else
            {
                LogitechLcd.Instance.monoSetBackground(pixels);
            }
            LogitechLcd.Instance.update();
        }

        private byte[] convertToMonochrome(byte[] bitmap)
        {
            byte[] monochromePixels = new byte[bitmap.Length];

            for (int ii = 0; ii < (((int)(MonoBitmap.Height)) * ((int)MonoBitmap.Width) * 4); ii = ii + 4)
            {
                monochromePixels[ii] = bitmap[ii / 4];
            }

            return monochromePixels;
        }

        protected virtual void OnDataUpdate(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
