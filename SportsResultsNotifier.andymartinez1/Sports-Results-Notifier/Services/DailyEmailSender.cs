using Microsoft.Extensions.Hosting;
using Sports_Results_Notifier.Services;

public class DailyEmailSender : BackgroundService
{
    private readonly IEmailService _emailService;
    private readonly IScraperService _scraperService;

    public DailyEmailSender(IEmailService emailService, IScraperService scraperService)
    {
        _emailService = emailService;
        _scraperService = scraperService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var doc = _scraperService.ScrapeHtml();
            var game = _scraperService.GetGamePlayedInfo(doc);
            _emailService.SendEmail(game);

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}
