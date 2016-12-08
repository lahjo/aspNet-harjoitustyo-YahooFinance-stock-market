using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Finance_Beta
{
    public class News : YahooApi
    {
        public List<object> industryNews { get; private set; }

        public News(string symbol)
        {
            try
            {
                // Get news
                industryNews = fetchNewsRss(symbol);
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
            }
        }
    }
}