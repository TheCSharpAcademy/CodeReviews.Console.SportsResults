using HtmlAgilityPack;
using SportsResults.TwilightSaw.Models;

namespace SportsResults.TwilightSaw.Controllers;

public class ScrapperController
{
    private readonly HtmlDocument? _htmlDoc = new HtmlWeb().Load("https://www.basketball-reference.com/boxscores/");
    public List<Game> GetGames()
    {
        var winners = _htmlDoc.DocumentNode.SelectNodes("//tr[@class = 'winner']/td/a").Where(p => p.InnerText != "Final").ToList();
        var losers = _htmlDoc.DocumentNode.SelectNodes("//tr[@class = 'loser']/td/a").Where(p => p.InnerText != "Final").ToList();

        var winnersScore = _htmlDoc.DocumentNode.SelectNodes("//tr[@class = 'winner']/td[@class = 'right'][1]");
        var losersScore = _htmlDoc.DocumentNode.SelectNodes("//tr[@class = 'loser']/td[@class = 'right'][1]");

        var gamesList = new List<Game>();
        for (var index = 0; index < winners.Count; index++)
        {
            var clearedWinnerScore = winnersScore.ToList()[index].InnerText
                .Replace("&nbsp;\n\t\t\t", " ")
                .Replace("OT", " ");
            var clearedLoser = losersScore.ToList()[index].InnerText
                .Replace("&nbsp;\n\t\t\t", " ")
                .Replace("OT", " "); ;
            if (clearedWinnerScore.Equals(" ")) winnersScore.Remove(winnersScore[index]);
            if (clearedLoser.Equals(" ")) losersScore.Remove(losersScore[index]);
            int.TryParse(winnersScore[index].InnerText, out var winnerScore);
            int.TryParse(losersScore[index].InnerText, out var loserScore);

           gamesList.Add(new Game(losers[index].InnerText,winners[index].InnerText,
                winnerScore, loserScore));
        }

        return gamesList;
    }

    public List<GameStatistic> GetGameStatistics()
    {
        var gameStatList = new List<GameStatistic>();
        var gameSummaries = _htmlDoc.DocumentNode.SelectNodes("//div[@class='game_summary expanded nohover ']");

        foreach (var game in gameSummaries)
        {
            var team1 = game.SelectSingleNode(".//tr[1]/td[1]/a")?.InnerText.Trim();
            var team2 = game.SelectSingleNode(".//tr[2]/td[1]/a")?.InnerText.Trim();

            var team1Scores = new List<int>();
            var team2Scores = new List<int>();

            var team1Quarters = game.SelectNodes(".//tr[1]/td[@class='center']");
            var team2Quarters = game.SelectNodes(".//tr[2]/td[@class='center']");

            foreach (var quarter in team1Quarters) if (int.TryParse(quarter.InnerText.Trim(), out var score)) team1Scores.Add(score);
            foreach (var quarter in team2Quarters) if (int.TryParse(quarter.InnerText.Trim(), out var score)) team2Scores.Add(score);

            var ptsPlayer = game.SelectSingleNode(".//table[@class = 'stats']//tr[1]/td[1]").InnerText.Trim() + " "
                + game.SelectSingleNode(".//table[@class = 'stats']//tr[1]/td[2]").InnerText.Trim();
            var trbPlayer = game.SelectSingleNode(".//table[@class = 'stats']//tr[2]/td[1]").InnerText.Trim() + " "
                + game.SelectSingleNode(".//table[@class = 'stats']//tr[2]/td[2]").InnerText.Trim();

            var ptsScores = game.SelectSingleNode(".//table[@class = 'stats']//tr[1]/td[@class = 'right']");
            var trbScores = game.SelectSingleNode(".//table[@class = 'stats']//tr[2]/td[@class = 'right']");

            int.TryParse(ptsScores.InnerText.Trim(), out var ptsScore);
            int.TryParse(trbScores.InnerText.Trim(), out var trbScore);
            
            gameStatList.Add(new GameStatistic(team1, team2, team1Scores, team2Scores, ptsPlayer, trbPlayer, ptsScore, trbScore));
        }
        return gameStatList;
    }
}