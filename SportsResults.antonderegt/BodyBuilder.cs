using System.Text;
using SportsResults.Models;

namespace SportsResults;

public class BodyBuilder
{
    public string BuilderMessageBody(List<Game>? games)
    {
        if (games == null || games.Count == 0)
        {
            return "No games to show today...";
        }

        StringBuilder stringBuilder = new();
        foreach (var game in games)
        {
            stringBuilder.AppendLine($"{game.Winner} {game.WinnerPoints}");
            stringBuilder.AppendLine($"{game.Loser} {game.LoserPoints}\n");
        }

        return stringBuilder.ToString();
    }
}