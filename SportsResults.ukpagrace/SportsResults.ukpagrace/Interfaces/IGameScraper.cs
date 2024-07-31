using SportsResults.ukpagrace.Model;
namespace SportsResults.ukpagrace.Interfaces
{
    public interface IGameScraper
    {
        public List<Game> ScrapGames(string url);
    }
}
