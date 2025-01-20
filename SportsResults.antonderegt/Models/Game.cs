namespace SportsResults.Models;

public class Game
{
    public string? Winner { get; set; }
    public int WinnerPoints { get; set; }
    public string? Loser { get; set; }
    public int LoserPoints { get; set; }
}