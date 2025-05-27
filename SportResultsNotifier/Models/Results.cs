namespace SportResultsNotifier.Models;

internal class Results
{
    internal string? Subject { get; set; }
    internal string? Body { get; set; }
}

internal class Game
{
    internal string? HomeTeam { get; set; }
    internal string? AwayTeam { get; set; }
    internal int HomeScore { get; set; }
    internal int AwayScore { get; set; }
    internal string? HomeTeamRef { get; set; }
    internal string? AwayTeamRef { get; set; }
    internal string? GameRef { get; set; }
    internal bool HomeWin => HomeScore > AwayScore;
    internal bool NoWinner => HomeScore == AwayScore;
}