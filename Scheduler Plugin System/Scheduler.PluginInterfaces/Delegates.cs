using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler.PluginInterfaces
{
    public delegate void ErrorHandler(string message, Severity errorSeverity);
    public delegate void CompletedHandler(string message);
    public delegate void ProgressReportHandler(string message, int percentCompleted);
}
