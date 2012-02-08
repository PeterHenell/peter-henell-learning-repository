using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scheduler.PluginInterfaces;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Linq;

namespace Scheduler._XML_Posten_Plugin
{
    public class PostenPlugin : BasePlugin
    {
        public PostenPlugin()
        {
            _identifier = "_XML_Posten_Plugin";
        }

        public override void Run(string connectionString, DateTime date, string startArgument)
        {
            try
            {
                ReportProgress("Starting making xml file for posten", 0);
                CreateXMLFile();
                ReportCompleted("Done with xml file");
            }
            catch (Exception e)
            {
                ReportError("Error while creating file: " + e.ToString(), Severity.Error);
            }
        }

        void CreateXMLFile()
        {
            using (SqlConnection conn = new SqlConnection(@"server=localhost;
                                        Integrated Security=true;
                                       Trusted_Connection=yes;
                                       database=_se;
                                       connection timeout=30"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(@"
                            select
	                            PersonName,
	                            Company,
	                            Address,
	                            PostalCode,
	                            City,
	                            Country,
	                            CountryCode,
	                            PhoneNumber,
	                            DoorCode
                            from orders
                            ", conn))
                {
                    DateTime date = DateTime.Now;// "20080910";

                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    ad.SelectCommand.Parameters.AddWithValue("date", date.ToString("yyyyMMdd"));
                    DataSet ds = new DataSet();
                    ad.Fill(ds);

                    string longDate = UppercaseWords(date.ToString("MMM, dd MMM yyyy") + " 10:00:00 +0100");

                    var m = new XDocument(
                       new XDeclaration("1.0", "iso-8859-1", "yes"),
                       new XProcessingInstruction("POSTNET", "SND=\"11111111111\" SNDKVAL=\"30\" REC=\"00011111111TEST\" RECKVAL=\"30\" MSGTYPE=\"INSTRXML\""),
                       new XElement("instruction",
                           new XElement("header",
                               new XElement("version", "1.0"),
                               new XElement("sender",
                                   new XAttribute("id", "111111111"),
                                   new XAttribute("codelist", "30")
                               ),
                               new XElement("receiver",
                                   new XAttribute("id", "111111111111"),
                                   new XAttribute("codelist", "30")
                               ),
                               new XElement("interchangetime", longDate),
                               new XElement("interchangeid", "1000000096"),
                               new XElement("applicationid", " Manager")
                           ),

                           from DataRow f in ds.Tables[0].Rows
                           select new XElement("consignment",
                               new XAttribute("functioncode", "9"),
                               new XElement("date", date.ToString("yyyMMdd")),
                               new XElement("service",
                                   new XAttribute("code", f["ServiceCode"])
                               ),
                               new XElement("consignor",
                                   new XAttribute("id", "111111111111"),
                                   new XAttribute("name", " AB"),
                                   new XElement("address",
                                       new XElement("street", "Timmervägen 8"),
                                       new XElement("postcode", "00000"),
                                       new XElement("city", "Katrineholm"),
                                       new XElement("countrycode", "SE")
                                   ),
                                   new XElement("contact",
                                       new XElement("communication", "0800000",
                                           new XAttribute("type", "TE")
                                        )
                                    )
                                ),
                                new XElement("consignee",
                                    new XAttribute("name", f["PersonName"]),
                                    new XElement("address",
                                        new XElement("street", f["Address"]),
                                        new XElement("postcode", f["PostalCode"]),
                                        new XElement("city", f["City"]),
                                        new XElement("countrycode", f["CountryCode"])
                                    ),
                                    new XElement("contact",
                                       new XElement("communication", f["PhoneNumber"],
                                           new XAttribute("type", "TE")
                                        )
                                    )
                                ),
                                new XElement("parcel",
                                    new XAttribute("id", f["ParcelID"]),
                                    new XElement("weight", "1.00")
                                )
                            )

                       ));

                    //Console.WriteLine(m.ToString());
                    m.Save("c:\\temp\\test.xml");
                }
            }
        }

        /// <summary>
        /// Uppercases the first letter in each word of the string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static string UppercaseWords(string value)
        {
            // Strings cannot be modified, creating an array of chars that CAN be modified.
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }
        
    }
}





