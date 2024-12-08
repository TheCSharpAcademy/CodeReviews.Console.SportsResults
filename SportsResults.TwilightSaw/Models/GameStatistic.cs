namespace SportsResults.TwilightSaw.Models;

public class GameStatistic(string teamWinner, string teamLoser, List<int> winnerStats, List<int> loserStats, string ptsPlayer, string trbPlayer, int ptsPlayerStats, int trbPlayerStats)
{
    public string TeamWinner { get; set; } = teamWinner;
    public string TeamLoser { get; set; } = teamLoser;
    public List<int> WinnerStats { get; set; } = winnerStats;
    public List<int> LoserStats { get; set; } = loserStats;

    public string PtsPlayer { get; set; } = ptsPlayer;
    public string TrbPlayer { get; set; } = trbPlayer;
    public int PtsPlayerStats { get; set; } = ptsPlayerStats;
    public int TrbPlayerStats { get; set; } = trbPlayerStats;

    public override string ToString()
    {
        return $"{TeamWinner} : {WinnerStats.Aggregate("", (current, x) => current + x + " ")}" +
               $"\t{PtsPlayer} {PtsPlayerStats}" +
               $"\n {TeamLoser} : {LoserStats.Aggregate("", (current, x) => current + x + " ")}" +
               $"\t{TrbPlayer} {TrbPlayerStats}";
    }
}