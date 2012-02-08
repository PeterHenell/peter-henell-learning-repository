using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Scheduler.PluginInterfaces
{

    public interface ISchedulePlugin : IComparable, IDisposable
    {
        /// <summary>
        /// Executes the main functionallity of the plugin
        /// </summary>
        /// <param name="connectionString">A connectionstring to the AS_config database where the plugin can find it's own connectionstrings</param>
        /// <param name="date">A date that can be used as a parameter to the task</param>
        /// <param name="startArgument">a string that can include the arguments needed to perform the intended task. 
        /// This value is configured in the AS_Config database</param>
        void Run(string connectionString, DateTime date, string startArgument);
        
        /// <summary>
        /// A name of the plugin so it can implement Equals() and CompareTo()
        /// </summary>
        string Identifier { get; }

        /// <summary>
        /// Disposing the object.
        /// </summary>
        void Dispose();

        /// <summary>
        /// To be invoked when there is an error occur
        /// </summary>
        event ErrorHandler OnError;
        /// <summary>
        /// To be invoked when the plugin have finished with no problem.
        /// </summary>
        event CompletedHandler OnCompleted;
        /// <summary>
        /// To be invoked when the plugin need to report some kind of progress. Used for logging
        /// </summary>
        event ProgressReportHandler OnProgress;
    }

   
}
