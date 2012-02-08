using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler.PluginInterfaces
{
    /// <summary>
    /// A level of error severity to indicate how bad the error is
    /// </summary>
    public enum Severity
    {
        /// <summary>
        /// An error occured but it's been handled or does not matter for the quality of the plugin task
        /// </summary>
        Message, 
        /// <summary>
        /// An error occured that can become a problem later. It does not change the quality of the plugin task
        /// </summary>
        Warning, 
        /// <summary>
        /// An error occured that affects the quality of the plugin task
        /// </summary>
        Error
    }
}
