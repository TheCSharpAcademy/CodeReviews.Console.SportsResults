using HtmlAgilityPack;
using Kmakai.SportsNotifier.Models;

namespace Kmakai.SportsNotifier;

public class ResultScraper
{
    private readonly string Url = "https://www.basketball-reference.com/boxscores/";

    public Game GetResults()
    {
        var game = new Game();

        var web = new HtmlWeb();
        var doc = web.Load(Url);

        var gameNode = doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[3]/div");

        game.WinningTeam = gameNode.SelectSingleNode("//*[@id=\"content\"]/div[3]/div/table[1]/tbody/tr[2]/td[1]/a").InnerText;
        game.WinningScore = int.Parse(gameNode.SelectSingleNode("//*[@id=\"content\"]/div[3]/div/table[1]/tbody/tr[2]/td[2]")
                       .InnerText);

        game.LosingTeam = gameNode.SelectSingleNode("//*[@id=\"content\"]/div[3]/div/table[1]/tbody/tr[1]/td[1]/a")
                      .InnerText;
        game.LosingScore = int.Parse(gameNode.SelectSingleNode("//*[@id=\"content\"]/div[3]/div/table[1]/tbody/tr[1]/td[2]").InnerText);

        return game;

    }
}
