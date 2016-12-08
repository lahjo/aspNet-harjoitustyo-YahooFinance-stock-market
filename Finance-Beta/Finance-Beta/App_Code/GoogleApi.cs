using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Finance_Beta
{
    public class GoogleApi
    {
        public object fetchQuotesData(string symbol)
        {
            // Create new object
            object quote = new object();

            string lastTradePriceOnly = "";    
            string daysLow = "";
            string daysHigh = "";
            string marketCapitalization = "";
            string name = "";
            string change = "";
            string percentChange = "";

            // url
            string url = "http://www.google.com/finance/info?infotype=infoquoteall&q=" + symbol;

            try
            {

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                StreamReader reader = new StreamReader(resp.GetResponseStream());

                // Skip first line
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    string[] values = line.Split(':');

                    char[] extraChars = { ',', '"', '\"', ' '};
                    string actionString = values[0].Trim(extraChars);

                    if (actionString == "name") {
                        // Set name
                        name = values[1].Trim('\"', ' ');
                    }
                    else if (actionString == "l")
                    {
                        // Set stock price
                        lastTradePriceOnly = values[1].Trim('\"', ' ');
                    }
                    else if (actionString == "c")
                    {
                        // Set stock change
                        change = values[1].Trim('\"', ' ');
                    }
                    else if (actionString == "cp")
                    {
                        // Set stock precentage change
                        percentChange = values[1].Trim('\"', ' ');
                    }
                    else if (actionString == "hi")
                    {
                        // Set stock day highest price
                        daysHigh = values[1].Trim('\"', ' ');
                    }
                    else if (actionString == "lo")
                    {
                        // Set stock day lowest price
                        daysLow = values[1].Trim('\"', ' ');
                    }
                    else if (actionString == "mc")
                    {
                        // Set company market capitalization
                        marketCapitalization = values[1].Trim('\"', ' ');
                    }
                }
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

            quote = new
            {
                // Set company stock price
                lastTradePriceOnly = lastTradePriceOnly,
                // Set company day lowest stock price
                daysLow = daysLow,
                // Set company day highest stock price
                daysHigh = daysHigh,
                // Set company market Capitalization size
                marketCapitalization = marketCapitalization,
                // Set company name
                name = name,
                // Set stock change and Percentage Change
                change_PercentChange = change + " - " + percentChange + "%",
            };

            return quote;
        }

        public List<object> fetchQuotesHistoricData(string symbol, int month)
        {
            List<object> historicaldata = new List<object>();
            object quote = null;

            string url = "http://www.google.com/finance/historical?q=" + symbol + "&startdate=" + DateTime.Today.AddMonths(-(month)).ToString("MMM dd, yyyy") + "&enddate=" + DateTime.Today.ToString("MMM dd, yyyy") + "&output=csv";

            try
            {

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                StreamReader reader = new StreamReader(resp.GetResponseStream());

                // Skip first line
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    string[] values = line.Split(',');

                    quote = new
                    {
                        // Set date
                        date = values[0],
                        // Set closing price
                        ClosingPrice = values[4]
                    };

                    // Add object to list
                    historicaldata.Add(quote);
                }
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

            // Reverse list order
            historicaldata.Reverse();

            return historicaldata;
        }
    }
}