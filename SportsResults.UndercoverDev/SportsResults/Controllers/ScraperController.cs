using Microsoft.Extensions.Logging;
using SportsResults.Services;

namespace SportsResults.Controllers;
public class ScraperController
{
    private readonly ScraperService _scraperService;
    private readonly EmailService _emailService;
    private readonly ILogger<ScraperController> _logger;

    public ScraperController(ScraperService scraperService, EmailService emailService, ILogger<ScraperController> logger)
    {
        _scraperService = scraperService;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Run()
    {
        try
        {
            var data = await _scraperService.ScrapeSportsDataAsync();
            await _emailService.SendSportsDataEmailAsync(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scraping and sending email");
        }
    }
}