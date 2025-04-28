using HtmlAgilityPack;
using SportsResultsNotifier.Models;

namespace SportsResultsNotifier.Interfaces;

public interface IScraperService
{
    public Task<List<GameResult>> ScrapeGameResultAsync();
    public Task<Team> ScrapeTeamInfoAsync(HtmlNode gameResultNode);
}
