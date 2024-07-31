using System.Net;
using System.Text;
using Microsoft.Extensions.Options;
using SportsResults.Configurations;
using SportsResults.Mail.Models;
using SportsResults.Mail.Services;
using SportsResults.Models;
using SportsResults.Services;

namespace SportsResults.WorkerService;

/// <summary>
/// Responsible for performing an action on a Periodic Timer.
/// </summary>
public class Worker : BackgroundService
{
    #region Fields

    private readonly ILogger<Worker> _logger;
    private readonly MailOptions _mailOptions;
    private readonly ScraperServiceOptions _scraperServiceOptions;
    private readonly WorkerServiceOptions _workerServiceOptions;

    #endregion
    #region Constructors

    public Worker(ILogger<Worker> logger, IOptions<MailOptions> mailOptions, IOptions<ScraperServiceOptions> scraperServiceOptions, IOptions<WorkerServiceOptions> workerServiceOptions)
    {
        _logger = logger;
        _scraperServiceOptions = scraperServiceOptions.Value;
        _mailOptions = mailOptions.Value;
        _workerServiceOptions = workerServiceOptions.Value;
    }

    #endregion
    #region Methods - Protected

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        TimeOnly scheduledTimeOnly = new TimeOnly(_workerServiceOptions.ScheduledHour, _workerServiceOptions.ScheduledMinute);
        DateOnly scheduledDateOnly = DateOnly.FromDateTime(DateTime.Now);
        DateTime scheduledDateTime = new DateTime(scheduledDateOnly, scheduledTimeOnly);

        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
        do
        {
            _logger.LogInformation("Worker service running at: {time}", DateTime.Now);
            _logger.LogInformation("- Scheduled date time: {time}", scheduledDateTime);

            if (IsReady(scheduledDateTime))
            {
                _logger.LogInformation("Performing scheduled execution.");

                var date = GetDate();
                var games = BasketballReferenceScraperService.ScrapeBoxscores(date);
                SendEmail(date, games);

                // Increment next scheduled date by 1.
                scheduledDateTime = scheduledDateTime.AddHours(_workerServiceOptions.ScheduledInvervalInHours);

                _logger.LogInformation("- New scheduled date time: {time}", scheduledDateTime);
            }

        } while (await timer.WaitForNextTickAsync(stoppingToken));

    }

    #endregion
    #region Methods - Private

    private static string GetEmailBodyText(IReadOnlyList<Game> games)
    {
        var sb = new StringBuilder();
        if (games.Count > 0)
        {
            foreach (var game in games)
            {
                ArgumentNullException.ThrowIfNull(game.Home, nameof(game.Home));
                ArgumentNullException.ThrowIfNull(game.Away, nameof(game.Away));

                sb.AppendLine($"{game.Home.Name}: {game.Home.Score}");
                sb.AppendLine($"{game.Away.Name}: {game.Away.Score}");
                sb.AppendLine();
            }
        }
        else
        {
            sb.AppendLine("No games played on this date.");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Determines if the schedule is ready to run.
    /// </summary>
    /// <param name="scheduledDateTime">The schedule date and time to perform the task.</param>
    /// <returns>True if the scheduled date time is less than the current time.</returns>
    private static bool IsReady(DateTime scheduledDateTime)
    {
        return scheduledDateTime <= DateTime.Now;
    }

    private DateTime GetDate()
    {
        // For testing purposes only!
        // You can set the date override to a day with games, or set to a day without games.
        return _scraperServiceOptions.DateOverride
            ? new DateTime(_scraperServiceOptions.DateOverrideYear, _scraperServiceOptions.DateOverrideMonth, _scraperServiceOptions.DateOverrideDay)
            : DateTime.Now.Date;
    }

    private void SendEmail(DateTime date, IReadOnlyList<Game> games)
    {
        // Configure message.
        var subject = $"Sports Results: {date.ToShortDateString()}";
        var emailBody = new EmailBody(GetEmailBodyText(games));
        var emailMessage = new EmailMessage(_mailOptions.UserEmailAddress, _mailOptions.ToEmailAddresses, subject, emailBody);

        // Configure service.
        var credentials = new NetworkCredential(_mailOptions.UserEmailAddress, _mailOptions.UserPassword);
        var emailService = new EmailService(_mailOptions.SmtpClientHost, _mailOptions.SmtpClientPort, credentials, _mailOptions.SmtpClientEnableSsl);

        emailService.SendEmail(emailMessage);
    }

    #endregion
}
