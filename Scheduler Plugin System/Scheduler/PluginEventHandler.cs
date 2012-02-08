using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler
{
    public static class PluginEventHandler
    {
        public static void Plugin_OnCompleted(string message)
        {
            Console.WriteLine("Completed " + message);
        }

        public static void Plugin_OnError(string message, PluginInterfaces.Severity errorSeverity)
        {
            Console.WriteLine(message + " " + errorSeverity.ToString());
        }

        public static void Plugin_OnProgress(string message, int percentCompleted)
        {
            Console.WriteLine(message);
        }
    }
}
