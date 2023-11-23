namespace SportsResultsNotifier.Models;
public class Match
{
    public int Id { get; set; }
    public string Team1 { get; set; } = string.Empty;
    public string Team2 { get; set; } = string.Empty;
    public int Team1Score { get; set; }
    public int Team2Score { get; set; }
}

