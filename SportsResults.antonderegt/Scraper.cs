using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using SportsResults.Models;

namespace SportsResults;

public class Scraper
{
    private readonly string UrlToScrape;
    public Scraper()
    {
        IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        UrlToScrape = configuration.GetSection("Scraper")["UrlToScrape"] ?? string.Empty;
    }

    public List<Game> GetResults()
    {
        var web = new HtmlWeb();
        var doc = web.Load(UrlToScrape);
        var scrapedGames = doc.DocumentNode.SelectNodes("//*[@class='game_summary expanded nohover ']");

        if (scrapedGames == null)
        {
            Console.WriteLine("No games found...");
            return [];
        }

        List<Game> games = [];
        foreach (var match in scrapedGames)
        {
            _ = int.TryParse(match.SelectSingleNode(".//tr[@class='winner']/td[2]").InnerText, out int winnerPoints);
            _ = int.TryParse(match.SelectSingleNode(".//tr[@class='loser']/td[2]").InnerText, out int loserPoints);

            Game game = new()
            {
                Winner = match.SelectSingleNode(".//tr[@class='winner']/td[1]").InnerText,
                WinnerPoints = winnerPoints,
                Loser = match.SelectSingleNode(".//tr[@class='loser']/td[1]").InnerText,
                LoserPoints = loserPoints
            };

            games.Add(game);
        }

        return games;
    }
}