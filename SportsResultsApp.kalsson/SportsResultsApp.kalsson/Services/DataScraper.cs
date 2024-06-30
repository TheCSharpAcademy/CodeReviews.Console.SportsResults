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

            // Locate the section containing the game data
            var gameNodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'game_summary') and contains(@class, 'expanded') and contains(@class, 'nohover')]");
            if (gameNodes != null)
            {
                Console.WriteLine($"Found {gameNodes.Count} game(s).");

                foreach (var gameNode in gameNodes)
                {
                    var teams = gameNode.SelectNodes(".//a[@href]");
                    var scores = gameNode.SelectNodes(".//tr[contains(@class, 'winner') or contains(@class, 'loser')]/td[@class='right']");

                    if (teams != null && scores != null && teams.Count >= 2 && scores.Count >= 2)
                    {
                        var game = new BasketballGameModel
                        {
                            Team1 = teams[0].InnerText.Trim(),
                            Team2 = teams[1].InnerText.Trim(),
                            Team1Score = int.Parse(scores[0].InnerText.Trim()),
                            Team2Score = int.Parse(scores[1].InnerText.Trim())
                        };

                        Console.WriteLine($"Game: {game.Team1} {game.Team1Score} - {game.Team2Score} {game.Team2}");

                        games.Add(game);
                    }
                    else
                    {
                        Console.WriteLine("Error parsing game data.");
                    }
                }
            }
            else
            {
                Console.WriteLine("No games found.");
            }

            return games;
        }
    }