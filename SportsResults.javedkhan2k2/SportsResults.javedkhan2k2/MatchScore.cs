using System.Security.Cryptography;

namespace SportsResult;

public class MatchScore
{
    public string MatchTitle { get; set; }
    public string MatchDate {get;set;}
    public Team Winner { get; set; }
    public Team Loser { get; set; }
    public MatchTopScorer TopScorer { get; set; }
    public MatchTopRebounder TopRebounder { get; set; }

    public MatchScore()
    {
        Winner = new Team();
        Loser = new Team();
        TopScorer = new MatchTopScorer();
        TopRebounder = new MatchTopRebounder();
    }
}