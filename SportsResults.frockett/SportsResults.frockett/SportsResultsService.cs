using Microsoft.Extensions.Hosting;

namespace SportsResults.frockett;

internal class SportsResultsService : BackgroundService
{
    private readonly ScraperService scraperService;

    public SportsResultsService(ScraperService scraperService)
    {
        this.scraperService = scraperService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        DateTime startTime = DateTime.Now;
        DateTime sendTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, 16, 27, 0);
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
                var results = await scraperService.GetScoresAsync();
                var table = EmailComposer.BuildHtmlTable(results);
                EmailService.SendEmail(table);

                sendTime = sendTime.AddDays(1);
                var delay = sendTime - DateTime.Now;
                await Task.Delay(delay, stoppingToken);

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred. Exception message: {ex.ToString()}");
            Environment.Exit(1);
        }
    }
}
