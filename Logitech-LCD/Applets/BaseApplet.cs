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

        private Timer _updatetTimer;

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
            }
        }

        private void updateGraphics(object sender, EventArgs e)
        {
            this.dataUpdate(this, EventArgs.Empty);
            PixelFormat format;
            if (_bpp == ((int)MonoBitmap.Bpp))
            {
                format = PixelFormat.Format1bppIndexed;
            }
            else
            {
                format = PixelFormat.Format32bppArgb;
            }
            Bitmap _bm = new Bitmap(_width, _height, format);
            this.DrawToBitmap(_bm, new Rectangle(0, 0, _width, _height));

            byte[] pixels = new byte[_width * _height * _bpp];

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

        protected virtual void OnDataUpdate(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
