using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace SportsResults
{
    internal class GetWinningTeams
    {
        internal static List<string> WinningNbaTeams()
        {
            var url = "https://www.basketball-reference.com/boxscores/";
            var web = new HtmlWeb();
            var loadedDocument = web.Load(url);

            List<string> winnerNames = new List<string>();
            var gameSummaryNodes = loadedDocument.DocumentNode.SelectNodes("//*[contains(@class, 'winner')]");



            foreach (var node in gameSummaryNodes)
            {
                string innerText = node.InnerText.Trim();

                // clean string
                innerText = Regex.Replace(innerText, @"\s+", " "); // Replace multiple spaces with a single space
                innerText = innerText.Replace("\n", "").Replace("\t", ""); // Remove newline and tab characters
                innerText = innerText.Replace("&nbsp;", ""); // Remove non-breaking space
                innerText = innerText.Replace("Final", "", StringComparison.OrdinalIgnoreCase);
                string teamName = Regex.Match(innerText, @"^[^\d]+").Value.Trim();


                winnerNames.Add(teamName);
            }
            return winnerNames;
        }
    }
}
