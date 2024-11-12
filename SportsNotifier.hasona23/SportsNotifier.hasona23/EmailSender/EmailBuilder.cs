using System.Text;
using SportsNotifier.hasona23.Models;

namespace SportsNotifier.hasona23.EmailSender;

public static class EmailBuilder
{
    public static string BuildMessage((string title, List<Game> games) data)
    {
        StringBuilder builder = new StringBuilder();
        
        builder.AppendLine($"{data.title}\n\n");
        foreach (var game in data.games)
        {
            builder.AppendLine($"{game.WinnerTeam} : {game.WinnerScore} | {game.LoserTeam} : {game.LoserScore}\n");
        }

        if (data.games.Count == 0)
            builder.AppendLine("No Games");
        return builder.ToString();
    }
}