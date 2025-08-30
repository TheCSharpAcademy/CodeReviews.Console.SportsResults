
namespace SportsResultsNotifier.Model;

public class GameData
{
    public List<TeamData> TeamData { get; set; }
    public TeamData WinningTeam {  get; set; }
    public string PtsStat { get; set; }
    public string TrbStat { get; set; }

    public override string ToString()
    {
        return $"{TeamData[0].Name} at {TeamData[1].Name} ({TeamData[0].TotalScore} - {TeamData[1].TotalScore})"
            + $"\n{WinningTeam.Name} Wins!"
            + $"\n\nPTS: {PtsStat}"
            + $"\nTRB: {TrbStat}";
    }
}
