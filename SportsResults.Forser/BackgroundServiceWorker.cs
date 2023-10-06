using SportsResults.Forser.Services;

namespace SportsResults.Forser
{
    internal class BackgroundServiceWorker : BackgroundService
    {
        private readonly ILogger<BackgroundServiceWorker> _logger;
        private readonly Notifier _notifier;

        public BackgroundServiceWorker(
            Notifier notifier,
            ILogger<BackgroundServiceWorker> logger) =>
            (_notifier, _logger) = (notifier, logger);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _notifier.GenerateNotification();
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{message}", ex.Message);
                Environment.Exit(1);
            }

        }
    }
}