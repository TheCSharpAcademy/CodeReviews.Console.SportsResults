namespace SportsResultsNotifier.UI.Services;

public class NotificationService
{
    private readonly ScraperService _scraper;
    private readonly EmailService _emailService;

    public NotificationService(ScraperService scraper, EmailService emailService)
    {
        _scraper = scraper;
        _emailService = emailService;
    }

    public async Task RunAsync(string to)
    {
        var results = await _scraper.GetResultsAsync();
        await _emailService.SendEmailAsync(
            to: to,
            subject: "Sports Results Notifier",
            body: results
            );
    }
}
