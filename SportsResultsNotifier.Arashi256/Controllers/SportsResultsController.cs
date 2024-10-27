using Microsoft.Extensions.Logging;
using SportsResultsNotifier.Arashi256.Services;
using SportsResultsNotifier.Arashi256.Classes;
using SportsResultsNotifier.Arashi256.Models;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace SportsResultsNotifier.Arashi256.Controllers
{
    internal class SportsResultsController : BackgroundService
    {
        private readonly ContactUtils _contactUtils;
        private readonly WebScraperService _scraperService;
        private readonly EmailService _emailService;
        private readonly ILogger<SportsResultsController> _logger;

        public SportsResultsController(ContactUtils contactUtils, WebScraperService scraperService, EmailService emailService, ILogger<SportsResultsController> logger)
        {
            _scraperService = scraperService;
            _emailService = emailService;
            _logger = logger;
            _contactUtils = contactUtils;
        }

        private async Task Run()
        {
            _logger.LogInformation("Starting controller task...");
            try
            {
                _logger.LogInformation("Scraping web data...");
                var response = await _scraperService.ScrapeDataAsync();
                if (response != null)
                {
                    if (response.Status.Equals(ResponseStatus.Success)) 
                    {
                        _logger.LogInformation("Web data response success!");
                        var data = response.Data as List<TeamGame>;
                        if (data != null)
                        {
                            _logger.LogInformation("Got sensible data back from the web scraper for email package");
                            var contacts = _contactUtils.GetContacts();
                            _logger.LogInformation("Sending email...");
                            response = await _emailService.SendEmailAsync(contacts["Sender"], contacts["Receiver"], "Daily Sports Results!", FormatResultsData(data));
                            if (response.Status.Equals(ResponseStatus.Success))
                                _logger.LogInformation($"Email sending success: '{response.Message}'");
                            else
                                _logger.LogInformation($"Email sending failed: '{response.Message}'");
                        }
                    }
                    else
                    {
                        _logger.LogError($"ERROR: '{response.Message}'");
                    }
                }
                else
                {
                    _logger.LogError("ERROR: Could not retrieve any response");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private string FormatResultsData(List<TeamGame> results)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\nHere is your daily sports results:\n");
            foreach (var game in results)
            {
                sb.AppendLine(game.ToString());
            }
            return sb.ToString();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Run the controller
                _logger.LogInformation("Waking up to get sports data");
                await Run();
                _logger.LogInformation("Going back to sleep");
                // Wait for 24 hours
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);

            }
        }
    }
}
