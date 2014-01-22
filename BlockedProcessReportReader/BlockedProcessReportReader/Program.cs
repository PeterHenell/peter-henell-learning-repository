using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;

namespace BlockedProcessReportReader
{
    class Program
    {
        static void Main(string[] args)
        {
            XDocument doc = XDocument.Load(@"c:\blocked.xml", LoadOptions.None);
            
            
            int i = 0;

            var blocked = from b in doc.Descendants("BlockedReports").Elements("blocked-process-report")
                          select new
                          {
                              InputBuff = b.Element("blocked-process").Element("process").Element("inputbuf").Value.ToLower(),
                              Spid = b.Element("blocked-process").Element("process").Attribute("spid").Value,
                              Lastbatchstarted = b.Element("blocked-process").Element("process").Attribute("lastbatchstarted").Value,
                              Lastbatchcompleted  = b.Element("blocked-process").Element("process").Attribute("lastbatchcompleted").Value,
                              Lasttranstarted = b.Element("blocked-process").Element("process").Attribute("lasttranstarted").Value,
                              Waittime = b.Element("blocked-process").Element("process").Attribute("waittime").Value
                          };
            var blocking = from b in doc.Descendants("BlockedReports").Elements("blocked-process-report")
                          select new
                          {
                              InputBuff = b.Element("blocking-process").Element("process").Element("inputbuf").Value,
                              Spid = b.Element("blocking-process").Element("process").Attribute("spid").Value,
                              Lastbatchstarted = b.Element("blocking-process").Element("process").Attribute("lastbatchstarted").Value,
                              Lastbatchcompleted = b.Element("blocking-process").Element("process").Attribute("lastbatchcompleted").Value
                          };

            var totalCount = blocked.Count();
            var blockedPlayerSession = blocked.Where(x => x.InputBuff.Contains("playersession")).Count();

            var c = blocked.GroupBy(x => x.Spid).Select(x => new { Key = x.Key, Count = x.Count() });

            Console.WriteLine(totalCount);
            Console.WriteLine(blockedPlayerSession);
            

            foreach (var bing in blocking)
            {
                Console.WriteLine("{0} {1} {2} {3}  ", bing.InputBuff, bing.Lastbatchcompleted, bing.Lastbatchstarted, bing.Spid);
            }

            Console.ReadKey();

            //foreach (var bl in blocked.Where(x => x.InputBuff.Contains("playersession") ))
            //{
            //    Console.WriteLine(bl);
            //}
        }
        
    }
}
