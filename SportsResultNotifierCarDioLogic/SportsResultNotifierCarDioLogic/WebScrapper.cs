using HtmlAgilityPack;
using SportsResultNotifierCarDioLogic.Model;

namespace SportsResultNotifierCarDioLogic;

internal class WebScrapper
{
    internal List<Game> Scrapper(int year, int month, int day)
    {
        List<Game> games = new List<Game>();

        HtmlWeb web = new HtmlWeb();
        HtmlDocument document = web.Load($"https://www.basketball-reference.com/boxscores/?month={month}&day={day}&year={year}");

        var gameNodes = document.DocumentNode.SelectNodes("//*[@id=\"content\"]/div[3]/div/table[1]/tbody");

        if(gameNodes != null)
        {
            foreach (var gameNode in gameNodes)
            {
                Game game = new Game();

                game.TeamA = gameNode.SelectSingleNode("tr[1]/td[1]/a").InnerText;
                game.ScoreTeamA = Int32.Parse(gameNode.SelectSingleNode("tr[1]/td[2]").InnerText);

                game.TeamB = gameNode.SelectSingleNode("tr[2]/td[1]").InnerText;
                game.ScoreTeamB = Int32.Parse(gameNode.SelectSingleNode("tr[2]/td[2]").InnerText);

                games.Add(game);
            }
        }
        else
        {
            games = null;
        }

        return games;
    }
}
