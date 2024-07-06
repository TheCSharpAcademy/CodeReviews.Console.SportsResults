using HtmlAgilityPack;

namespace SportsResultsNotifier;

public class WebScraper
{
    public static List<SportsData> Scraper(string url)
    {
        List<SportsData> games = new();

        HtmlWeb web = new HtmlWeb();

        var document = web.Load(url);

        var nodes = document.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div");

        if (nodes.Count > 0)
        {
            foreach (var node in nodes)
            {
                var game = new SportsData
                {
                    SportsTeam1 = node.SelectSingleNode(".//table[1]/tbody/tr[1]/td[1]/a").InnerText,
                    SportsTeam2 = node.SelectSingleNode(".//table[1]/tbody/tr[2]/td[1]/a").InnerText,
                    Team1Score = int.Parse(node.SelectSingleNode(".//table[1]/tbody/tr[1]/td[2]").InnerText),
                    Team2Score = int.Parse(node.SelectSingleNode(".//table[1]/tbody/tr[2]/td[2]").InnerText)
                };
                games.Add(game);
            }

            return games;
        }
        else
            return games;
    }

    public static string ScrapeDate(string url)
    {
        HtmlWeb web = new HtmlWeb();
        var document = web.Load(url);
        var date = document.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[1]/span").InnerText;
        return date;
    }

    public static string ScrapeGameAmmount(string url)
    {
        HtmlWeb web = new HtmlWeb();
        var document = web.Load(url);
        var gameAmount = document.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[2]/h2").First().InnerText;
        return gameAmount;
    }
}