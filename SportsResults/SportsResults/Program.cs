using Microsoft.Extensions.Configuration;
using SportsResults.Models;
using SportsResults.Services;

internal class Program
{
    static async Task Main()
    {
        try
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            ScraperService scraperService = new ScraperService(config);
            EmailService emailService = new EmailService(config);

            while (true)
            {
                List<SportsResult> results = await scraperService.Run();

                await emailService.SendMessage(results);

                Console.WriteLine("Scheduling the next parsing in 24 hours...");
                await Task.Delay(TimeSpan.Parse(config["Schedule"]));
            }
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
        }

    }
}