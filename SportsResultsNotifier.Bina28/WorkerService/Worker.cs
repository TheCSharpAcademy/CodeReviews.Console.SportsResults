using Hangfire;
using WorkerService.Services;

namespace WorkerService;

public class Worker : BackgroundService
{
	private readonly IBackgroundJobClient _backgroundJobClient;
	private readonly BasketballDataScraper _dataScraper;

	public Worker(IBackgroundJobClient backgroundJobClient, BasketballDataScraper dataScraper)
	{
		_backgroundJobClient = backgroundJobClient;
		_dataScraper = dataScraper;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		// Configure Hangfire job scheduling here
		RecurringJob.AddOrUpdate(
			"daily-email-job",
			() => _dataScraper.ScrapeData(),
			"* * * * *");  // Runs every minute

		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(1000, stoppingToken); // Keep worker running
		}
	}
}
