using SportsResults.KamilKolanowski.Services;

namespace SportsResults.KamilKolanowski.Controllers;

public class SportsNotifierController
{
    internal void SendMessageWithSportsResults()
    {
        MailService mail = new MailService();

        mail.SendMail("<your_recipient's mail>");
    }
}
