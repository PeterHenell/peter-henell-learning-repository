using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            Scheduler _scheduler = null;
            _scheduler = new Scheduler();
            _scheduler.Start();

            System.Threading.Thread.Sleep(1000000);
        }
    }
}
