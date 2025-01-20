using HtmlAgilityPack;
using SportsResults.Models;
using System.Text.RegularExpressions;

namespace SportsResults
{
    internal class GetScores
    {
        internal static List<Game> GetGameScores()
        {
            var url = "https://www.basketball-reference.com/boxscores/";
            var web = new HtmlWeb();
            var loadedDocument = web.Load(url);

            List<Game> games = new List<Game>();
            var gameSummaryNodes = loadedDocument.DocumentNode.SelectNodes("//*[contains(@class, 'teams')]");

            foreach (var node in gameSummaryNodes)
            {
                string innerText = node.InnerText.Trim();

                innerText = Regex.Replace(innerText, @"\s+", " "); // Replace multiple spaces with a single space
                innerText = innerText.Replace("\n", "").Replace("\t", ""); // Remove newline and tab characters
                innerText = innerText.Replace("&nbsp;", ""); // Remove non-breaking space
                innerText = innerText.Replace("Final", "", StringComparison.OrdinalIgnoreCase).Trim();
                string team1 = innerText.Split("  ")[0];
                string team2 = innerText.Split("  ")[1];

                Game game = new Game();
                game.team1PlusScore = team1;
                game.team2PlusScore = team2;

                games.Add(game);
            }
            return games;
        }
    }
}
