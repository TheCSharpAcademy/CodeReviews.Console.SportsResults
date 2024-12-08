namespace SportsResults.TwilightSaw.Models;

public class GameStatistic(string teamWinner, string teamLoser, List<int> winnerStats, List<int> loserStats)
{
    public string TeamWinner { get; set; } = teamWinner;
    public string TeamLoser { get; set; } = teamLoser;
    public List<int> WinnerStats { get; set; } = winnerStats;
    public List<int> LoserStats { get; set; } = loserStats;
}