using HtmlAgilityPack;

namespace SportsResultsNotifier;

internal class GameScraper
{
    private static readonly string Url = $"https://www.basketball-reference.com/boxscores/";

    public (string Subject, string Body, bool IsDataExists) ScrapeGameResults()
    {
        try
        {
            var html = FetchHtmlContent(Url);
            if (string.IsNullOrEmpty(html))
            {
                Console.WriteLine("Failed to fetch HTML content.");
                return (string.Empty, string.Empty, false);
            }

            var document = new HtmlDocument();
            document.LoadHtml(html);

            var titleNode = document.DocumentNode.SelectSingleNode("//*[@id='content']/h1");
            var resultsNode = document.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]/h2");
            var gameNodes = document.DocumentNode.SelectNodes("//div[contains(@class,'game_summary')]");

            if (titleNode == null || gameNodes == null)
            {
                Console.WriteLine("Failed to retrieve game data.");
                return (string.Empty, string.Empty, false);
            }

            string subject = titleNode.InnerText;
            string body = resultsNode != null ? $"<h2>{resultsNode.InnerText}</h2>" : string.Empty;

            foreach (var game in gameNodes)
            {
                var rows = game.SelectNodes(".//table[1]/tbody/tr");

                if (rows == null) continue;

                string winnerTeam = string.Empty;
                string loserTeam = string.Empty;
                string winnerScore = string.Empty;
                string loserScore = string.Empty;

                foreach (var row in rows)
                {
                    var teamNode = row.SelectSingleNode("./td[1]");
                    var scoreNode = row.SelectSingleNode("./td[2]");
                    var classAttr = row.GetAttributeValue("class", string.Empty);

                    if (teamNode == null || scoreNode == null) continue;

                    string team = teamNode.InnerText.Trim();
                    string score = scoreNode.InnerText.Trim();

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

                if (!string.IsNullOrEmpty(winnerTeam) && !string.IsNullOrEmpty(loserTeam))
                {
                    body += $"{winnerTeam} won with {winnerScore} points over {loserTeam} ({loserScore}) points<br>";
                }
            }

            return (subject, body, true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Scraping failed: {ex.Message}");
            return (string.Empty, string.Empty, false);
        }
    }

    private string FetchHtmlContent(string url)
    {
        try
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP Request Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General Error: {ex.Message}");
        }

        return string.Empty;
    }
}