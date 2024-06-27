using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Phonebook.Services;
using SportsResult;

namespace SportsResults;

public class SportsResultsBackgroundService : BackgroundService
{
    private readonly Scrapper _scrapper;
    private readonly EmailService _emailService;
    private readonly ILogger<SportsResultsBackgroundService> _logger;

    public SportsResultsBackgroundService(ILogger<SportsResultsBackgroundService> logger, Scrapper scrapper, EmailService emailService)
    {
        _logger = logger;
        _scrapper = scrapper;
        _emailService = emailService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Sports Result Background Service is starting");
        while (!stoppingToken.IsCancellationRequested)
        {
            var currentTime = DateTime.Now;
            var nextRunTime = DateTime.Today.AddDays(1).AddHours(0).AddMinutes(0).AddSeconds(0); // next 00:00:00
            var timeToWait = nextRunTime - currentTime;

            if (timeToWait < TimeSpan.Zero)
            {
                nextRunTime = nextRunTime.AddDays(1);
                timeToWait = nextRunTime - currentTime;
            }

            _logger.LogInformation($"Background Service will wait for {timeToWait} to run the task.");
            await Task.Delay(timeToWait, stoppingToken);

            if (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Background Service is running the scheduled task at: {time}", DateTimeOffset.Now);
                _scrapper.ScrapeMatchData();
                var emailBody = _scrapper.GetTeamScoresAsHtml();
                await _emailService.SendEmailAsHtml(_scrapper.MatchDate, emailBody);
            }
        }
        _logger.LogInformation("Sports Result Background Service is stoping");

    }
}