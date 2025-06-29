namespace SportsResults.Models;

public class Match
{
    public string? Team1 { get; set; }
    public string? Team2 { get; set; }
    public int Team1Score { get; set; }
    public int Team2Score { get; set; }
    public string? Winner { get; set; }
    public DateTime Date { get; set; }
}