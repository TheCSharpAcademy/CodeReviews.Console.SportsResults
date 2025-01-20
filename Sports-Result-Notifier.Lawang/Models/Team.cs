namespace Sports_Result_Notifier.Lawang;

public class Team
{
    public string TeamName { get; set; } = "";
    public int TotalScore { get; set; }
    public List<int> QuarterScores { get; set; } = new();
}
