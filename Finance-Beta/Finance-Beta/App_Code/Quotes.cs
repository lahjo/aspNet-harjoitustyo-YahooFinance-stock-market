using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;


namespace Finance_Beta
{
    public class Quotes : YahooApi
    {
        private GoogleApi googleFinanceApi = new GoogleApi();
        public string symbol { get; private set; }
        public string name { get; private set; }
        public double lastTradePrice { get; private set; }
        public double daysLow { get; private set; }
        public double daysHigh { get; private set; }
        public string marketCapitalization { get; private set; }
        public string currency { get; private set; }
        public string change_PercentChange { get; private set; }
        public string poweredBy { get; private set; }

        public List<object> QuotesHistoricData { get; private set; }

        public Quotes(string sy, int months)
        {
            
            try
            {
                object quoteData = fetchQuotesDataYQL(sy);

                // Set company name
                name = quoteData.GetType().GetProperty("name").GetValue(quoteData).ToString();
                // Set stock currency
                currency = quoteData.GetType().GetProperty("currency").GetValue(quoteData).ToString();
                // Set company stock price
                lastTradePrice = Double.Parse(quoteData.GetType().GetProperty("lastTradePriceOnly").GetValue(quoteData).ToString(), CultureInfo.InvariantCulture);
                // Set company day lowest stock 
                daysLow = Double.Parse(quoteData.GetType().GetProperty("daysLow").GetValue(quoteData).ToString(), CultureInfo.InvariantCulture);
                // Set company day highest stock price
                //daysHigh = Double.Parse(quoteData.GetType().GetProperty("daysHigh").GetValue(quoteData).ToString(), CultureInfo.InvariantCulture);
                daysHigh = Double.Parse(quoteData.GetType().GetProperty("currency").GetValue(quoteData).ToString(), CultureInfo.InvariantCulture);
                // Set company market Capitalization size
                marketCapitalization = quoteData.GetType().GetProperty("marketCapitalization").GetValue(quoteData).ToString();
                // Set stock change and Percentage Change
                change_PercentChange = quoteData.GetType().GetProperty("change_PercentChange").GetValue(quoteData).ToString();
                // Set powered by
                poweredBy = "Yahoo! Inc.";


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

                try
                {
                    object quoteData = googleFinanceApi.fetchQuotesData(sy);

                    // Set company name
                    name = quoteData.GetType().GetProperty("name").GetValue(quoteData).ToString();
                    // Set company stock price
                    lastTradePrice = Double.Parse(quoteData.GetType().GetProperty("lastTradePriceOnly").GetValue(quoteData).ToString(), CultureInfo.InvariantCulture);
                    // Set company day lowest stock 
                    daysLow = Double.Parse(quoteData.GetType().GetProperty("daysLow").GetValue(quoteData).ToString(), CultureInfo.InvariantCulture);
                    // Set company day highest stock price
                    daysHigh = Double.Parse(quoteData.GetType().GetProperty("daysHigh").GetValue(quoteData).ToString(), CultureInfo.InvariantCulture);
                    // Set company market Capitalization size
                    marketCapitalization = quoteData.GetType().GetProperty("marketCapitalization").GetValue(quoteData).ToString();
                    // Set stock change and Percentage Change
                    change_PercentChange = quoteData.GetType().GetProperty("change_PercentChange").GetValue(quoteData).ToString();
                    // Set powered by
                    poweredBy = "Google Inc.";
                }
                catch (Exception ex2)
                {

                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine("Message :" + ex2.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex2.StackTrace +
                           "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                        writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                    }

                }
            }

            try
            {
                // Get quotes historic prices
                QuotesHistoricData = fetchQuotesHistoricDataYQL(sy, months);
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

                // Get quotes historic prices
                QuotesHistoricData = googleFinanceApi.fetchQuotesHistoricData(sy, months);
                System.Diagnostics.Debug.WriteLine(ex);
            }

            
        }
    }
}