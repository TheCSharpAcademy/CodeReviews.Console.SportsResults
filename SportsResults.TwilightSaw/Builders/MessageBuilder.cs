using SportsResults.TwilightSaw.Controllers;

namespace SportsResults.TwilightSaw.Builders;

public class MessageBuilder(ScrapperController scrapperController)
{
    public string GetGameMessage()
    {
        var gamesList = scrapperController.GetGames().Select(p => p.ToString()).ToList();
        var message = gamesList.Aggregate("", (current, x) => current + x + "\n");
        return message;
    }

    public string GetGameStatisticsMessage()
    {
        var gamesStatisticList = scrapperController.GetGames().Select(p => p.ToString()).ToList();
        var message = gamesStatisticList.Aggregate("", (current, x) => current + x + "\n");
        return message;
    }
}