
namespace SportsResultsNotifier.Model;

public class TeamData
{
    public string Name { get; set; }
    public int TotalScore { get; set; }
    public List<int> ScoreByQuarter { get; set; } = [];

    public override string ToString()
    {
        List<string> headers = ["Q1", "Q2", "Q3", "Q4", "OT"];
        var str = $"{Name}:\n\t{headers[0]}- {ScoreByQuarter[0]} " +
            $"\t{headers[1]}- {ScoreByQuarter[1]} " +
            $"\t{headers[2]}- {ScoreByQuarter[2]} " +
            $"\t{headers[3]}- {ScoreByQuarter[3]}";

        if(ScoreByQuarter.Count >= 5)
        {
            for(int i = 4; i < ScoreByQuarter.Count; i++)
            {
                str += $"\t{headers[i]}- {ScoreByQuarter[4]} ";
            }
        }

        return str;
    }
}
