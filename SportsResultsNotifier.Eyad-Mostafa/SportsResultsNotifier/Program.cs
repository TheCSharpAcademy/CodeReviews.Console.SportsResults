using Microsoft.Extensions.Configuration;

namespace SportsResultsNotifier;

internal class Program
{
    private static void Main(string[] args)
    {
        var config = LoadConfiguration();
        var emailSettings = config.GetSection("EmailSettings").Get<EmailSettings>();
        if (emailSettings is null)
        {
            Console.WriteLine("Email settings not found in appsettings.json");
            return;
        }

        var scraper = new GameScraper();
        (string subject, string body, bool isDataExists) = scraper.ScrapeGameResults();

        if (!isDataExists)
        {
            Console.WriteLine("No data to send.");
            return;
        }

        var emailService = new EmailService(emailSettings);
        emailService.SendEmail(subject, body);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Email Sent Successfully!");
        Console.ResetColor();
    }

    static IConfiguration LoadConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
    }
}