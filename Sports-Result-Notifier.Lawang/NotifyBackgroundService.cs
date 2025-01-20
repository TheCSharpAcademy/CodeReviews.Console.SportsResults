using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Sports_Result_Notifier.Lawang;

public class NotifyBackgroundService : BackgroundService
{
    private WebScrapper _web;
    private MailSender _emailSender;
    private ILogger<NotifyBackgroundService> _logger;

    // Change FromHours to FromSeconds for testing purposes
    private TimeSpan _timeSpan = TimeSpan.FromHours(24);

    // Add new recievers email addresses
    private List<string> recievers = new() { "lawanglama781@gmail.com"};
    public NotifyBackgroundService(ILogger<NotifyBackgroundService> logger)
    {
        _web = new WebScrapper($"https://www.basketball-reference.com/boxscores/");
        _emailSender = new MailSender();
        _logger = logger;

    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // scrapping the result from this date
                var date = DateTime.Now;
                _web.SetDateUrl(date);

                var title = _web.GetTitle();

                var result = _web.GetAllResults();
                var message = EmailBuilder.BuildEmail(result, title);
                
                foreach (var email in recievers)
                {
                    _logger.LogInformation($"Sending Email to {email}");
                    _emailSender.SendEmail(message, email);
                }
                _logger.LogInformation($"All Email Sent for {DateTime.Now.ToShortDateString()}");
                await Task.Delay(_timeSpan, stoppingToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
