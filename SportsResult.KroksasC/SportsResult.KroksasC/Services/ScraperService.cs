using HtmlAgilityPack;
using SportsResult.KroksasC.Models;

namespace SportsResult.KroksasC.Services
{
    internal class ScraperService
    {
        private static HttpClient client = new();

        public static async Task<List<SportResult>> GetSportResults(string url)
        {
            List<SportResult> sportResults = new List<SportResult>();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(url);

            var games = document.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div[position()>0]");

            foreach (var node in games)
            {
                sportResults.Add(new SportResult
                {
                    Winner = node.SelectSingleNode("table[1]/tbody/tr[contains(@class, 'winner')]/td[1]/a").InnerText,
                    Loser = node.SelectSingleNode("table[1]/tbody/tr[contains(@class, 'loser')]/td[1]/a").InnerText,
                    LoserPoints = node.SelectSingleNode("table[1]/tbody/tr[contains(@class, 'loser')]/td[2]").InnerText,
                    WinnerPoints = node.SelectSingleNode("table[1]/tbody/tr[contains(@class, 'winner')]/td[2]").InnerText,
                });
                
            }
            return sportResults;   
        }
    }
}
