using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Logitech_LCD.Utils;
using Control = System.Windows.Controls.Control;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace Logitech_LCD.Applets
{
    /// <summary>
    /// Logique d'interaction pour BaseWPFApplet.xaml
    /// </summary>
    public partial class BaseWPFApplet
    {
        private readonly int _height;
        private readonly int _width;
        private readonly LcdType? _lcdType;

        //Timers
        private Timer _updatetTimer;
        private Timer _buttonCheckTimer;



        public event EventHandler DataUpdate
        {
            add
            {
                AddHandler(DataUpdateEvent, value);
            }

            remove
            {
                RemoveHandler(DataUpdateEvent, value);
            }
        }


        #region EventDefinitions


        public static readonly RoutedEvent DataUpdateEvent =
            EventManager.RegisterRoutedEvent("DataUpdateEvent", RoutingStrategy.Direct,
                typeof(EventHandler), typeof(BaseWPFApplet));

        public static readonly RoutedEvent LcdColorLeftButtonPressedEvent =
            EventManager.RegisterRoutedEvent("LcdColorLeftButtonPressedEvent", RoutingStrategy.Direct,
                typeof(EventHandler), typeof(BaseWPFApplet));

        public static readonly RoutedEvent LcdColorRightButtonPressedEvent =
            EventManager.RegisterRoutedEvent("LcdColorRightButtonPressedEvent", RoutingStrategy.Direct,
                typeof(EventHandler), typeof(BaseWPFApplet));

        public static readonly RoutedEvent LcdColorUpButtonPressedEvent =
            EventManager.RegisterRoutedEvent("LcdColorUpButtonPressedEvent", RoutingStrategy.Direct,
                typeof(EventHandler), typeof(BaseWPFApplet));

        public static readonly RoutedEvent LcdColorDownButtonPressedEvent =
            EventManager.RegisterRoutedEvent("LcdColorDownButtonPressedEvent", RoutingStrategy.Direct,
                typeof(EventHandler), typeof(BaseWPFApplet));

        public static readonly RoutedEvent LcdColorOkButtonPressedEvent =
            EventManager.RegisterRoutedEvent("LcdColorOkButtonPressedEvent", RoutingStrategy.Direct,
                typeof(EventHandler), typeof(BaseWPFApplet));

        public static readonly RoutedEvent LcdColorCancelButtonPressedEvent =
            EventManager.RegisterRoutedEvent("LcdColorCancelButtonPressedEvent", RoutingStrategy.Direct,
                typeof(EventHandler), typeof(BaseWPFApplet));

        public static readonly RoutedEvent LcdColorMenuButtonPressedEvent =
            EventManager.RegisterRoutedEvent("LcdColorMenuButtonPressedEvent", RoutingStrategy.Direct,
                typeof(EventHandler), typeof(BaseWPFApplet));

        public static readonly RoutedEvent LcdMonoButton0PressedEvent =
            EventManager.RegisterRoutedEvent("LcdMonoButton0PressedEvent", RoutingStrategy.Direct,
                typeof(EventHandler), typeof(BaseWPFApplet));

        public static readonly RoutedEvent LcdMonoButton1PressedEvent =
            EventManager.RegisterRoutedEvent("LcdMonoButton1PressedEvent", RoutingStrategy.Direct,
                typeof(EventHandler), typeof(BaseWPFApplet));

        public static readonly RoutedEvent LcdMonoButton2PressedEvent =
            EventManager.RegisterRoutedEvent("LcdMonoButton2PressedEvent", RoutingStrategy.Direct,
                typeof(EventHandler), typeof(BaseWPFApplet));

        public static readonly RoutedEvent LcdMonoButton3PressedEvent =
            EventManager.RegisterRoutedEvent("LcdMonoButton3PressedEvent", RoutingStrategy.Direct,
                typeof(EventHandler), typeof(BaseWPFApplet));

        #endregion

        #region LcdColorButtonEventHandlers

        //Color LCD button events
        public event EventHandler LcdColorLeftButtonPressed
        {
            add
            {
                AddHandler(LcdColorLeftButtonPressedEvent, value);
            }

            remove
            {
                RemoveHandler(LcdColorLeftButtonPressedEvent, value);
            }
        }

        public event EventHandler LcdColorRightButtonPressed
        {
            add
            {
                AddHandler(LcdColorRightButtonPressedEvent, value);
            }
            remove
            {
                RemoveHandler(LcdColorRightButtonPressedEvent, value);
            }
        }
        public event EventHandler LcdColorUpButtonPressed
        {
            add
            {
                AddHandler(LcdColorUpButtonPressedEvent, value);
            }
            remove
            {
                RemoveHandler(LcdColorUpButtonPressedEvent, value);
            }
        }
        public event EventHandler LcdColorDownButtonPressed
        {
            add
            {
                AddHandler(LcdColorDownButtonPressedEvent, value);
            }
            remove
            {
                RemoveHandler(LcdColorDownButtonPressedEvent, value);
            }
        }

        public event EventHandler LcdColorOkButtonPressed
        {
            add
            {
                AddHandler(LcdColorOkButtonPressedEvent, value);
            }
            remove
            {
                RemoveHandler(LcdColorOkButtonPressedEvent, value);
            }
        }

        public event EventHandler LcdColorCancelButtonPressed
        {
            add
            {
                AddHandler(LcdColorCancelButtonPressedEvent, value);
            }
            remove
            {
                RemoveHandler(LcdColorCancelButtonPressedEvent, value);
            }
        }

        public event EventHandler LcdColorMenuButtonPressed
        {
            add
            {
                AddHandler(LcdColorMenuButtonPressedEvent, value);
            }
            remove
            {
                RemoveHandler(LcdColorMenuButtonPressedEvent, value);
            }
        }


        #endregion
        #region LcdMonoButtonEventHandlers
        //Mono LCD Button Events
        public event EventHandler LcdMonoButton0Pressed
        {
            add
            {
                AddHandler(LcdMonoButton0PressedEvent, value);
            }
            remove
            {
                RemoveHandler(LcdMonoButton0PressedEvent, value);
            }
        }

        public event EventHandler LcdMonoButton1Pressed
        {
            add
            {
                this.AddHandler(LcdMonoButton1PressedEvent, value);
            }
            remove
            {
                RemoveHandler(LcdMonoButton1PressedEvent, value);
            }
        }

        public event EventHandler LcdMonoButton2Pressed
        {
            add
            {
                AddHandler(LcdMonoButton2PressedEvent, value);
            }
            remove
            {
                RemoveHandler(LcdMonoButton2PressedEvent, value);
            }
        }

        public event EventHandler LcdMonoButton3Pressed
        {
            add
            {
                AddHandler(LcdMonoButton3PressedEvent, value);
            }
            remove
            {
                RemoveHandler(LcdMonoButton3PressedEvent, value);
            }
        }

        #endregion


        public BaseWPFApplet()
            : this(StaticMethods.DetectLcdType())
        {
        }


        public BaseWPFApplet(LcdType? lcdType)
        {
            if (lcdType != null)
            {
                _lcdType = lcdType;

                _updatetTimer = new Timer { Interval = 100 / 6 };
                _updatetTimer.Tick += delegate
                {
                    Dispatcher.Invoke(UpdateGraphics);
                };

                _buttonCheckTimer = new Timer { Interval = 200 };
                _buttonCheckTimer.Tick += CheckButtons;

                if (lcdType == LcdType.Color)
                {
                    Height = _height = (int)ColorBitmap.Height;
                    Width = _width = (int)ColorBitmap.Width;
                }
                else if (lcdType == LcdType.Mono)
                {
                    Height = _height = (int)MonoBitmap.Height;
                    Width = _width = (int)MonoBitmap.Width;
                }
                else
                {
                    throw new ArgumentException("Bad LCD Type", "lcdType");
                }
                _updatetTimer.Start();
                _buttonCheckTimer.Start();
            }

            InitializeComponent();
        }


        private void CheckButtons(object sender, EventArgs e)
        {
            if (LogitechLcd.Instance.IsButtonPressed(Buttons.ColorLeft))
            {
                RaiseEvent(new RoutedEventArgs(LcdColorLeftButtonPressedEvent));
            }
            else if (LogitechLcd.Instance.IsButtonPressed(Buttons.ColorRight))
            {
                RaiseEvent(new RoutedEventArgs(LcdColorLeftButtonPressedEvent));
            }
            else if (LogitechLcd.Instance.IsButtonPressed(Buttons.ColorUp))
            {
                RaiseEvent(new RoutedEventArgs(LcdColorLeftButtonPressedEvent));
            }
            else if (LogitechLcd.Instance.IsButtonPressed(Buttons.ColorDown))
            {
                RaiseEvent(new RoutedEventArgs(LcdColorLeftButtonPressedEvent));
            }
            else if (LogitechLcd.Instance.IsButtonPressed(Buttons.ColorOk))
            {
                RaiseEvent(new RoutedEventArgs(LcdColorLeftButtonPressedEvent));
            }
            else if (LogitechLcd.Instance.IsButtonPressed(Buttons.ColorCancel))
            {
                RaiseEvent(new RoutedEventArgs(LcdColorLeftButtonPressedEvent));
            }
            else if (LogitechLcd.Instance.IsButtonPressed(Buttons.ColorMenu))
            {
                RaiseEvent(new RoutedEventArgs(LcdColorLeftButtonPressedEvent));
            }
            else if (LogitechLcd.Instance.IsButtonPressed(Buttons.MonoButton0))
            {
                RaiseEvent(new RoutedEventArgs(LcdColorLeftButtonPressedEvent));
            }
            else if (LogitechLcd.Instance.IsButtonPressed(Buttons.MonoButton1))
            {
                RaiseEvent(new RoutedEventArgs(LcdColorLeftButtonPressedEvent));
            }
            else if (LogitechLcd.Instance.IsButtonPressed(Buttons.MonoButton2))
            {
                RaiseEvent(new RoutedEventArgs(LcdColorLeftButtonPressedEvent));
            }
            else if (LogitechLcd.Instance.IsButtonPressed(Buttons.MonoButton3))
            {
                RaiseEvent(new RoutedEventArgs(LcdColorLeftButtonPressedEvent));
            }
        }


        private Bitmap DrawToBitmap(Rectangle rect)
        {

            Bitmap bitmap;

            var renderTarget = new RenderTargetBitmap(rect.Width, rect.Height, 96, 96, PixelFormats.Pbgra32);

            var visual = new DrawingVisual();

            using (DrawingContext context = visual.RenderOpen())
            {
                var brush = new VisualBrush(this);
                context.DrawRectangle(brush, null, new Rect(new Point(), new Size(rect.Width, rect.Height)));
            }

            renderTarget.Render(visual);

            using (var stream = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderTarget));
                encoder.Save(stream);
                bitmap = new Bitmap(stream);
            }

            return bitmap;
        }


        private void UpdateGraphics()
        {
            RaiseEvent(new RoutedEventArgs(DataUpdateEvent));

            var bm = DrawToBitmap(new Rectangle(0, 0, _width, _height));

            byte[] pixels = new byte[_width * _height * 4];

            BitmapData bitmapData = bm.LockBits(
                new Rectangle(0, 0, _width, _height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

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
    }
}
