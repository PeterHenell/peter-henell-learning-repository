using System.Collections;
using System;

namespace Scheduler.PluginInterfaces
{

    public class PluginCollection : CollectionBase
    {
        public int Add(ISchedulePlugin aIPlugin)
        {
            return InnerList.Add(aIPlugin);
        }

        public void Insert(int index, ISchedulePlugin aIPlugin)
        {
            InnerList.Insert(index, aIPlugin);
        }
        
        public void Remove(ISchedulePlugin aIPlugin)
        {
            InnerList.Remove(aIPlugin);
        }
        
        public ISchedulePlugin Find(string pluginName)
        {
            foreach (ISchedulePlugin plugin in this)
            {
                if (plugin.Identifier == pluginName)
                {
                    return plugin;
                }
            }
            return null;
        }
        
        public bool Contains(string MenuText)
        {
            return (Find(MenuText) != null);
        }
        
        public ISchedulePlugin this[int index] 
        {
            get { return (ISchedulePlugin)base.InnerList[index]; }
        }
    }
}