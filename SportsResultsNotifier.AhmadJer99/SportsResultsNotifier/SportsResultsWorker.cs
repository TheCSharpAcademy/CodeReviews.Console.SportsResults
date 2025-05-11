using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SportsResultsNotifier.Interfaces;
using System.Text;

namespace SportsResultsNotifier;

public class SportsResultsWorker : BackgroundService
{
    private readonly IScraperService _scarperService;
    private readonly ILogger<SportsResultsWorker> _logger;
    private readonly IEmailService _emailService;

    private const string _recieverEmail = "ajerjees010@gmail.com";
    private const string _subject = "Daily NBA News Letter";

    public SportsResultsWorker(IScraperService scraperService,IEmailService emailService, ILogger<SportsResultsWorker> logger)  
    {
        _scarperService = scraperService;
        _logger = logger;
        _emailService = emailService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await RunAsync();
            Thread.Sleep(TimeSpan.FromHours(24));
        }
    }

    private async Task RunAsync()
    {
        StringBuilder stringBuilder = new();
        int gameNum = 1;

        try
        {
            var gameResults = await _scarperService.ScrapeGameResultAsync();
            _logger.LogInformation("Service Running...");
            foreach (var gameResult in gameResults)
            {
                stringBuilder.AppendLine($"<font size=5><strong>Game{gameNum++}:</strong></font size><br>");

                stringBuilder.Append($"<font size=4><strong>{gameResult.WinningTeam.TeamName} Won against {gameResult.LosingTeam.TeamName}!</strong></font size><br>");
                stringBuilder.Append(@$"<font size=3><strong>{gameResult.WinningTeam.TeamName} ({gameResult.WinningTeam.Score}) Vs 
                                    {gameResult.LosingTeam.TeamName} ({gameResult.LosingTeam.Score})</strong></font size><br>");
                stringBuilder.AppendLine("<br>--------------------------------------------------<br>");
            }
            var emailBody = stringBuilder.ToString();
            await _emailService.SendLocalEmailAsync(_recieverEmail, _subject, emailBody);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while scraping game results.");
        }
    }
}