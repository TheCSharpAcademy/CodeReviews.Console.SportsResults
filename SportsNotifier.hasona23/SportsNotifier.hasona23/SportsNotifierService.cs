
namespace SportsNotifier.hasona23;

public class SportsNotifierService : BackgroundService
{
    private readonly ILogger<SportsNotifierService> _logger;
    private const int DurationMinutes = 1;
    public SportsNotifierService(ILogger<SportsNotifierService> logger)
    {
        _logger = logger;
        _logger.LogInformation("Starting service");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            
            await Task.Delay(TimeSpan.FromMinutes(DurationMinutes), stoppingToken);
        }
    }
}