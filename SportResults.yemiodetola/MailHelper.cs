using System.Text;
using SportResults.Models;


namespace SportResults.Helpers;

public class MailHelper
{
  public static string GenerateMailBody(List<Result> results)
  {
    var sb = new StringBuilder();
    sb.AppendLine("<html>");
    sb.AppendLine("<body>");

    if (results.Count == 0)
    {
      sb.AppendLine("<h3>No NBA games played today :(</h3>");
    }
    else
    {
      sb.AppendLine("<table>");
      sb.AppendLine("<thead>");
      sb.AppendLine("<tr>");
      sb.AppendLine("<th>Winner</th>");
      sb.AppendLine("<th>Winner Score</th>");
      sb.AppendLine("<th>Looser</th>");
      sb.AppendLine("<th>Looser Score</th>");
      sb.AppendLine("</tr>");
      sb.AppendLine("</thead>");
      sb.AppendLine("<tbody>");
      foreach (var result in results)
      {
        sb.AppendLine("<tr>");
        sb.AppendLine($"<td>{result.Winner}</td>");
        sb.AppendLine($"<td><b>{result.WinnerScore}</b></td>");
        sb.AppendLine($"<td>{result.Loser}</td>");
        sb.AppendLine($"<td>{result.LoserScore}</td>");
        sb.AppendLine("</tr>");
      }
    }
    sb.AppendLine("</body>");
    sb.AppendLine("</html>");
    return sb.ToString();
  }
}