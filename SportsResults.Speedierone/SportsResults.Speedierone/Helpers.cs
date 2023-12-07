using HtmlAgilityPack;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsResults.Speedierone
{
    public class Helpers
    {
        public static string GetUserInput(string msg)
        {
            Console.WriteLine(msg);
            var email = Console.ReadLine();
            return email;
        }

        public static string GenerateBody(List<Results> results)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine("<body>");
            sb.AppendLine("<h2>NBA Games Summary</h2>");

            if (results.Count > 0)
            {
                sb.AppendLine("<table border='1'>");
                sb.AppendLine("<tr><th>Game #</th><th>Winner</th><th>Score</th><th>Loser</th><th>Score</th></tr>");

                for (int i = 0; i < results.Count; i++)
                {
                    var result = results[i];
                    sb.AppendLine($"<tr><td>{i + 1}</td><td>{result.WinningTeam}</td><td>{result.WinningScore}</td><td>{result.LosingTeam}</td><td>{result.LosingScore}</td></tr>");
                }

                sb.AppendLine("</table>");
            }
            else
            {
                sb.AppendLine("<p>No NBA games played today</p>");
            }

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }
    }
}
