using System.Text;

namespace SportsResults.Models;

public class Match
{
    public string? Team1 { get; set; }
    public string? Team2 { get; set; }
    public int Team1Score { get; set; }
    public int Team2Score { get; set; }
    public string? Winner { get; set; }
    public DateTime Date { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"{Team1}   {Team1Score}");
        sb.AppendLine($"{Team2}   {Team2Score}");
        sb.AppendLine($"Winner: {Winner}");

        return sb.ToString();
    }
}