namespace SportsResultsNotifier.Models;

public class GameResult
{
    public string? Winner { get; init; }
    public string? WinnerScore { get; init; }
    public string? Loser { get; init; }
    public string? LoserScore { get; init; }
}