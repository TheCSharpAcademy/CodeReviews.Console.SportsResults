using WebScraper.Models;

namespace WebScraper.Interfaces;

public interface IBasketballGameRepository
{
    public void InsertBasketballGames(List<BasketballGame> basketballGames);
}
