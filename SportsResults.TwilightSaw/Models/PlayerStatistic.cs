namespace SportsResults.TwilightSaw.Models;

public class PlayerStatistic(string ptsPlayer, string trbPlayer, int ptsPlayerStats, int trbPlayerStats)
{
    public string PtsPlayer { get; set; } = ptsPlayer;
    public string TrbPlayer { get; set; } = trbPlayer;
    public int PtsPlayerStats { get; set; } = ptsPlayerStats;
    public int TrbPlayerStats { get; set; } = trbPlayerStats;
}