using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Finance_Beta
{
    public partial class index : System.Web.UI.Page
    {
        private Quotes quote;
        private News companyNews;
        private DBpedia companyInfoPedia = new DBpedia();
        private financialsMorningstar financialsData;
        protected void Page_Load(object sender, EventArgs e) {
        }

        public string watson(string url)
        {
            WatsonAlchemyLanguage analyze = new WatsonAlchemyLanguage();


            return analyze.sentimentAnalysis(url);
        }

        public string watsonKeywords(string url)
        {
            WatsonAlchemyLanguage analyze = new WatsonAlchemyLanguage();

            return analyze.keywordsAnalysis(url);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string[] words = txtSearch.Text.Split('/');
            string seachWord;

            if(words.Length == 2)
            {
                seachWord = words[1];
            }else
            {
                seachWord = words[0];
            }

            quote = new Quotes(seachWord, 1);
            // Set name to <name> label
            name.Text = quote.name;
            // Set market capitalization to <mkt capitilization> label
            marketCapitalization.Text = quote.marketCapitalization;
            // Set stock price value to <stockprice> label
            stockPrice.Text = quote.lastTradePrice.ToString();
            // Set precentage change to <change_PercentChange> label
            change_PercentChange.Text = quote.change_PercentChange;
            // Set day highest price to <daysHigh> label
            daysHigh.Text = quote.daysHigh.ToString();
            // Set day lowest price to <daysLow> label
            daysLow.Text = quote.daysLow.ToString();
            // Set currency to <currency> label
            currency.Text = quote.currency;

            poweredby.Text = quote.poweredBy;

            object description = companyInfoPedia.companyInfo(quote.name);
            name.ToolTip = description.GetType().GetProperty("description").GetValue(description).ToString();

            try
            {
                double maxPrice = quote.QuotesHistoricData.Max(t => double.Parse(t.GetType().GetProperty("ClosingPrice").GetValue(t).ToString()));
                double minPrice = quote.QuotesHistoricData.Min(t => double.Parse(t.GetType().GetProperty("ClosingPrice").GetValue(t).ToString()));

                Chart1.ChartAreas[0].AxisY.Maximum = maxPrice + 0.1;
                Chart1.ChartAreas[0].AxisY.Minimum = minPrice - 0.1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
                        
            foreach (object nodes in quote.QuotesHistoricData)
            {
                Chart1.Series[0].Points.AddXY(nodes.GetType().GetProperty("date").GetValue(nodes).ToString(), nodes.GetType().GetProperty("ClosingPrice").GetValue(nodes).ToString());
            }
            

            // Get company news
            companyNews = new News(seachWord);
            newsList.DataSource = companyNews.industryNews;
            newsList.DataBind();

            financialsData = new financialsMorningstar();
            List<object> data = financialsData.financialsInfo(seachWord);
            GridView1.DataSource = data;
            GridView1.DataBind();

            // Create a new legend called "Legend2".
            Chart2.Legends.Add(new Legend("Revenue"));

            // Create a new legend called "Legend2".
            Chart2.Legends.Add(new Legend("Earnings"));

            try
            {

            foreach (object nodes in data)
            {
                Chart2.Series[0].Points.AddXY(nodes.GetType().GetProperty("year").GetValue(nodes).ToString(), nodes.GetType().GetProperty("totalRevenue").GetValue(nodes).ToString());
            }

            Chart2.Series[0].SmartLabelStyle.Enabled = true;
            // Assign the legend to Series1.
            Chart2.Series[0].Legend = "Revenue";
            Chart2.Series[0].IsVisibleInLegend = true;
            Chart2.Series[0].LegendText = "Revenue";

            foreach (object nodes in data)
            {
                Chart2.Series[1].Points.AddXY(nodes.GetType().GetProperty("year").GetValue(nodes).ToString(), nodes.GetType().GetProperty("NetIncome").GetValue(nodes).ToString());
            }

            // Assign the legend to Series1.
            Chart2.Series[1].Legend = "Earnings";
            Chart2.Series[1].IsVisibleInLegend = true;
            Chart2.Series[1].LegendText = "Earnings";

            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        protected void historicTimeLine_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string yourValue = btn.CommandArgument;

            string[] words = txtSearch.Text.Split('/');
            string seachWord;

            if (words.Length == 2)
            {
                seachWord = words[1];
            }
            else
            {
                seachWord = words[0];
            }

            quote = new Quotes(seachWord, int.Parse(yourValue));

            try
            {
                double maxPrice = quote.QuotesHistoricData.Max(t => double.Parse(t.GetType().GetProperty("ClosingPrice").GetValue(t).ToString()));
                double minPrice = quote.QuotesHistoricData.Min(t => double.Parse(t.GetType().GetProperty("ClosingPrice").GetValue(t).ToString()));

                Chart1.ChartAreas[0].AxisY.Maximum = maxPrice + 0.1;
                Chart1.ChartAreas[0].AxisY.Minimum = minPrice - 0.1;

                foreach (object nodes in quote.QuotesHistoricData)
                {
                    Chart1.Series[0].Points.AddXY(nodes.GetType().GetProperty("date").GetValue(nodes).ToString(), nodes.GetType().GetProperty("ClosingPrice").GetValue(nodes).ToString());
                }
            }
            catch (Exception)
            {

                System.Diagnostics.Debug.WriteLine(yourValue);
            }
        }
    }
}

