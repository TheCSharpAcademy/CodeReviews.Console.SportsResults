using Microsoft.Extensions.Configuration;
using Spectre.Console;
using WebScraper_RyanW84.Models;

namespace WebScraper_RyanW84.Service;

public interface IScraper
{
    Task<Results> Run();
}

public interface IEmailService
{
    Task SendEmail(Results results);
}

public class TimerChecker(
    IConfiguration configuration,
    IScraper scraper,
    IEmailService emailService
)
{
    private readonly IConfiguration _configuration = configuration;
    internal Timer Checker;

    private async void TimerCallback(object state)
    {
        try
        {
            var results = await scraper.Run();
            if (results.EmailTableRows.Length is not 0)
            {
                await emailService.SendEmail(results);
                // Reset timer for next check in 24 hours
                ResetTimer();
                AnsiConsole.MarkupLine(
                    "[green]Scrape successful - Next check scheduled in 24 hours[/]"
                );
            }
            else
            {
                AnsiConsole.MarkupLine(
                    "[red]Scraper returned no results, trying again in 2 seconds[/]"
                );
                Thread.Sleep(2000); // Sleep for 2 seconds
                await scraper.Run(); // Only run again if the first attempt failed
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex}");
            // Reset timer to try again in 1 hour on error
            ResetTimer(TimeSpan.FromHours(1));
        }
    }

    private void ResetTimer(TimeSpan? interval = null)
    {
        try
        {
            // Default interval is 24 hours if not specified
            interval ??= TimeSpan.FromDays(1);

            // Dispose existing timer
            Checker?.Dispose();

            var due = DateTime.Now.Add(interval.Value);
            var span = due.Subtract(DateTime.Now);

            AnsiConsole.MarkupLine(
                "[Bold Italic Green]Resetting scraper to check at {0:dd MMM yyyy HH:mm} (~{1} hours {2} minutes)[/]",
                due,
                (int)span.TotalHours,
                span.Minutes
            );

            // Create new timer with specified interval
            Checker = new Timer(
                TimerCallback,
                null,
                (int)span.TotalMilliseconds,
                Timeout.Infinite // Use -1 to ensure timer only runs once per interval
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error resetting timer: {ex}");
        }
    }

    public Task SetTimer()
    {
        // Initial timer setup - start in 10 seconds
        ResetTimer(TimeSpan.FromSeconds(10));
        return Task.CompletedTask;
    }
}
