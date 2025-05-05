using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using SportsResultsNotifier.Interfaces;
using SportsResultsNotifier.Models;

namespace SportsResultsNotifier.Services;

public class ScraperService : IScraperService
{
    private readonly IConfiguration _configuration;

    public ScraperService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<GameResult>> ScrapeGameResultAsync()
    {
        HtmlDocument tempHtmlDoc = new();
        List<GameResult> gameResults = new();

        var gamesResultsNodes = await LoadGameResultsAsync();
        if (gamesResultsNodes == null || gamesResultsNodes.Count == 0)
        {
            throw new Exception("No game results found.");
        }

        foreach (var gameResultNode in gamesResultsNodes)
        {
            tempHtmlDoc.LoadHtml(gameResultNode.OuterHtml);

            var winningNode = tempHtmlDoc.DocumentNode.Descendants(0)
            .Where(n => n.HasClass("winner")).ToList(); // am expecting to get exactly one node matching this filter

            var losingNode = tempHtmlDoc.DocumentNode.Descendants(0)
            .Where(n => n.HasClass("loser")).ToList(); // am expecting to get exactly one node matching this filter

            var LosingTeam = await ScrapeTeamInfoAsync(losingNode[0]);
            var WinningTeam = await ScrapeTeamInfoAsync(winningNode[0]);

            GameResult gameResult = new()
            {
                WinningTeam = WinningTeam,
                LosingTeam = LosingTeam
            };
            gameResults.Add(gameResult);
        }
        return gameResults;
    }

    public async Task<Team> ScrapeTeamInfoAsync(HtmlNode gameResultNode)
    {
        HtmlDocument teamHtmlDoc = new();
        teamHtmlDoc.LoadHtml(gameResultNode.OuterHtml);
        // each team has 3 td elements, the first one is the team name, the second one is the score, and the third one is irrelavent.

        var teamInfoNodes = teamHtmlDoc.DocumentNode.SelectNodes("//td").ToList();

        var teamName = teamInfoNodes[0].InnerText;
        var teamScore = teamInfoNodes[1].InnerText;

        return new Team { TeamName = teamName, Score = teamScore };
    }

    private async Task<List<HtmlNode>> LoadGameResultsAsync()
    {
        var scrapeUrl = _configuration.GetValue<string>("ScrapeUrl");
        var webDoc = new HtmlWeb();
        var fullPageHtmlDoc = await webDoc.LoadFromWebAsync(scrapeUrl); // loading the full html document

        return fullPageHtmlDoc.DocumentNode.Descendants(0).Where(n => n.HasClass("game_summary")).ToList();
    }
}