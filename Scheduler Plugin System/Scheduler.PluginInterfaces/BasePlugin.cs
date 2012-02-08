using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler.PluginInterfaces
{
    public abstract class BasePlugin : ISchedulePlugin
    {
        /// <summary>
        /// An identifier to know what plugin this is. Used to implement <see cref="CompareTo"/>, <see cref="Equals"/> etc.
        /// </summary>
        protected string _identifier;

        /// <summary>
        /// The main function that the scheduler will invoke at the scheduled time.
        /// </summary>
        /// <param name="date">Scheduler will normally send DateTime.Now</param>
        /// <param name="startArgument">the string of parameters that the plugin might need to perform it's task</param>
        public abstract void Run(string connectionString, DateTime date, string startArgument);

        public string Identifier
        {
            get { return _identifier; }
        }

        /// <summary>
        /// Sends the OnError Event
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="errorSeverity">An indication on what kind of error this is</param>
        protected void ReportError(string message, Severity errorSeverity)
        {
            if (OnError != null)
                OnError(message, errorSeverity);
        }
        /// <summary>
        /// Sends the OnProgress Event.
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="percentCompleted">Indicates how much progress have been made</param>
        protected void ReportProgress(string message, int percentCompleted)
        {
            if (OnProgress != null)
                OnProgress(message, percentCompleted);
        }
        /// <summary>
        /// Sends the OnProgress Event
        /// </summary>
        /// <param name="message">The messeage to send</param>
        protected void ReportProgress(string message)
        {
            ReportProgress(message, 0);
        }

        /// <summary>
        /// Sends the OnCompleted Event
        /// </summary>
        /// <param name="message">The message to send</param>
        protected void ReportCompleted(string message)
        {
            if (OnCompleted != null)
                OnCompleted(message);
        }

        /// <summary>
        /// Compare this instance to another object.
        /// </summary>
        /// <param name="obj">The other object to compare this instance with.</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            return _identifier.CompareTo(obj);
        }
        /// <summary>
        /// Checks if this object is value equal as other object
        /// </summary>
        /// <param name="obj">Other object to compare</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return _identifier.Equals(obj);
        }

        /// <summary>
        /// Get the hashcode of this object
        /// </summary>
        /// <returns>the hashcode of the object</returns>
        public override int GetHashCode()
        {
            return _identifier.GetHashCode();
        }

        /// <summary>
        /// Disposes of the instance
        /// </summary>
        public void Dispose()
        {
            OnError = null;
            OnCompleted = null;
            OnProgress = null;
            _identifier = null;
        }

        /// <summary>
        /// Checks if two instances of plugins are the same, reference equal and value equal.
        /// </summary>
        /// <param name="a">first instance</param>
        /// <param name="b">second instance</param>
        /// <returns></returns>
        public static bool operator ==(BasePlugin a, BasePlugin b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a._identifier == b._identifier;
        }
        /// <summary>
        /// Not equal <see cref="=="/>
        /// </summary>
        /// <param name="a">first instance</param>
        /// <param name="b">second instance</param>
        /// <returns></returns>
        public static bool operator !=(BasePlugin a, BasePlugin b)
        {
            return !(a == b);
        }

        // Events are described in ISchedulePlugin interface
        public event ErrorHandler OnError;
        public event CompletedHandler OnCompleted;
        public event ProgressReportHandler OnProgress;
       
    }
}
