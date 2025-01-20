namespace SportsResults;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            Scrapper scrapper = new(GlobalConfig.Url);
            var matches = scrapper.Run();

            EmailSender email = new(matches.ToMailBody());
            email.Send();

            var now = DateTime.Now;
            var nextRunTime = now.AddDays(1);
            var delay = nextRunTime - now;

            await Task.Delay(delay, stoppingToken);
        }
    }
}