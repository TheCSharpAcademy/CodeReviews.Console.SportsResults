using System.Configuration;

namespace SportsResults.wkktoria.Services;

public static class NotifierService
{
    private const string DailyTime = "12:00:00";
    private static readonly string[] TimeParts = DailyTime.Split(':');
    private static readonly DateTime DateNow = DateTime.Now;

    private static DateTime _date = new(DateNow.Year, DateNow.Month, DateNow.Day, int.Parse(TimeParts[0]),
        int.Parse(TimeParts[1]),
        int.Parse(TimeParts[2]));

    private static readonly string ReceiverEmail = ConfigurationManager.AppSettings.Get("ReceiverEmail")!;
    private static readonly string SenderEmail = ConfigurationManager.AppSettings.Get("SenderEmail")!;
    private static readonly string SenderPassword = ConfigurationManager.AppSettings.Get("SenderPassword")!;


    public static void Run()
    {
        TimeSpan ts;

        if (_date > DateNow)
        {
            ts = _date - DateNow;
        }
        else
        {
            _date = _date.AddDays(1);
            ts = _date - DateNow;
        }

        Task.Delay(ts).ContinueWith(_ =>
        {
            try
            {
                Mailer.SendEmail(SenderEmail, SenderPassword, ReceiverEmail);
                Console.WriteLine("Email sent!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to send email: {e.Message}");
            }
        });


        Console.WriteLine("Press any key to stop service...");
        Console.ReadKey();
    }
}