using SportsResults.ukpagrace.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace SportsResults.ukpagrace.Scheduler
{
    public class WorkerService : BackgroundService
    {
        public readonly IEmailInterface _email;
        public readonly ILogger<WorkerService> _logger;
        public WorkerService( ILogger<WorkerService> logger, IEmailInterface email)
        {
            _email = email;
            _logger = logger;
        }
        protected override async Task ExecuteAsync (CancellationToken stoppingToken)
        {

            _logger.LogInformation("{Service} is running", nameof(Scheduler));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Sending Email");

                _email.SendMail();

                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);

            }
        }
    }
}
