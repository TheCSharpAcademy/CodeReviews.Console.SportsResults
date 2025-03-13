using Microsoft.Extensions.Configuration;

namespace SportsResultsNotifier;

internal class Program
{
    private static async Task Main(string[] args)
    {
        while (true)
        {
            // Run at 7 AM daily
            var now = DateTime.Now;
            var nextRunTime = new DateTime(now.Year, now.Month, now.Day, 7, 0, 0)
                                .AddDays(now.Hour >= 7 ? 1 : 0);

            var delay = nextRunTime - now;
            Console.WriteLine($"Next run scheduled at: {nextRunTime}");

            await Task.Delay(delay);

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
            }
            else
            {
                var emailService = new EmailService(emailSettings);
                emailService.SendEmail(subject, body);
                Console.WriteLine("Email Sent Successfully!");
            }
        }
    }
    static IConfiguration LoadConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
    }
}
