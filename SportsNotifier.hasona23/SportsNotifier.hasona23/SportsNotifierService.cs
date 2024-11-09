using SportsNotifier.hasona23.EmailSender;

namespace SportsNotifier.hasona23;

public class SportsNotifierService : BackgroundService
{
    private readonly ILogger<SportsNotifierService> _logger;
    private const int DurationMinutes = 60;
    private Scrapper.Scrapper _scrapper;
    private EmailSender.EmailSender _emailSender;
    private string[] _recipients = ["abc@abc.com","foo@foo.com"];
    
    public SportsNotifierService(EmailSender.EmailSender emailSender,Scrapper.Scrapper scrapper,ILogger<SportsNotifierService> logger)
    {
        _logger = logger;
        _scrapper = scrapper;
        _emailSender = emailSender;
        _logger.LogInformation("Starting service");
       
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var data = _scrapper.Scrape();
                if (data.Games.Count != 0)
                {
                    var body = EmailBuilder.BuildMessage(data);
                    foreach (var recipient in _recipients)
                        _emailSender.SendEmail(recipient, data.Title, body);
                    _logger.LogInformation("Ending service");
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(DurationMinutes), stoppingToken);
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        _emailSender.Dispose();
    }
}