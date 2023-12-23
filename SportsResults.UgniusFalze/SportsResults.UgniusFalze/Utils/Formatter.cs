using System.Text;
using SportsResults.UgniusFalze.Models;

namespace SportsResults.UgniusFalze.Utils;

public static class Formatter
{
    public static string FormatGames(List<Game> games)
    {
        var gamesString = new StringBuilder()
            .AppendLine("<html>")
            .AppendLine("<body>");
        if (games.Count == 0)
        {
            gamesString
                .AppendLine("<p>No games were played today</p>")
                .AppendLine("</body>")
                .AppendLine("</html>");
            return gamesString.ToString();
        }

        gamesString
            .AppendLine("<table>")
            .AppendLine("<tr>")
            .AppendLine("<th>Game number</th>")
            .AppendLine("<th>Winner</th>")
            .AppendLine("<th>Winner Score</th>")
            .AppendLine("<th>Looser</th>")
            .AppendLine("<th>Looser Score</th>")
            .AppendLine("</tr>");
        for (var i = 0; i < games.Count; i++)
        {
            var game = games[i];
            gamesString
                .AppendLine("<tr>")
                .AppendLine($"<td>{i + 1}</td>")
                .AppendLine($"<td>{game.Winner}</td>")
                .AppendLine($"<td>{game.WinnerScore}</td>")
                .AppendLine($"<td>{game.Looser}</td>")
                .AppendLine($"<td>{game.LooserScore}</td>")
                .AppendLine("</tr>");
        }

        gamesString
            .AppendLine("</body>")
            .AppendLine("</html>");
        return gamesString.ToString();
    }
}