using Microsoft.Extensions.Configuration;
using SportsResults.jollejonas.Services;


public class Program
{
    private static Timer? _timer;
    public static void Main()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        Console.WriteLine("Application has started. Scheduling daily task...");

        ScheduleDailyTask(configuration);
        Console.ReadLine();
    }
    private static void ScheduleDailyTask(IConfiguration configuration)
    {
        TimeSpan runTime = new TimeSpan(8, 0, 0);
        TimeSpan currentTime = DateTime.Now.TimeOfDay;
        TimeSpan firstRun = (runTime > currentTime) ? runTime - currentTime : runTime - currentTime + new TimeSpan(24, 0, 0);

        _timer = new Timer(_ =>
        {
            ExecuteTask(configuration);
        }, null, firstRun, new TimeSpan(0, 5, 0));
    }

    private static void ExecuteTask(IConfiguration configuration)
    {
        Mail mail = new Mail(configuration);
        Scraper scraper = new Scraper();
        string message = scraper.Scrape();

        if (string.IsNullOrEmpty(message))
        {
            Console.WriteLine($"{DateTime.Now}: Error while scraping");
            return;
        }

        mail.SendMail(message);
        Console.WriteLine($"{DateTime.Now}: Email sent successfully.");
    }
}