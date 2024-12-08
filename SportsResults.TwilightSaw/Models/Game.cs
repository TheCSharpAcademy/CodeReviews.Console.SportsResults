namespace SportsResults.TwilightSaw.Models;

public class Game(string loser, string winner, int winnerScore, int loserScore)
{
    public string Winner { get; set; } = winner;
    public string Loser { get; set; } = loser;
    public int WinnerScore { get; set; } = winnerScore;
    public int LoserScore { get; set; } = loserScore;

    public override string ToString()
    {
        return $"Winner - {Winner}: {WinnerScore}, Loser - {Loser}: {LoserScore}";
    }
}