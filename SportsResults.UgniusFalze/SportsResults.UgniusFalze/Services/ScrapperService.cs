using HtmlAgilityPack;
using SportsResults.UgniusFalze.Models;

namespace SportsResults.UgniusFalze.Services;

public class ScrapperService
{
    private readonly string GamesUrl = @"https://www.basketball-reference.com/boxscores/";
    public List<Game>? GetGames()
    {
        var web = new HtmlWeb();
        var htmlDoc = web.Load(GamesUrl);
        var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@class = \"teams\"]");
        if (nodes == null)
        {
            Console.WriteLine("No games were found for today, try again later.");
            return null;
        }
        var games = new List<Game>();
        foreach (var node in nodes)
        {
            var loser = node.SelectSingleNode(".//*[@class = \"loser\"]//td/a[1]").InnerText;
            var winner = node.SelectSingleNode(".//*[@class = \"winner\"]//td/a[1]").InnerText;
            var winnerScore = node.SelectSingleNode(".//*[@class = \"winner\"]//td[2]").InnerText;
            var loserScore = node.SelectSingleNode(".//*[@class = \"loser\"]//td[2]").InnerText;
            games.Add(new Game(int.Parse(winnerScore), int.Parse(loserScore), winner, loser));
        }

        return games;
    }
}