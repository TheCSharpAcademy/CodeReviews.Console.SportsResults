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
            DateTime startTime = DateTime.Now;
            DateTime sendTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, 10, 01, 0);
            if (startTime > sendTime)
            {
                sendTime = sendTime.AddDays(1);
            }
            TimeSpan startupDelay = sendTime - startTime;
            await Task.Delay(startupDelay, stoppingToken);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _notifier.GenerateNotification();
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    sendTime = sendTime.AddDays(1);
                    TimeSpan delay = sendTime - DateTime.Now;
                    await Task.Delay(delay, stoppingToken);
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