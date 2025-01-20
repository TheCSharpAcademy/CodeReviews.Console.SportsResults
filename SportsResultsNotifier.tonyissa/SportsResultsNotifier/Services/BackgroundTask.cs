using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SportsResultsNotifier.Services;

public class BackgroundTask : BackgroundService
{
    private readonly HtmlScraperService _scraperService;
    private readonly MailService _mailService;
    private readonly ILogger<BackgroundTask> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(24);

    public BackgroundTask(HtmlScraperService scraperService, MailService mailService, ILogger<BackgroundTask> logger)
    {
        _scraperService = scraperService;
        _mailService = mailService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Scraping service is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Daily task started at: {time}", DateTimeOffset.Now);

                await Run(stoppingToken);

                _logger.LogInformation("Daily task completed at: {time}", DateTimeOffset.Now);
            }
            catch
            {
                _logger.LogError("An error has occured at {time}.", DateTimeOffset.Now);
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("Scraping service is stopping.");
    }

    private async Task Run(CancellationToken stoppingToken)
    {
        var scoreList = await _scraperService.ExecuteScrapeAsync(stoppingToken);
        _mailService.SendSportsUpdate(scoreList);
    }
}