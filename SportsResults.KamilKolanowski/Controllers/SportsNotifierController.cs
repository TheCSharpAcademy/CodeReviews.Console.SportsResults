using SportsResults.KamilKolanowski.Services;

namespace SportsResults.KamilKolanowski.Controllers;

public class SportsNotifierController
{
    static Timer _timer;
    
    internal void SendMessageWithSportsResults(string mailAddress)
    {
        MailService mail = new MailService();

        mail.SendMail(mailAddress);
    }

    internal void TriggerSportsResultsMessageSenderAsync(string mailAddress)
    {
        
        TimeSpan initialDelay = TimeSpan.Zero;
        TimeSpan interval = TimeSpan.FromSeconds(86400); // 24 hours in seconds

        _timer = new Timer(_ => SendMessageWithSportsResults(mailAddress), null, initialDelay, interval);
        Console.ReadLine();
    }
}
