using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scheduler.PluginInterfaces;

namespace Scheduler
{
    /// <summary>
    /// Used to know when a Plugin is ready to run.
    /// </summary>
    public sealed class ExecutionTime
    {
        //int Hour { get; set; }
        //int Day { get; set; }
        //int Month { get; set; }
        //int WeekDay { get; set; }

        /// <summary>
        /// The date and time when the plugin should run
        /// </summary>
        DateTime _whenToRun;

        /// <summary>
        /// Creates the object with the suplied datetime as executiontime
        /// </summary>
        /// <param name="whenToRun">Date and time when the plugin should run</param>
        public ExecutionTime(DateTime whenToRun)
        {
            // Clone the date to run
            _whenToRun = new DateTime(whenToRun.Year, whenToRun.Month, whenToRun.Day);
        }

        /// <summary>
        /// The date and time when the plugin should run
        /// </summary>
        public DateTime WhenToRun
        {
            get
            {
                return _whenToRun;
            }
            set { _whenToRun = value; }
        }

        /// <summary>
        /// Checks if it is time for the plugin to Run based on the supplied datetime.
        /// </summary>
        /// <param name="dt">The date and time to compare to.</param>
        /// <returns>true if it's time to Run, else false</returns>
        public bool IsTimeToRun(DateTime dt)
        {
            return dt > _whenToRun;
        }
    }
}
