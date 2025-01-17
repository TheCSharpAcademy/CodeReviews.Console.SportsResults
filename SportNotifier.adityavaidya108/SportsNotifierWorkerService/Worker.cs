using SportsNotifierWorkerService;

namespace SportsNotifier;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly INotificationService _notificationService;

    public Worker(ILogger<Worker> logger, INotificationService notificationService)
    {
        _logger = logger;
        _notificationService = notificationService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _notificationService.StartApp();
                _logger.LogInformation("Email sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"{ex.Message}");
            }
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}