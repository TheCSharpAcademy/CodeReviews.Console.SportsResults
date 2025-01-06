using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SportsResult.KroksasC.Services
{
    public class DailyTaskService : BackgroundService
    {
        private readonly ILogger<DailyTaskService> _logger;

        public DailyTaskService(ILogger<DailyTaskService> logger)
        {
            _logger = logger;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                var now = DateTime.Now;

                var specificTime = DateTime.Now.Date.AddHours(15).AddMinutes(8);// Change hours in a way that you want, in this exemples hours is 15:08:00 and date is today

                DateTime nextRunTime = now < specificTime ? specificTime : specificTime.AddDays(1);

                // Calculate the delay until the next scheduled time
                var delay = nextRunTime - now;

                _logger.LogInformation($"Next task scheduled at: {nextRunTime}");

                try
                {
                    await Task.Delay(delay, stoppingToken);
                    // Perform your task here
                    _logger.LogInformation("Daily task started.");
                    PerformDailyTask();
                    _logger.LogInformation("Daily task completed.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error during task execution: {ex.Message}");
                }
                _logger.LogInformation("Waiting for another time");
            }
        }

        private void PerformDailyTask()
        {
            EmailService.SendEmail();
        }
    }
}
