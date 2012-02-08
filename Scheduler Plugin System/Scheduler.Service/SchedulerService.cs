using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Scheduler.Service
{
    partial class SchedulerService : ServiceBase
    {
        private Scheduler _scheduler;
        public SchedulerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _scheduler = new Scheduler();
            _scheduler.Start();
        }

        protected override void OnStop()
        {
            _scheduler.Stop();
        }
    }
}
