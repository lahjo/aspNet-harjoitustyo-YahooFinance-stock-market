using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;

namespace Finance_Beta
{
    public class WatsonAlchemyLanguage
    {
        public string sentimentAnalysis(string newsUrl)
        {
            string sentiment = "";

            try
            {
                string url = "http://access.alchemyapi.com/calls/url/URLGetTextSentiment?apikey=" + WebConfigurationManager.AppSettings["BluemixApiKey"] + "&url=" + newsUrl + "&showSourceText=1&outputMode=json";
                
                using (WebClient wc = new WebClient())
                {
                    dynamic json = JValue.Parse(wc.DownloadString(url));
                    // Set sentiment result
                    sentiment = json.docSentiment.type;
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

                sentiment = "";
            }

            return sentiment;
        }

        public string keywordsAnalysis(string newsUrl)
        {

            object keyword = null;
            string keywordList = "";

            try
            {
                string url = "http://access.alchemyapi.com/calls/url/URLGetRankedKeywords?apikey=" + WebConfigurationManager.AppSettings["BluemixApiKey"] + "&url=" + newsUrl + "&showSourceText=1&outputMode=json";

                using (WebClient wc = new WebClient())
                {
                    dynamic json = JValue.Parse(wc.DownloadString(url));

                    foreach (dynamic node in json.keywords)
                    {
                        keyword = new
                        {
                            // Set keyword
                            keywords = node.text,
                            // Set keyword relevance
                            keywordsRelevance = node.relevance

                        };

                        // Create keyword list
                        keywordList += keyword.GetType().GetProperty("keywords").GetValue(keyword).ToString();
                        keywordList += "<br />";
                        keywordList += "Relevance: " + keyword.GetType().GetProperty("keywordsRelevance").GetValue(keyword).ToString();
                        keywordList += "<br />";
                        keywordList += "<br />";
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

                keywordList = "";
            }
  

            return keywordList;
        }
    }
}