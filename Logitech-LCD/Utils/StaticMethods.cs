using Logitech_LCD.Exceptions;

namespace Logitech_LCD.Utils
{
    public static class StaticMethods
    {
        internal static byte[] ConvertToMonochrome(byte[] bitmap)
        {
            byte[] monochromePixels = new byte[bitmap.Length / 4];

            for (int ii = 0; ii < (int)(MonoBitmap.Height) * (int)MonoBitmap.Width; ii++)
            {
                monochromePixels[ii] = bitmap[ii * 4];
            }

            return monochromePixels;
        }

        internal static LcdType? DetectLcdType()
        {
            try
            {
                if (LogitechLcd.Instance.IsConnected(LcdType.Color))
                {
                    return LcdType.Color;
                }
                else if (LogitechLcd.Instance.IsConnected(LcdType.Mono))
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
                LogitechLcd.Instance.Init("", LcdType.Color | LcdType.Mono);
                return DetectLcdType();
#else
                return null;
#endif
            }
        }
    }
}