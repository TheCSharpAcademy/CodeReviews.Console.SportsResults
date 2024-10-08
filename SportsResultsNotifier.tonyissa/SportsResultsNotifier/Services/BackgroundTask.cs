using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SportsResultsNotifier.Services;

public class BackgroundTask : BackgroundService
{
    private readonly HtmlScraperService _scraperService;
    private readonly ILogger<BackgroundTask> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(24);

    public BackgroundTask(HtmlScraperService scraperService, ILogger<BackgroundTask> logger)
    {
        _scraperService = scraperService;
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

                await _scraperService.ExecuteScrapeAsync(stoppingToken);

                _logger.LogInformation("Daily task completed at: {time}", DateTimeOffset.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error has occured at {time}. Error: {message}", DateTimeOffset.Now, ex.Message);
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("Scraping service is stopping.");
    }
}