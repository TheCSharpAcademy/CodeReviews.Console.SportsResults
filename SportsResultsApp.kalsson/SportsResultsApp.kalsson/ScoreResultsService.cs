using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SportsResultsApp.kalsson.Services;

namespace SportsResultsApp.kalsson;

internal class ScoreResultsService : BackgroundService
{
    private readonly EmailSender _emailSender;
    private readonly DataScraper _scraper;
    private readonly string _recipientEmail;
    private readonly TimeSpan _interval;

    public ScoreResultsService(IConfiguration configuration, DataScraper scraper, EmailSender emailSender)
    {
        _scraper = scraper;
        _emailSender = emailSender;
        _interval = TimeSpan.FromDays(1);

        _recipientEmail = configuration["Email:RecipientEmail"];
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await RunTask(); // Run the task immediately

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_interval, stoppingToken);

            await RunTask();
        }
    }

    private async Task RunTask()
    {
        try
        {
            Console.WriteLine("Scraping basketball game data...");

            var games = await _scraper.ScrapeBasketballGamesAsync();
            if (games.Count == 0)
            {
                Console.WriteLine("No games found.");
                return;
            }

            Console.WriteLine($"Found {games.Count} game(s).");

            await _emailSender.SendEmailAsync(games);
            Console.WriteLine($"Email sent to {_recipientEmail}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}