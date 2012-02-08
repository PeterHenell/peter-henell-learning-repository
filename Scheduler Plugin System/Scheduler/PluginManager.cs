using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using Scheduler.PluginInterfaces;
using System.Data.SqlClient;
using System.Threading;

namespace Scheduler
{
    /// <summary>
    /// Responsible for loading Plugins from assemblies
    /// </summary>
    public class PluginManager
    {
        /// <summary>
        /// Singleton Instance access to the object
        /// </summary>
        private static PluginManager _instance;

        /// <summary>
        /// Singleton Instance access to the object
        /// </summary>
        public static PluginManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PluginManager();
                return _instance;
            }
        }
     
        /// <summary>
        /// private constructor for Singleton pattern
        /// </summary>
        PluginManager()
        {
        }

        

        /// <summary>
        /// Will load and create an instance of a ISchedulePlugin from assembly file name
        /// </summary>
        /// <remarks>
        /// If the assembly contains more than one plugin only the first one found will be returned.
        /// </remarks>
        /// <param name="assemblyName">The name of the Assembly to load the plugin from</param>
        /// <returns>An instance of the plugin or null if no plugins were found</returns>
        public ISchedulePlugin LoadPlugin(string assemblyName)
        {
            try
            {
                string assemblyFileName = System.Environment.CurrentDirectory + "\\Plugins\\" + assemblyName;
                Assembly asm = Assembly.LoadFrom(assemblyFileName);
                Type[] types = asm.GetTypes();
                foreach (Type thisType in types)
                {
                    Type[] interfaces = thisType.GetInterfaces();
                    foreach (Type thisInterface in interfaces)
                    {
                        if (thisInterface.Name == "ISchedulePlugin")
                        {
                            if (thisInterface.Namespace == "Scheduler.PluginInterfaces")
                            {
                                //-- Load the object
                                object obj = asm.CreateInstance(thisType.Namespace + "." + thisType.Name);
                                return obj as ISchedulePlugin;
                            }
                        }
                    }

                }
            }
            catch
            {
                // This should be logged
            }

            return null;
        }

        /// <summary>
        /// Checks the Plugin folder for the specified assembly. 
        /// If the assembly is found it is loaded and checked weather it contains a plugin or not.
        /// The plugin need to implement the ISchedulePlugin interface to be a valid plugin.
        /// </summary>
        /// <param name="assemblyName">The Dll that should be tested</param>
        /// <returns>true if the DLL can be found and it contains a Plugin, else false</returns>
        public bool CanFindPlugin(string assemblyName)
        {
            if (LoadPlugin(assemblyName) != null)
                return true;
            else 
                return false;
        }
    }
}



