using System.Text;
using SportsResults.Models;

namespace SportsResults;

public static class MessageHelper
{
    private static bool AreNewsFromToday(IEnumerable<Match> matches)
    {
        return matches.First().Date==DateTime.Today;
    }
    public static string GetFormattedMessageBody(List<Match> matches)
    {
       StringBuilder sb = new StringBuilder();
       if (!matches.Any())
       {
           sb.AppendLine("No matches found for this season");
           return sb.ToString();
       }
       sb.AppendLine(
           $"Hello, here are the news for today ({DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day})");
       if (!AreNewsFromToday(matches))
       {
           sb.AppendLine("No matches from today found, the most recent matches:");
           sb.AppendLine();
       }

       sb.AppendLine($"Matches from: {matches.First().Date.ToShortDateString()}");
       
       foreach (var match in matches)
       {    
           sb.AppendLine(match.ToString());
           sb.AppendLine("__________________________________");
           sb.AppendLine();
       }

       sb.AppendLine("Thank you for using our service, see you tomorrow!");
       return sb.ToString();
    }

}