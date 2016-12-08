using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.IO;

namespace Finance_Beta
{
    public class financialsMorningstar
    {
        public List<object> financialsInfo(string symbol)
        {
            List<object> financials = new List<object>();
            // Income Statement
            string url = "http://financials.morningstar.com/ajax/ReportProcess4CSV.html?t=" + symbol + "&reportType=is&period=12&dataType=A&order=asc&columnYear=5&number=3";

            try
            {

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader reader = new StreamReader(resp.GetResponseStream());

            // Skip first line
            reader.ReadLine();

            var yearLine = reader.ReadLine();
            string[] years = yearLine.Split(',');

            for(int i = 1; i < years.Length; i++)
            {
                if (years[i] != "")
                {
                    object yearIncomeStatement = new
                    {
                        year = years[i],
                        totalRevenue = "",
                        CostOfRevenue = "",
                        GrossProfit = "",
                        NetIncome = "",
                    };

                    financials.Add(yearIncomeStatement);
                }
            }

            while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    string[] values = line.Split(',');

                switch (values[0])
                {
                    case "Revenue":
                        for (int i = 0, round = 1; round < values.Length; i++, round++)
                        {
                            if (values[round] != "")
                            {
                                object data = new
                                {
                                    // Set year
                                    year = financials[i].GetType().GetProperty("year").GetValue(financials[i]).ToString(),
                                    // Set total revenue
                                    totalRevenue = values[round],
                                    // Set cost of revenue
                                    CostOfRevenue = financials[i].GetType().GetProperty("CostOfRevenue").GetValue(financials[i]).ToString(),
                                    // Set gross profit
                                    GrossProfit = financials[i].GetType().GetProperty("GrossProfit").GetValue(financials[i]).ToString(),
                                    // Set net income
                                    NetIncome = financials[i].GetType().GetProperty("NetIncome").GetValue(financials[i]).ToString(),
                                };

                                financials.RemoveAt(i);
                                financials.Insert(i, data);
                            }
                        }
                        break;
                    case "Cost of revenue":
                        for (int i = 0, round = 1; round < values.Length; i++, round++)
                        {
                            if (values[round] != "")
                            {
                                object data = new
                                {
                                    // Set year
                                    year = financials[i].GetType().GetProperty("year").GetValue(financials[i]).ToString(),
                                    // Set total revenue
                                    totalRevenue = financials[i].GetType().GetProperty("totalRevenue").GetValue(financials[i]).ToString(),
                                    // Set cost of revenue
                                    CostOfRevenue = values[round],
                                    // Set gross profit
                                    GrossProfit = financials[i].GetType().GetProperty("GrossProfit").GetValue(financials[i]).ToString(),
                                    // Set net income
                                    NetIncome = financials[i].GetType().GetProperty("NetIncome").GetValue(financials[i]).ToString(),
                                };

                                financials.RemoveAt(i);
                                financials.Insert(i, data);
                            }
                        }
                        break;
                    case "Gross profit":
                        for (int i = 0, round = 1; round < values.Length; i++, round++)
                        {
                            if (values[round] != "")
                            {
                                object data = new
                                {
                                    // Set year
                                    year = financials[i].GetType().GetProperty("year").GetValue(financials[i]).ToString(),
                                    // Set total revenue
                                    totalRevenue = financials[i].GetType().GetProperty("totalRevenue").GetValue(financials[i]).ToString(),
                                    // Set cost of revenue
                                    CostOfRevenue = financials[i].GetType().GetProperty("CostOfRevenue").GetValue(financials[i]).ToString(),
                                    // Set gross profit
                                    GrossProfit = values[round],
                                    // Set net income
                                    NetIncome = financials[i].GetType().GetProperty("NetIncome").GetValue(financials[i]).ToString(),
                                };

                                financials.RemoveAt(i);
                                financials.Insert(i, data);
                            }
                        }
                        break;
                    case "Net income":
                        for (int i = 0, round = 1; round < values.Length; i++, round++)
                        {
                            if (values[round] != "")
                            {
                                object data = new
                                {
                                    // Set year
                                    year = financials[i].GetType().GetProperty("year").GetValue(financials[i]).ToString(),
                                    // Set total revenue
                                    totalRevenue = financials[i].GetType().GetProperty("totalRevenue").GetValue(financials[i]).ToString(),
                                    // Set cost of revenue
                                    CostOfRevenue = financials[i].GetType().GetProperty("CostOfRevenue").GetValue(financials[i]).ToString(),
                                    // Set gross profit
                                    GrossProfit = financials[i].GetType().GetProperty("GrossProfit").GetValue(financials[i]).ToString(),
                                    // Set net income
                                    NetIncome = values[round],
                                };
                                
                                // Remove old object from list
                                financials.RemoveAt(i);
                                // Inser new object to list
                                financials.Insert(i, data);
                            }
                        }
                        break;
                    default:
                        break;
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

                financials = null;
            }

            return financials;
        }     
    }
}