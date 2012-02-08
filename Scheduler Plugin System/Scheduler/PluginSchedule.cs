using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scheduler.PluginInterfaces;
using System.Threading;

namespace Scheduler
{
    public sealed class PluginSchedule : IDisposable
    {
        /// <summary>
        /// The plugin that should be invoked
        /// </summary>
        public ISchedulePlugin Plugin { get; set; }
        
        /// <summary>
        /// The time when the plugin should be started
        /// </summary>
        public ExecutionTime ExecutionStartTime { get; set; }
        /// <summary>
        /// The (main) thread that the plugin will run in.
        /// </summary>
        private Thread _scheduleThread { get; set; }

        /// <summary>
        /// String that can contain parameters needed for the plugin to run.
        /// </summary>
        string _parameterString;

        /// <summary>
        /// The name of the DLL that the plugin can be loaded from
        /// </summary>
        private string _assemblyName;

        string _connectionString;

        /// <summary>
        /// Constructor. Sets up all the needed information for the plugin to run
        /// </summary>
        /// <param name="assemblyName">the assembly file name where the plugin is located</param>
        /// <param name="connectionString">the connectionstring that the plugin can use to load additional configurations from</param>
        /// <param name="timeToRun">the time when the plugin should be started</param>
        /// <param name="parameterString">a string that can contain parameters needed for the plugin to run</param>
        public PluginSchedule(string assemblyName, string connectionString, DateTime timeToRun, string parameterString)
        {
            _assemblyName = assemblyName;
            ExecutionStartTime = new ExecutionTime(timeToRun);
            _parameterString = parameterString.Clone() as string;
            _connectionString = connectionString.Clone() as string;

            // Check if the plugin can be found. But do not load and save the reference for it.
            if (! PluginManager.Instance.CanFindPlugin(_assemblyName))
            {
                // Detta skall loggas istället
                throw new Exception(string.Format("could not load {0}", _assemblyName));
            }


            // When the scheduler starts this plugin, the plugin will load from its assembly and its Run-method is invoked.
            // This is done in a seperate thread to not disturb the scheduler.
            _scheduleThread = new Thread(delegate() 
                {
                    Plugin = PluginManager.Instance.LoadPlugin(_assemblyName);

                    Plugin.OnProgress += PluginEventHandler.Plugin_OnProgress;
                    Plugin.OnError += PluginEventHandler.Plugin_OnError;
                    Plugin.OnCompleted += PluginEventHandler.Plugin_OnCompleted;
                                        
                    Plugin.Run(_connectionString, DateTime.Now, _parameterString);

                    Plugin.OnProgress -= PluginEventHandler.Plugin_OnProgress;
                    Plugin.OnError -= PluginEventHandler.Plugin_OnError;
                    Plugin.OnCompleted -= PluginEventHandler.Plugin_OnCompleted;

                    Plugin.Dispose();
                });
        }

        /// <summary>
        /// Starts the thread that will load and run the plugin.
        /// </summary>
        public void RunScheduledPlugin()
        {
            _scheduleThread.Start();
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes of the object and all its children
        /// </summary>
        public void Dispose()
        {
            _scheduleThread.Abort();
            Plugin.Dispose();
            _scheduleThread = null;
        }

        #endregion
    }
}
