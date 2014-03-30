using System;

namespace Logitech_LCD.Exceptions
{
    [Serializable]
    public class LcdNotInitializedException : Exception
    {
        public LcdNotInitializedException() :
            base("The Logitech SDK must be Initialized prior to any method use")
        {
        }

        public LcdNotInitializedException(string message)
            : base(message)
        {
        }

        public LcdNotInitializedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
