using HtmlAgilityPack;
using SportsResultsApp.kalsson.Models;

namespace SportsResultsApp.kalsson.Services;

public class DataScraper
{
    private const string Url = "https://www.basketball-reference.com/boxscores/";

    public async Task<List<BasketballGameModel>> ScrapeBasketballGamesAsync()
    {
        var httpClient = new HttpClient();
        var html = await httpClient.GetStringAsync(Url);

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        var games = new List<BasketballGameModel>();

        var gameNodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'game_summary')]");
        if (gameNodes != null)
        {
            foreach (var gameNode in gameNodes)
            {
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

                    games.Add(game);
                }
            }
        }

        return games;
    }
}