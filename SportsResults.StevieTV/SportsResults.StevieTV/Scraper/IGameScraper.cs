using SportsResults.StevieTV.Models;

namespace SportsResults.StevieTV.Scraper;

public interface IGameScraper
{
    List<Game> GetGames(string url);
}