using HtmlAgilityPack;
using SportsResults.Models;

namespace SportsResults.Services;

/// <summary>
/// Service that is responsible for scraping the basketball reference website.
/// </summary>
public class BasketballReferenceScraperService
{
    #region Fields

    public static readonly string BaseUrl = "https://www.basketball-reference.com";
    public static readonly string BoxscoresUrl = $"{BaseUrl}/boxscores";

    #endregion
    #region Methods

    public static IReadOnlyList<Game> ScrapeBoxscores(DateTime gameDateTime)
    {
        List<Game> games = [];

        var urlBuilder = new UriBuilder(BoxscoresUrl)
        {
            Query = $"month={gameDateTime.Month}&day={gameDateTime.Day}&year={gameDateTime.Year}"
        };

        var web = new HtmlWeb();
        var document = web.Load(urlBuilder.ToString());

        var gameNodes = document.DocumentNode.SelectNodes("//div[@class='game_summaries']/div/table[@class='teams']");
        if (gameNodes is not null)
        {
            foreach (var node in gameNodes)
            {
                games.Add(new Game
                {
                    Home = new BoxScore
                    {
                        Name = node.SelectSingleNode("tbody/tr[2]/td[1]").InnerText,
                        Score = int.Parse(node.SelectSingleNode("tbody/tr[2]/td[2]").InnerText)
                    },
                    Away = new BoxScore
                    {
                        Name = node.SelectSingleNode("tbody/tr[1]/td[1]").InnerText,
                        Score = int.Parse(node.SelectSingleNode("tbody/tr[1]/td[2]").InnerText)
                    }
                });
            }
        }

        return games;
    }

    #endregion
}
