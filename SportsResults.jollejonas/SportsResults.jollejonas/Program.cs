using Microsoft.Extensions.Configuration;
using SportsResults.jollejonas.Services;


public class Program
{
    public static void Main()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        Mail mail = new Mail(configuration);
        Scraper scraper = new Scraper();
        string message = scraper.Scrape();

        if (message == "")
        {
            Console.WriteLine("Error while scraping");
            return;
        }
        mail.SendMail(message);
    }
}