using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Finance_Beta
{
    /// <summary>
    /// Summary description for companySearch
    /// </summary>
    public class companySearch1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            /*
            List<string> customers = new List<string>();
            customers.Add("Anna");
            customers.Add("Hessu");
            customers.Add("Elsa");
            customers.Add("Mikki");
            customers.Add("Aku");
            context.Response.Write(new JavaScriptSerializer().Serialize(customers));
            */



            string term = context.Request["term"] ?? "";
            List<object> company = new List<object>();
            object lookup = null;

            string url = "http://dev.markitondemand.com/MODApis/Api/v2/Lookup/json?input=" + term + "&callback=";

            try
            {
                using (WebClient wc = new WebClient())
                {
                    dynamic json = JValue.Parse(wc.DownloadString(url));

                    foreach (dynamic node in json)
                    {
                        lookup = new
                        {
                            // Set
                            Symbol = node.Symbol,
                            Name = node.Name,
                            Exchange = node.Exchange

                        };

                        company.Add(lookup.GetType().GetProperty("Name").GetValue(lookup).ToString() + " (" + lookup.GetType().GetProperty("Exchange").GetValue(lookup).ToString() + ") " + "/ " + lookup.GetType().GetProperty("Symbol").GetValue(lookup).ToString());
                    }
                    
                }
            }
            catch (Exception)
            {
                lookup = null;
            }

            context.Response.Write(new JavaScriptSerializer().Serialize(company));

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}