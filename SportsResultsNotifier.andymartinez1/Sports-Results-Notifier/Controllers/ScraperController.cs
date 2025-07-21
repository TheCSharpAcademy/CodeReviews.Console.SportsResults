using Spectre.Console;
using Sports_Results_Notifier.Models;
using Sports_Results_Notifier.Services;
using Sports_Results_Notifier.Views;

namespace Sports_Results_Notifier.Controllers;

public class ScraperController
{
    private readonly IScraperService _scraperService;
    private readonly IEmailService _emailService;

    public ScraperController(IScraperService scraperService, IEmailService emailService)
    {
        _scraperService = scraperService;
        _emailService = emailService;
    }

    public Game GetGameInfo()
    {
        var url = "https://www.basketball-reference.com/boxscores/";

        var doc = _scraperService.ScrapeHtml(url);

        var game = _scraperService.GetGamePlayedInfo(doc);

        UserInterface.ViewTable(game);

        return game;
    }

    public void SendEmail(Game game)
    {
        _emailService.SendEmail(game);

        AnsiConsole.WriteLine("Successfully sent email");
    }
}
