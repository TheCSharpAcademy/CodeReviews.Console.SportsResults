using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SportsResults.Speedierone
{
    public class Scraper
    {
        public List<Results> GetResults(string url)
        {
            List<Results> results = new List<Results>();

            HtmlWeb web = new HtmlWeb();
            var doc = web.Load(url);

            var table = doc.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div[1]/table[1]/tbody/tr");

            if (table != null)
            {
                foreach (var node in table)
                {
                    var result = new Results
                    {
                        Team1 = node.SelectSingleNode("td[1]").InnerText,
                        Team2= node.SelectSingleNode("td[1]").InnerText,
                        Score1= int.Parse(node.SelectSingleNode("td[2]").InnerText),
                        //LoserScore = node.SelectSingleNode("td[6]").InnerText,
                    };
                        results.Add(result);                 
                }
            }
            return results;
        }
    }
}
//*[@id="content"]/div[3]/div[1]/table[1]/tbody/tr[2]