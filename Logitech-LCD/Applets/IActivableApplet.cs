using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitech_LCD.Applets
{
    /// <summary>
    /// Interface describing a way to disable an applet update.
    /// </summary>
    interface IActivableApplet
    {
        /// <summary>
        /// Gets or sets a value indicating if an applet should listen to button presses/send visual updates or not.
        /// </summary>
        /// <remarks>Defaults to <code>true</code></remarks>
        bool IsActive { get; set; }
    }
}
