using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SportsResults;

public class SportsController : BackgroundService
{
    private readonly ILogger<SportsController> _logger;
    private Timer _timer;
    private ScrappingService _scrappingService;
    private EmailService _emailService;

    public SportsController(ILogger<SportsController> logger, ScrappingService scrappingService, EmailService emailService)
    {
        _logger = logger;
        _scrappingService = scrappingService;
        _emailService = emailService;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(ReportResults, null, TimeSpan.Zero, TimeSpan.FromDays(1));
        return Task.CompletedTask;
    }

    private void ReportResults(object state)
    {
        _logger.LogInformation("Sending email at: {time}", DateTimeOffset.Now);

        var subject = _scrappingService.GetTitle();
        var body = _scrappingService.GetResults();
        _emailService.SendEmail(subject, body);
    }

    public override Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("The service is stopping.");
        _timer?.Change(Timeout.Infinite, 0);
        return base.StopAsync(stoppingToken);
    }

    public override void Dispose()
    {
        _timer?.Dispose();
        base.Dispose();
    }
}