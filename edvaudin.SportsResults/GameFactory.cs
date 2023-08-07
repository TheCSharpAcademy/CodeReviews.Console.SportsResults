using HtmlAgilityPack;

namespace SportsResults;

public class GameFactory
{
    private static Game CreateGame(HtmlNode node)
    {
        return new Game
        {
            WinningTeam = node.SelectSingleNode(".//tr[@class='winner']").SelectSingleNode(".//a").InnerText,
            WinningScore = int.Parse(node.SelectSingleNode(".//tr[@class='winner']").SelectSingleNode(".//td[@class='right']").InnerText),
            LosingTeam = node.SelectSingleNode(".//tr[@class='loser']").SelectSingleNode(".//a").InnerText,
            LosingScore = int.Parse(node.SelectSingleNode(".//tr[@class='loser']").SelectSingleNode(".//td[@class='right']").InnerText),
            HighestPointScorer = node.SelectNodes(".//table[@class='stats']/tbody/tr")[0].SelectSingleNode(".//td[contains(a, *)]").InnerText,
            HighestPlayerPoints = int.Parse(node.SelectNodes(".//table[@class='stats']/tbody/tr")[0].SelectSingleNode(".//td[@class='right']").InnerText),
            MostTotalRebounder = node.SelectNodes(".//table[@class='stats']/tbody/tr")[1].SelectSingleNode(".//td[contains(a, *)]").InnerText,
            MostPlayerTotalRebounds = int.Parse(node.SelectNodes(".//table[@class='stats']/tbody/tr")[1].SelectSingleNode(".//td[@class='right']").InnerText),
        };
    }

    public static List<Game> CreateGamesFromHtmlNodeCollection(HtmlNodeCollection nodes)
    {
        List<Game> games = new();
        foreach (var node in nodes)
        {
            games.Add(CreateGame(node));
        }
        return games;
    }
}
