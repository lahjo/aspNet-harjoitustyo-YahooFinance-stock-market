using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;

namespace Finance_Beta
{
    public class DBpedia
    {
        public object companyInfo(string name)
        {
            // Create new object
            object company = new object();

            XmlDocument xmlData = new XmlDocument();
            // Load xml company info from Dbpedia api
            xmlData.Load("http://lookup.dbpedia.org/api/search.asmx/KeywordSearch?QueryClass=Company&QueryString=" + name);

            XmlNodeList nodelist = xmlData.GetElementsByTagName("Description");
            XmlNode node = nodelist[0];

                try
                {
                    company = new
                    {
                        // Set description
                        description = node.InnerText
                    };
                }
                catch (Exception ex)
                {

                string filePath = HttpContext.Current.Server.MapPath("~/App_Data/log.txt");

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                       "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                }

                company = new
                {
                    // Set description
                    description = ""
                };
            }

            return company;

        }
    }
}