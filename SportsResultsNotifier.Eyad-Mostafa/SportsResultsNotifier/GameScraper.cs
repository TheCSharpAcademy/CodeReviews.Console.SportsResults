using HtmlAgilityPack;
using System.Net;

namespace SportsResultsNotifier;

internal class GameScraper
{
    private static readonly DateOnly yesterdaysDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
    private static readonly string day = $"{yesterdaysDate.Day:D1}";
    private static readonly string month = $"{yesterdaysDate.Month:D1}";
    private static readonly string year = $"{yesterdaysDate.Year:D4}";
    private static readonly string url = $"https://www.basketball-reference.com/boxscores/?month={month}&day={day}&year={year}";

    public (string Subject, string Body, bool isDataExists) ScrapeGameResults()
    {
        try
        {
            using (var client = new WebClient())
            {
                string html = client.DownloadString(url);
                var document = new HtmlDocument();
                document.LoadHtml(html);

                var titleNode = document.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/h1");
                var resultsNode = document.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]/h2");
                var gameNodes = document.DocumentNode.SelectNodes("//div[contains(@class,'game_summary')]");

                if (titleNode == null || gameNodes == null)
                {
                    Console.WriteLine("Failed to retrieve game data.");
                    return (string.Empty, string.Empty, false);
                }

                string subject = titleNode.InnerText;
                string body = $"<h2>{resultsNode?.InnerText}</h2>";

                foreach (var game in gameNodes)
                {
                    var rows = game.SelectNodes(".//table[1]/tbody/tr");

                    string winnerTeam = "", loserTeam = "";
                    string winnerScore = "", loserScore = "";

                    foreach (var row in rows)
                    {
                        var team = row.SelectSingleNode("./td[1]").InnerText;
                        var score = row.SelectSingleNode("./td[2]").InnerText;
                        var classAttr = row.GetAttributeValue("class", "");

                        if (classAttr.Contains("winner"))
                        {
                            winnerTeam = team;
                            winnerScore = score;
                        }
                        else if (classAttr.Contains("loser"))
                        {
                            loserTeam = team;
                            loserScore = score;
                        }
                    }

                    body += $"{winnerTeam} won with {winnerScore} points over {loserTeam} ({loserScore}) points</br>";
                }

                return (subject, body, true);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Scraping failed: {ex.Message}");
            return (string.Empty, string.Empty, false);
        }
    }
}
