namespace SportsResultsService;

public sealed class WindowsBackgroundService(
    IConfiguration configuration,
    ILogger<WindowsBackgroundService> logger,
    EmailingService emailingService)
    : BackgroundService
{
    private bool _emailSentToday = false;
    private DateTime _lastSentDate = DateTime.MinValue;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (DateTime.Now.Hour == configuration.GetValue<int>("EmailSendingTime:Hours")
                    && DateTime.Now.Minute == configuration.GetValue<int>("EmailSendingTime:Minutes")
                    && !_emailSentToday)
                {
                    emailingService.SendEmail();
                    logger.LogWarning("Today's sports results email sent.");
                    _emailSentToday = true;
                    _lastSentDate = DateTime.Today;
                }
                else if (DateTime.Today > _lastSentDate)
                {
                    _emailSentToday = false;
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);
            Environment.Exit(1);
        }
    }
}
