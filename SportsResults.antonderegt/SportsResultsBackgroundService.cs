using Microsoft.Extensions.Hosting;

namespace SportsResults;

public class SportsResultsBackgroundService : BackgroundService
{
    private readonly Scraper _scraper;
    private readonly BodyBuilder _bodyBuilder;
    private readonly Mailer _mailer;
    private readonly TimeSpan _interval;

    public SportsResultsBackgroundService()
    {
        _scraper = new();
        _bodyBuilder = new();
        _mailer = new();
        _interval = new TimeSpan(24, 0, 0);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var games = _scraper.GetResults();
        var body = _bodyBuilder.BuilderMessageBody(games);
        _mailer.SendEmail("Sports Results", body);
        await Task.Delay(_interval, stoppingToken).ConfigureAwait(false);
        await ExecuteAsync(stoppingToken);
    }
}