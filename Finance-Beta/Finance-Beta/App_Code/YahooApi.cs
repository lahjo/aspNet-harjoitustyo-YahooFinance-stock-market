using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Xml;

namespace Finance_Beta
{
    public class YahooApi
    {

        protected object fetchQuotesDataYQL(string symbol)
        {
            XmlDocument doc1 = new XmlDocument();
            // Load xml stock info from yahoo api using yahoo YQL rest api
            doc1.Load("http://query.yahooapis.com/v1/public/yql?q=select%20%2a%20from%20yahoo.finance.quotes%20where%20symbol%20in%20%28%22" + symbol + "%22%29&env=store://datatables.org/alltableswithkeys");

            XmlNodeList nodelist = doc1.SelectNodes("/query/results/quote");

            // Create empty object
            object quote = null;

            foreach (XmlNode node in nodelist)
            {
                try
                {
                    quote = new
                    {
                        // Set company stock price
                        lastTradePriceOnly = node.SelectSingleNode("LastTradePriceOnly").InnerText,
                        // Set company day lowest stock price
                        daysLow = node.SelectSingleNode("DaysLow").InnerText,
                        // Set company day highest stock price
                        daysHigh = node.SelectSingleNode("DaysHigh").InnerText,
                        // Set company market Capitalization size
                        marketCapitalization = node.SelectSingleNode("MarketCapitalization").InnerText,
                        // Set company name
                        name = node.SelectSingleNode("Name").InnerText,
                        // Set stock change and Percentage Change
                        change_PercentChange = node.SelectSingleNode("Change_PercentChange").InnerText,
                        // Set stock currency
                        currency = node.SelectSingleNode("Currency").InnerText
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

                    throw;
                }
                
            }


            return quote;

        }

        protected List<object> fetchQuotesHistoricDataYQL(string symbol, int month)
        {
            // Create empty object
            object quote = null;
            // Create empty object list
            List<object> historicaldata = new List<object>();


            XmlDocument xmlData = new XmlDocument();
            // Load xml stock info from yahoo api using yahoo YQL rest api
            xmlData.Load("http://query.yahooapis.com/v1/public/yql?q=select%20%2a%20from%20yahoo.finance.historicaldata%20where%20symbol%20in%20%28%27" + symbol + "%27%29%20and%20startDate%20=%20%27" + DateTime.Today.AddMonths(-(month)).ToString("yyyy-MM-dd") + "%27%20and%20endDate%20=%20%27" + DateTime.Today.ToString("yyyy-MM-dd") + "%27&diagnostics=true&env=store://datatables.org/alltableswithkeys");

            XmlNodeList nodelist = xmlData.SelectNodes("/query/results/quote");

            

            foreach (XmlNode node in nodelist)
            {
                try
                {
                    quote = new
                    {
                        // Set date
                        date = node.SelectSingleNode("Date").InnerText,
                        // Set stock price
                        ClosingPrice = node.SelectSingleNode("Close").InnerText,
                    };

                    // Add object to object list
                    historicaldata.Add(quote);
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

                    throw;
                }

            }

            // Reverse list
            historicaldata.Reverse();

            return historicaldata;

        }

        protected List<object> fetchNewsRss(string symbol) {
            // Create empty object
            object news = null;
            // Create empty object list
            List<object> yahooNews = new List<object>();

            string url = " http://finance.yahoo.com/rss/industry?s=" + symbol;

            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();
            foreach (SyndicationItem item in feed.Items)
            {
                try
                {
                    news = new
                    {
                        title = item.Title.Text,
                        url = item.Links[0].Uri.AbsoluteUri,
                        publishDate = item.PublishDate.LocalDateTime
                    };

                    // Add object to object list
                    yahooNews.Add(news);
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

                    throw;
                }
            }

            return yahooNews;
        }
    }
}