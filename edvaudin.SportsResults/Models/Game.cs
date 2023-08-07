namespace SportsResults;

public class Game
{
    public string WinningTeam { get; set; }
    public string LosingTeam { get; set; }
    public int WinningScore { get; set; }
    public int LosingScore { get; set; }

    public string HighestPointScorer { get; set; }
    public int HighestPlayerPoints { get; set; }

    public string MostTotalRebounder { get; set; }
    public int MostPlayerTotalRebounds { get; set; }

    public override string ToString()
    {
        return $"{WinningTeam} ({WinningScore} - {LosingScore}) {LosingTeam}\n" +
               $"PTS - {HighestPointScorer} ({HighestPlayerPoints}) | TRB - {MostTotalRebounder} ({MostPlayerTotalRebounds})";
    }

}
