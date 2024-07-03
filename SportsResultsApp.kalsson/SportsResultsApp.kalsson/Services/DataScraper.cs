using HtmlAgilityPack;
using SportsResultsApp.kalsson.Models;

namespace SportsResultsApp.kalsson.Services;

public class DataScraper
{
    private const string Url = "https://www.basketball-reference.com/boxscores/";

    /// <summary>
    /// Scrapes basketball game data from the provided URL.
    /// </summary>
    /// <returns>A list of BasketballGameModel objects representing the scraped game data.</returns>
    public async Task<List<BasketballGameModel>> ScrapeBasketballGamesAsync()
    {
        var httpClient = new HttpClient();
        var html = await httpClient.GetStringAsync(Url);

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        var games = new List<BasketballGameModel>();

        // Print the loaded HTML content for debugging
        Console.WriteLine("Loaded HTML:");
        Console.WriteLine(html.Substring(0,
            Math.Min(html.Length, 1000))); // Print the first 1000 characters of the HTML

        // Locate the section containing the game data
        var gameNodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'game_summary')]");
        if (gameNodes != null)
        {
            Console.WriteLine($"Found {gameNodes.Count} game(s).");

            foreach (var gameNode in gameNodes)
            {
                Console.WriteLine(
                    $"Game node HTML: {gameNode.InnerHtml.Substring(0, Math.Min(gameNode.InnerHtml.Length, 500))}");

                var winnerNode = gameNode.SelectSingleNode(".//table/tbody/tr[1]");
                var loserNode = gameNode.SelectSingleNode(".//table/tbody/tr[2]");

                if (winnerNode != null && loserNode != null)
                {
                    var team1 = winnerNode.SelectSingleNode("td[1]/a")?.InnerText.Trim();
                    var team1Score = int.Parse(winnerNode.SelectSingleNode("td[2]")?.InnerText.Trim() ?? "0");
                    var team2 = loserNode.SelectSingleNode("td[1]/a")?.InnerText.Trim();
                    var team2Score = int.Parse(loserNode.SelectSingleNode("td[2]")?.InnerText.Trim() ?? "0");

                    var game = new BasketballGameModel
                    {
                        Team1 = team1,
                        Team1Score = team1Score,
                        Team2 = team2,
                        Team2Score = team2Score
                    };

                    Console.WriteLine($"Game: {game}");

                    games.Add(game);
                }
                else
                {
                    Console.WriteLine("Error parsing game data: winnerNode or loserNode is null.");
                }
            }
        }
        else
        {
            Console.WriteLine("No game summaries found using the specified XPath.");
        }

        return games;
    }
}