using Microsoft.Extensions.Configuration;
namespace SportsResultsNotifier;

public class Program
{
    static async Task Main()
    {
        var scraper = new WebScraper();
        await scraper.LoadWebDocument();
        string text = scraper.FetchData();
        var config = new ConfigurationBuilder().AddJsonFile("email-creds.json", optional: false, reloadOnChange: true).Build();
        var emailSender = new EmailSender(config);
        emailSender.SendEmail(text);
        Console.ReadKey();
    }
}
