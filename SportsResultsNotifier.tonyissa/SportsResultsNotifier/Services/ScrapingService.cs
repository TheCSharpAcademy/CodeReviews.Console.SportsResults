using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SportsResultsNotifier.Services;

public class ScrapingService : BackgroundService
{
    private readonly ILogger<ScrapingService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(24);

    public ScrapingService(ILogger<ScrapingService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Scraping service is starting.");

        while (stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Daily task started at: {time}", DateTimeOffset.Now);

                await DoSomething(stoppingToken);

                _logger.LogInformation("Daily task completed at: {time}", DateTimeOffset.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error has occured");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("Scraping service is stopping.");
    }

    private async Task DoSomething(CancellationToken stoppingToken)
    {

    }
}