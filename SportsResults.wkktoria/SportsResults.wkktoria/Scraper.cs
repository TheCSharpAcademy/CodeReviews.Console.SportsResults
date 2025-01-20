using HtmlAgilityPack;
using SportsResults.wkktoria.Models;

namespace SportsResults.wkktoria;

public static class Scraper
{
    private const string Url = "https://www.basketball-reference.com/boxscores/";

    public static List<Game> GetGamesData()
    {
        var web = new HtmlWeb();
        var doc = web.Load(Url);

        var gameSummariesNodes = doc.DocumentNode.SelectNodes("//*[@class='game_summary expanded nohover ']");

        if (gameSummariesNodes == null) return new List<Game>();

        var gamesList = (from gameSummaryNode in gameSummariesNodes
            let winner = gameSummaryNode.SelectSingleNode(".//tr[@class='winner']/td[1]").InnerText
            let loser = gameSummaryNode.SelectSingleNode(".//tr[@class='loser']/td[1]").InnerText
            let winnerScoreString = gameSummaryNode.SelectSingleNode(".//tr[@class='winner']/td[2]").InnerText
            let loserScoreString = gameSummaryNode.SelectSingleNode(".//tr[@class='loser']/td[2]").InnerText
            select new Game
            {
                Winner = winner, Loser = loser,
                WinnerScore = int.Parse(winnerScoreString), LoserScore = int.Parse(loserScoreString)
            }).ToList();

        return gamesList;
    }
}