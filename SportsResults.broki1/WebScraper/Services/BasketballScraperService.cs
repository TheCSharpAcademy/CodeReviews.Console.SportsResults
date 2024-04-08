using HtmlAgilityPack;
using WebScraper.Interfaces;
using WebScraper.Models;

namespace WebScraper.Services;

public class BasketballScraperService : IWebScraperService
{
    private readonly IBasketballGameRepository _basketballGameRepository;
    private readonly string _url;
    private HtmlWeb _web;
    private HtmlDocument _document;

    public BasketballScraperService(IBasketballGameRepository basketballGameRepository)
    {
        this._url = "https://www.basketball-reference.com/boxscores/";
        this._web = new HtmlWeb();
        this._document = this._web.Load(this._url);

        this._basketballGameRepository = basketballGameRepository;
    }

    public void InsertGames()
    {
        var games = this._document.DocumentNode.SelectNodes("//div[@class='game_summary expanded nohover ']").ToArray();
        var basketballGames = new List<BasketballGame>();

        foreach (var game in games)
        {
            var awayTeam = game.SelectSingleNode("table[@class='teams']//tbody//tr[1]//td[1]//a").InnerText;
            var homeTeam = game.SelectSingleNode("table[@class='teams']//tbody//tr[2]//td[1]//a").InnerText;

            var awayScore = Convert.ToInt32(game.SelectSingleNode("table[@class='teams']//tbody//tr[1]//td[2]").InnerText);
            var homeScore = Convert.ToInt32(game.SelectSingleNode("table[@class='teams']//tbody//tr[2]//td[2]").InnerText);



            var basketballGame = new BasketballGame
            {
                Date = DateTime.Today.AddDays(-1),
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                HomeTeamScore = homeScore,
                AwayTeamScore = awayScore
            };

            basketballGames.Add(basketballGame);
            // Console.WriteLine(game.InnerHtml);
        }

        this._basketballGameRepository.InsertBasketballGames(basketballGames);
    }
}
