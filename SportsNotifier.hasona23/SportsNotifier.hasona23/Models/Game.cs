namespace SportsNotifier.hasona23.Models;

public record Game(string WinnerTeam, string LoserTeam, int WinnerScore, int LoserScore)
{
    public override string ToString()
    {
        return $"Winner: {WinnerTeam}, Loser: {LoserTeam}, WinnerScore: {WinnerScore}, LoserScore: {LoserScore}";
    }
}