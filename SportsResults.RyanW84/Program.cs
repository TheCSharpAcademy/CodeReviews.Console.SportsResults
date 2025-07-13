using Microsoft.Extensions.Configuration;

using Spectre.Console;

using WebScraper_RyanW84.Service;

namespace WebScraper_RyanW84;

internal class Program
{
    private static async Task Main(string[] args) // -h for Halestorm Gigs (set) or -b for Basketball Results
    {
        Helpers helpers = new();
      
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .AddUserSecrets<Program>()
            .Build();

        if (args.Length == 0)
            throw new ArgumentException(
                "Please specify which scraper to use: '-h' for Halestorm Gigs or '-b' for Basketball Results.");

        IScraper scraper = args[0].ToLowerInvariant() switch
        {
            "-h" => new HalestormScraper(helpers),
            "-b" => new BasketballScraper(helpers),
            _ => throw new ArgumentException("Invalid scraper selection. Use '-h' or '-b'.")
        };

        IEmailService emailService = new Email(config, scraper);

        var timerChecker = new TimerChecker(config, scraper, emailService);
		await timerChecker.SetTimer();

		// Prevent app from exiting
		AnsiConsole.MarkupLine("[bold red]Press any key to exit[/]");
        Console.ReadLine();
    }
}