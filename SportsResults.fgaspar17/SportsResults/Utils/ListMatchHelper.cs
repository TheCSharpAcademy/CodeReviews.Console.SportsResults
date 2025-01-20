using System.Text;

namespace SportsResults;

public static class ListMatchHelper
{
    public static string ToMailBody(this List<Match> matches)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"------------- Matches, Date: {DateTime.Today:yyyy-MM-dd} -------------");
        sb.AppendLine();

        if (matches.Count == 0)
        {
            sb.AppendLine("No matches today.");
            return sb.ToString();
        }

        foreach (var match in matches)
        {
            if (match.Winner != null && match.Loser != null)
            {
                sb.AppendLine($"* Winner: {match.Winner.Name} (Score: {match.Winner.Score})");
                sb.AppendLine($"  Loser:  {match.Loser.Name} (Score: {match.Loser.Score})");
                sb.AppendLine();
            }
            else
            {
                sb.AppendLine("Incomplete match data.");
                sb.AppendLine();
            }
        }

        return sb.ToString();
    }
}