using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Logitech_LCD.Applets
{
    public abstract class BaseApplet : UserControl
    {
        private int _height;
        private int _width;
        private int _bpp;
        private LcdType _lcdType;

        private Timer _updatetTimer;
        public BaseApplet(LcdType lcdType)
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

            _updatetTimer.Start();

        }

        private void updateGraphics(object sender, EventArgs e)
        {
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
        }
    }
}
