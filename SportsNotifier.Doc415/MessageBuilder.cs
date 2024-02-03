namespace SportsNotifier;
using SportsNotifier.Models;

internal class MessageBuilder
{
    public string BuildMessage((string title, List<Game> games) data)
    {
        string message = $"{data.Item1}\n\n";
        foreach (var game in data.Item2)
        {
            message += $"{game.Winner}: {game.WinnerScore} - {game.Looser}: {game.LooserScore}\n";
        }
        return message;
    }
}
