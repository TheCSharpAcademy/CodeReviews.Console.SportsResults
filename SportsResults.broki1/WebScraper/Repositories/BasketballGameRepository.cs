using WebScraper.Data;
using WebScraper.Interfaces;
using WebScraper.Models;

namespace WebScraper.Repositories;

public class BasketballGameRepository : IBasketballGameRepository
{
    private readonly WebScraperContext _context;

    public BasketballGameRepository(WebScraperContext context)
    {
        this._context = context;
    }

    public void InsertBasketballGames(List<BasketballGame> basketballGames)
    {
        this._context.BasketballGames.AddRange(basketballGames);
        this._context.SaveChanges();
    }
}
