using HtmlAgilityPack;
using SportsResultsNotifier.Models;

namespace SportsResultsNotifier.Services;

public class WebScraperService
{
    internal List<GameResult> GetGameResults(string url)
    {
        HtmlWeb web = new HtmlWeb();
        HtmlDocument doc = web.Load(url);

        var allNbaGames = GetGameNodes(doc);
        if (allNbaGames.Count == 0)
            return new List<GameResult>();

        return ExtractGameResults(allNbaGames);
    }

    private string? GetNodeInnerText(HtmlNode node, string xpath)
        => node.SelectSingleNode(xpath)?.InnerText.Trim();


    private HtmlNodeCollection GetGameNodes(HtmlDocument doc)
        => doc.DocumentNode.SelectNodes("//*[@id='content']/div[3]/div");

    private List<GameResult> ExtractGameResults(HtmlNodeCollection games)
    {
        var results = new List<GameResult>();
        foreach (var gameNode in games)
        {
            // Extract game details
            var winner = GetNodeInnerText(gameNode, ".//tr[@class='winner']//a[@href]");
            var winnerScore = GetNodeInnerText(gameNode, ".//tr[@class='winner']//td[@class='right']");
            var loser = GetNodeInnerText(gameNode, ".//tr[@class='loser']//a[@href]");
            var loserScore = GetNodeInnerText(gameNode, ".//tr[@class='loser']//td[@class='right']");

            // Only add valid results
            if (!string.IsNullOrEmpty(winner) && !string.IsNullOrEmpty(winnerScore) &&
                !string.IsNullOrEmpty(loser) && !string.IsNullOrEmpty(loserScore))
            {
                results.Add(new GameResult
                {
                    Winner = winner,
                    WinnerScore = winnerScore,
                    Loser = loser,
                    LoserScore = loserScore
                });
            }
        }

        return results;
    }
}