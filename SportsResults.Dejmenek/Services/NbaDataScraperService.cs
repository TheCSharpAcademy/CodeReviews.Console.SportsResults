using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using SportsResults.Dejmenek.Models;

namespace SportsResults.Dejmenek.Services;
public class NbaDataScraperService : INbaDataScraperService
{
    private readonly HtmlWeb _htmlWeb;
    private readonly HtmlDocument _htmlDocument;
    private readonly DateTime _date = DateTime.Now;

    public NbaDataScraperService(HtmlWeb htmlWeb, IConfiguration configuration)
    {
        _htmlWeb = htmlWeb;
        _htmlDocument = _htmlWeb.Load($"{configuration.GetSection("BaseUrl").Value}/?month={_date.Month}&day={_date.Day}&year={_date.Year}");

    }
    public List<TeamStanding> ScrapeEasternConferenceStandings()
    {
        List<TeamStanding> easternTeams = new List<TeamStanding>();
        var easternTeamsElements = _htmlDocument.DocumentNode.SelectNodes("//table[@id=\"confs_standings_E\"]/tbody/tr");

        if (easternTeamsElements is null)
        {
            return [];
        }

        foreach (var easternTeamElement in easternTeamsElements)
        {
            easternTeams.Add(new TeamStanding(
                    new Team(easternTeamElement.SelectSingleNode("./th/a")?.InnerText),
                    easternTeamElement.SelectSingleNode("./td[@data-stat=\"wins\"]")?.InnerText,
                    easternTeamElement.SelectSingleNode("./td[@data-stat=\"losses\"]")?.InnerText,
                    easternTeamElement.SelectSingleNode("./td[@data-stat=\"win_loss_pct\"]")?.InnerText
                ));
        }

        return easternTeams;
    }

    public List<Game> ScrapeGames()
    {
        List<Game> games = new List<Game>();
        var gameElements = _htmlDocument.DocumentNode.SelectNodes("//table[@class=\"teams\"]");

        if (gameElements is null)
        {
            return [];
        }

        foreach (var gameElement in gameElements)
        {
            games.Add(new Game(
                    new Team(gameElement.SelectSingleNode("./tbody/tr[@class=\"loser\"]/td/a")?.InnerText),
                    new Team(gameElement.SelectSingleNode("./tbody/tr[@class=\"winner\"]/td/a")?.InnerText),
                    gameElement.SelectSingleNode("./tbody/tr[@class=\"loser\"]/td[@class=\"right\"]")?.InnerText,
                    gameElement.SelectSingleNode("./tbody/tr[@class=\"winner\"]/td[@class=\"right\"]")?.InnerText
            ));
        }

        return games;
    }

    public List<TeamStanding> ScrapeWesternConferenceStandings()
    {
        List<TeamStanding> westernTeams = new List<TeamStanding>();
        var westernTeamsElements = _htmlDocument.DocumentNode.SelectNodes("//table[@id=\"confs_standings_W\"]/tbody/tr");

        if (westernTeamsElements is null)
        {
            return [];
        }

        foreach (var westernTeamElement in westernTeamsElements)
        {
            westernTeams.Add(new TeamStanding(
                    new Team(westernTeamElement.SelectSingleNode("./th").InnerText),
                    westernTeamElement.SelectSingleNode("./td[@data-stat=\"wins\"]")?.InnerText,
                    westernTeamElement.SelectSingleNode("./td[@data-stat=\"losses\"]")?.InnerText,
                    westernTeamElement.SelectSingleNode("./td[@data-stat=\"win_loss_pct\"]")?.InnerText
                ));
        }

        return westernTeams;
    }
}
