using SportsResults.frockett.Models;
using System.Text;

namespace SportsResults.frockett;

internal static class EmailComposer
{
    public static string BuildHtmlTable(List<GameResult> results)
    {
        var htmlBuilder = new StringBuilder();

        htmlBuilder.AppendLine($"<h2>Top 5 Score Updates as of {DateTime.Now.ToString("MMMM dd, yyyy")}</h2>");

        htmlBuilder.AppendLine("<table border ='1'>");
        htmlBuilder.AppendLine("<tr><th>Home Team</th><th>Score</th><th>Away Team</th><th>Score</th></tr>");

        foreach (var game in results)
        {
            var homeScore = int.Parse(game.HomeFinalScore);
            var awayScore = int.Parse(game.AwayFinalScore);

            // make the winning team's info bold, like on the website.
            string homeTeamDisplay = homeScore > awayScore ? $"<strong>{game.HomeTeam}</strong>" : game.HomeTeam;
            string awayTeamDisplay = awayScore > homeScore ? $"<strong>{game.AwayTeam}</strong>" : game.AwayTeam;
            string homeScoreDisplay = homeScore > awayScore ? $"<strong>{game.HomeFinalScore}</strong>" : game.HomeFinalScore;
            string awayScoreDisplay = awayScore > homeScore ? $"<strong>{game.AwayFinalScore}</strong>" : game.AwayFinalScore;

            htmlBuilder.AppendLine($"<tr><td>{homeTeamDisplay}</td><td>{homeScoreDisplay}</td><td>{awayTeamDisplay}</td><td>{awayScoreDisplay}</td></tr>");
        }

        htmlBuilder.AppendLine("</table>");

        return htmlBuilder.ToString();
    }
}
