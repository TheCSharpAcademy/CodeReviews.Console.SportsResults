using HtmlAgilityPack;
using SportsResults.Models;

namespace SportsResults.Service;

internal static class WebScraper
{

    public static List<Game> Scrape()
    {
        string html = @"https://www.basketball-reference.com/boxscores/";
        HtmlWeb web = new HtmlWeb();
        HtmlDocument htmlDoc = web.Load(html);
        List<Game> games = new();

        var gameSummaries = htmlDoc.DocumentNode.SelectNodes("//div[@class='game_summary expanded nohover ']");
        if (gameSummaries != null)
        {
            foreach (var gameSummary in gameSummaries)
            {
                var winnerNode = gameSummary.SelectSingleNode(".//tr[@class='winner']");
                var loserNode = gameSummary.SelectSingleNode(".//tr[@class='loser']");

                if (winnerNode != null && loserNode != null)
                {
                    Game game = new();

                    game.Matchup = $"{winnerNode.SelectSingleNode(".//td[1]/a").InnerText} vs {loserNode.SelectSingleNode(".//td[1]/a").InnerText}";
                    game.Winner = winnerNode.SelectSingleNode(".//td[1]/a").InnerText;
                    game.WinnerScore = winnerNode.SelectSingleNode(".//td[2]").InnerText;
                    game.Loser = loserNode.SelectSingleNode(".//td[1]/a").InnerText;
                    game.LoserScore = loserNode.SelectSingleNode(".//td[2]").InnerText;

                    games.Add(game);
                }
            }
        }

        return games;
    }
}
