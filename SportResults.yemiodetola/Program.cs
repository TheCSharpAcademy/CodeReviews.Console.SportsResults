using SportResults.Services;
using SportResults.Models;
using SportResults.Mailer;
using SportResults.Helpers;

var timer = new Timer(SendDailyEmail, null, 0, 86400000);

// Keep the app running
Console.WriteLine("Press any key to stop...");
Console.ReadKey();

static void SendDailyEmail(object? state)
{
    try
    {
        string url = "https://www.basketball-reference.com/boxscores/";
        
        ScraperService scraperService = new ScraperService(url);
        List<Result> results = scraperService.ScrapGameData();
        
        string formattedResult = MailHelper.GenerateMailBody(results);
        
        Mailer.SendEmail(formattedResult);
        
        Console.WriteLine($"[{DateTime.Now}] Email sent successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[{DateTime.Now}] Error: {ex.Message}");
    }
}
