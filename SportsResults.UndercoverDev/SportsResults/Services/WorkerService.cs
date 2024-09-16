using SportsResults.Controllers;

namespace SportsResults.Services
{
    public class WorkerService : BackgroundService
    {
        private readonly ScraperController _scraperController;
        private readonly ILogger<Startup> _logger;

        public WorkerService(ScraperController scraperController, ILogger<Startup> logger)
        {
            _scraperController = scraperController;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Scraping sports data...");
                _scraperController.Run().Wait();
                _logger.LogInformation("Scraping sports data completed.");

                await Task.Delay(TimeSpan.FromDays(1), stoppingToken); // Run every 24 hours
            }
        }
    }
}