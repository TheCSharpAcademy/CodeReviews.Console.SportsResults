using System.Text;
using HtmlAgilityPack;
using SportsNotifier.hasona23.Config;
using SportsNotifier.hasona23.Models;

namespace SportsNotifier.hasona23.Scrapper;

public class Scrapper
{
    private string Url { get; }
    private ILogger<Scrapper> Logger { get; }
    public Scrapper(ILogger<Scrapper> logger)
    {
        Url = AppSetting.BasketBallUrl;
        Logger = logger;
        Logger.LogInformation("Scrapper started");
    }

    public (string Title, List<Game> Games) Scrape()
    {
        try
        {
            var web = new HtmlWeb();
            var document = web.Load(Url);
            List<Game> games = [];
            var gameDivs = document.DocumentNode.SelectNodes(@"
        //div[@class='game_summary expanded nohover ']")?.ToList() ?? [];
            string date = document.DocumentNode.SelectSingleNode("//body/div[@id='wrap']/div[@class='index']/h1")
                .InnerText;
            if (gameDivs.Count == 0)
            {
                Logger.LogInformation("No games found");
                return (date, games);
            }

            Logger.LogInformation("---------------------------Scrapper-Started-------------------------");
            foreach (var gameDiv in gameDivs)
            {
                games.Add(
                    new Game(WinnerTeam: gameDiv.SelectSingleNode(".//table/tbody/tr[1]/td[1]").InnerText,
                        LoserTeam: gameDiv.SelectSingleNode(".//table/tbody/tr[2]/td[1]").InnerText,
                        WinnerScore: int.Parse(gameDiv.SelectSingleNode(".//table/tbody/tr[1]/td[2]").InnerText),
                        LoserScore: int.Parse(gameDiv.SelectSingleNode(".//table/tbody/tr[2]/td[2]").InnerText))
                );
            }

            Logger.LogInformation(date);
            Logger.LogInformation(gameDivs.Count.ToString());
            var strBuilder = new StringBuilder();
            foreach (var game in games)
            {
                strBuilder.AppendLine(game.ToString());
            }

            Logger.LogInformation(strBuilder.ToString());
            Logger.LogInformation("----------------------Scrapper-finished---------------------");
            return (date, games);
        }
        catch (Exception e)
        {
            Logger.LogError(e.Message);
        }
        return (string.Empty,[]);
    }

}