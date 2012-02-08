using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Scheduler
{
    public class Scheduler
    {
        /// <summary>
        /// The timers used to periodically check if some plugin is due to run.
        /// </summary>
        private System.Timers.Timer _timer;
        /// <summary>
        /// The main schedule containing all the scheduled plugins.
        /// </summary>
        private List<PluginSchedule> _schedules;

        /// <summary>
        /// Constructor that will load the schedule for plugins that are scheduled to run Today
        /// </summary>
        public Scheduler()
        {
            _schedules = new List<PluginSchedule>();
            LoadScheduleForToday();
        }

        /// <summary>
        /// Loads the schedule for plugins that should run today.
        /// </summary>
        private void LoadSchedule()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Config"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"select 
	                                                        p.PluginId,
	                                                        p.AssemblyName,
	                                                        p.PluginName,
	                                                        s.ExecutionTime,
	                                                        par.ParameterString
                                                        from 
	                                                        Plugins p
                                                        inner join tx_DailySchedule s
	                                                        on p.PluginID = s.PluginId
                                                        inner join PluginParameters par
	                                                        on p.PluginId = par.PluginId",
                                        conn))
                {
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    // The actuall loading of the plugins is not done until its time to execute them.
                    while (reader.Read())
                    {
                        _schedules.Add(new PluginSchedule(
                            reader["AssemblyName"].ToString(),
                            connectionString,
                            (DateTime)reader["ExecutionTime"],
                            reader["ParameterString"].ToString()
                            ));
                    }

                }
            }

            // Add event handlers
            foreach (var item in _schedules)
            {
                //item.Plugin.OnProgress += Plugin_OnProgress;
                //item.Plugin.OnError += Plugin_OnError;
                //item.Plugin.OnCompleted += Plugin_OnCompleted;
            }
        }

        

        private void LoadScheduleForToday()
        {
            // fyll _schedules från databasen med dagens händelser
            //PluginManager.Instance.RefreshPlugins();
            //foreach (var item in PluginManager.Instance.Plugins)
            //{

            //}
            LoadSchedule();
        }

        public void Start()
        {
            _timer = new System.Timers.Timer();
            _timer.Interval = 1000;
            _timer.AutoReset = true;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);

            _timer.Start();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            List<PluginSchedule> toBeRemoved = new List<PluginSchedule>();
            foreach (var item in _schedules)
            {
                DateTime dateTimeNow = DateTime.Now;
                if (item.ExecutionStartTime.IsTimeToRun(dateTimeNow))
                {
                    item.RunScheduledPlugin();
                    toBeRemoved.Add(item);
                }
            }
            foreach (var item in toBeRemoved)
            {
                _schedules.Remove(item);
            }
        }

        public void Stop()
        {
            foreach (var item in _schedules)
            {
                item.Dispose();
            }
            //PluginManager.Instance.Plugins.Clear();
        }
    }
}
