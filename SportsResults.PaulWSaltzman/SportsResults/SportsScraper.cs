using HtmlAgilityPack;
using SportsResults.Models;


namespace SportsResults;

internal class SportsScraper
{
    internal HtmlAgilityPack.HtmlDocument LoadDocument(string url)
    {
        var uri = new Uri(url);
        List<Game> games = new List<Game>();

        HtmlWeb web = new HtmlWeb();
        var doc = web.Load(url);

        return doc;
    }
    internal List<Game> GetGames(HtmlDocument document)
    {
        var games = new List<Game>();

        var rawGames = document.DocumentNode.SelectNodes("//*[@class='game_summary expanded nohover ']");

        foreach (var rawGame in rawGames)
        {
            var game = new Game();

            game.Winner = rawGame.SelectSingleNode(".//tr[@class='winner']/td[1]").InnerText;
            game.Loser = rawGame.SelectSingleNode(".//tr[@class='loser']/td[1]").InnerText;

            string winnerScoreStr = rawGame.SelectSingleNode(".//tr[@class='winner']/td[2]").InnerText;
            int winnerScore;
            if (int.TryParse(winnerScoreStr, out winnerScore))
            {
                game.WinnerScore = winnerScore;
            }

            string loserScoreStr = rawGame.SelectSingleNode(".//tr[@class='loser']/td[2]").InnerText;
            int loserScore;
            if (int.TryParse(loserScoreStr, out loserScore))
            {
                game.LoserScore = loserScore;
            }

            games.Add(game);
        }

        return games;
    }
}